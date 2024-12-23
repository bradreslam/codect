﻿using DTO;

namespace Interfaces;

public interface IComponent
{
	List<string> GetAllComponentIds();
	string InsertComponentInDatabase(List<string> contactPoints, string feature);
	bool IdExistsInDatabase(string id);
	ComponentDTO GetComponentBasedOnId(string id);
}