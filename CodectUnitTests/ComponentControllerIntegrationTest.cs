using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using BLL.Models;
using Codect.Classes;
using DAL;
using DTO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace CodectUnitTests;

[TestClass]
public class ComponentControllerIntegrationTest : IClassFixture<WebApplicationFactory<StartUp>>
{
	private readonly HttpClient _Client;

	public ComponentControllerIntegrationTest(WebApplicationFactory<StartUp> factory)
	{
		_Client = factory.CreateClient();
	}

	public CodectEfCoreDbContext createDbContext()
	{
		var connection = new SqliteConnection("DataSource=:memory:");
		connection.Open(); // SQLite requires the connection to remain open for the database to persist

		var options = new DbContextOptionsBuilder<CodectEfCoreDbContext>()
			.UseSqlite(connection)
			.Options;

		// Create the schema for testing
		var context = new CodectEfCoreDbContext(options);
		context.Database.EnsureCreated();

		return context;
	}

	public void UploadNewComponent(CodectEfCoreDbContext _context)
	{
		List<ContactPoint> listContactPoints = new()
		{
			ContactPoint.E,
			ContactPoint.N,
			ContactPoint.S,
			ContactPoint.W
		};

		Component component = new(listContactPoints, "RedLed");
		
		_context.Components.Add(component);

		_context.SaveChanges();
	}

	public void ClearDatabase(CodectEfCoreDbContext _context)
	{

		_context.Database.ExecuteSqlRaw("DELETE FROM Components");

		_context.SaveChanges();
	}

