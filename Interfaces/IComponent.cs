﻿using DTO;

namespace Interfaces;

public interface IComponent
{
	List<ComponentDTO> GetAllComponents();
	ComponentDTO? GetComponentByName(string name);
	void InsertComponentInDatabase(string name, List<int> contactPoints, int feature);
	bool NameExistsInDatabase(string name);
}