	[Fact]
	public async Task GetFeatureList_returns_list_of_strings()
	{

		//Act
		var response = await _Client.GetAsync("https://localhost:7278/features");

		//Assert
		response.EnsureSuccessStatusCode();

		var featureList = response.Content.ReadFromJsonAsync<List<string>>();
		featureList?.Id.Should().BePositive();
		featureList.Result.Count.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task GetComponentImage_returns_a_valid_svg()
	{
		//Arrange
		string id = "1111RedLed";
		using var _context = createDbContext();
		ClearDatabase(_context);
		UploadNewComponent(_context);

		//Act
		var response = await _Client.GetAsync($"https://localhost:7278/components/{id}/Image");

		//Assert
		response.EnsureSuccessStatusCode();

		var image = response.Content.ReadAsStringAsync();
		image?.Result.Should().Contain("svg");

		ClearDatabase(_context);
	}

	[Fact]
	public async Task GetComponentImage_returns_error_when_id_does_not_exists()
	{
		//Arrange
		string id = "fakeId";

		//Act
		var response = await _Client.GetAsync($"https://localhost:7278/components/{id}/Image");

		//Assert
		response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
		
		var error = await response.Content.ReadAsStringAsync();
		error.Should().Contain("Component does not exist in database");
	}

	[Fact]
	public async Task GetAllComponentIds_returns_a_list_strings()
	{
		//Arrange
		using var _context = createDbContext();
		ClearDatabase(_context);
		UploadNewComponent(_context);

		//Act
		var response = await _Client.GetAsync("https://localhost:7278/components/ids");

		//Assert
		response.EnsureSuccessStatusCode();

		var featureList = response.Content.ReadFromJsonAsync<List<string>>();
		featureList?.Id.Should().BePositive();
		featureList.Result.Count.Should().BeGreaterThan(0);

		ClearDatabase(_context);
	}

	[Fact]
	public async Task GetComponentInfo_returns_a_dictionary_with_expected_data_when_given_a_valid_id()
	{
		//Arrange
		string id = "1111RedLed";
		using var _context = createDbContext();
		ClearDatabase(_context);
		UploadNewComponent(_context);

		//Act
		var response = await _Client.GetAsync($"https://localhost:7278/components/{id}");

		//Assert
		var info = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
		info?["endPoints"].Should().Be("N,E,S,W");
		info["description"].Should().Be("An simple red led light will light up when provided with power");
		info["feature"].Should().Be("RedLed");

		ClearDatabase(_context);
	}

	[Fact]
	public async Task GetComponentInfo_returns_an_exception_when_given_an_invalid_id()
	{
		//Arrange
		string id = "FakeId";

		//Act
		var response = await _Client.GetAsync($"https://localhost:7278/components/{id}");

		//Assert
		response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

		var error = await response.Content.ReadAsStringAsync();
		error.Should().Contain("Component does not exist in database");
	}

	[Fact]
	public async Task ComponentCreate_should_insert_a_component_in_the_database_when_provided_with_correct_data()
	{
		//Arrange
		using var _context = createDbContext();
		//ClearDatabase(_context);

		List<string> contactPoints = new()
		{
			"N",
			"E"
		};
		string feature = "RedLed";
		ComponentDTO componentDTO = new()
		{
			contactPoints = contactPoints,
			feature = feature
		};

		var jsonComponentDto = new StringContent(
			JsonSerializer.Serialize(componentDTO),
			Encoding.UTF8,
			"application/json");

		ComponentRepository componentRepository = new ComponentRepository(_context);

		//Act
		var response = await _Client.PostAsync("https://localhost:7278/components/new-component"
			, jsonComponentDto);

		//Assert

		var error = await response.Content.ReadAsStringAsync();
		response.EnsureSuccessStatusCode();

		var id = await response.Content.ReadAsStringAsync();
		id?.Should().Be("1100RedLed");

		componentRepository.IdExistsInDatabase(id).Should().Be(true);

		ClearDatabase(_context);
	}

	[Fact]
	public async Task ComponentCreate_should_return_an_exception_when_feature_is_incorrect()
	{
		//Arrange
		using var _context = createDbContext();
		List<string> contactPoints = new()
		{
			"N",
			"E"
		};
		string feature = "Incorrect feature";
		ComponentDTO componentDTO = new()
		{
			contactPoints = contactPoints,
			feature = feature
		};

		var jsonComponentDto = new StringContent(
			JsonSerializer.Serialize(componentDTO),
			Encoding.UTF8,
			"application/json");

		ComponentRepository componentRepository = new ComponentRepository(_context);

		//Act
		var response = await _Client.PostAsync("https://localhost:7278/components/new-component"
			, jsonComponentDto);

		//Arrange
		response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

		var error = await response.Content.ReadAsStringAsync();
		error.Should().Contain("feature has to exist in dictionary");

		componentRepository.IdExistsInDatabase("1100Incorect feature").Should().Be(false);
	}

	[Fact]
	public async Task ComponentCreate_should_return_an_exception_when_there_are_more_than_4_contact_points()
	{
		//Arrange
		using var _context = createDbContext();
		List<string> contactPoints = new()
		{
			"N",
			"E",
			"S",
			"W",
			"N"
		};
		string feature = "RedLed";
		ComponentDTO componentDTO = new()
		{
			contactPoints = contactPoints,
			feature = feature
		};

		var jsonComponentDto = new StringContent(
			JsonSerializer.Serialize(componentDTO),
			Encoding.UTF8,
			"application/json");

		ComponentRepository componentRepository = new ComponentRepository(_context);

		//Act
		var response = await _Client.PostAsync("https://localhost:7278/components/new-component"
			, jsonComponentDto);

		//Arrange
		response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

		var error = await response.Content.ReadAsStringAsync();
		error.Should().Contain("There can be no more than 4 contact points");

		componentRepository.IdExistsInDatabase("11111RedLed").Should().Be(false);
	}

	[Fact]
	public async Task ComponentCreate_should_return_an_exception_when_there_are_less_than_2_contact_points()
	{
		//Arrange
		using var _context = createDbContext();
		List<string> contactPoints = new()
		{
			"N"
		};
		string feature = "RedLed";
		ComponentDTO componentDTO = new()
		{
			contactPoints = contactPoints,
			feature = feature
		};

		var jsonComponentDto = new StringContent(
			JsonSerializer.Serialize(componentDTO),
			Encoding.UTF8,
			"application/json");

		ComponentRepository componentRepository = new ComponentRepository(_context);

		//Act
		var response = await _Client.PostAsync("https://localhost:7278/components/new-component"
			, jsonComponentDto);

		//Arrange
		response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

		var error = await response.Content.ReadAsStringAsync();
		error.Should().Contain("There can be no less than 2 contact points");

		componentRepository.IdExistsInDatabase("1000RedLed").Should().Be(false);
	}

	[Fact]
	public async Task ComponentCreate_should_return_an_exception_when_contact_points_are_invalid()
	{
		//Arrange
		using var _context = createDbContext();
		List<string> contactPoints = new()
		{
			"Invalid contact point",
			"Invalid contact point"
		};
		string feature = "RedLed";
		ComponentDTO componentDTO = new()
		{
			contactPoints = contactPoints,
			feature = feature
		};

		var jsonComponentDto = new StringContent(
			JsonSerializer.Serialize(componentDTO),
			Encoding.UTF8,
			"application/json");

		ComponentRepository componentRepository = new ComponentRepository(_context);

		//Act
		var response = await _Client.PostAsync("https://localhost:7278/components/new-component"
			, jsonComponentDto);

		//Arrange
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		var error = await response.Content.ReadAsStringAsync();
		error.Should().Contain("The provided contact points are not valid");

		componentRepository.IdExistsInDatabase("0000RedLed").Should().Be(false);
	}
}