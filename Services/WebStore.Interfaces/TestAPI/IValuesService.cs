﻿namespace WebStore.Interfaces.TestAPI;

public interface IValuesService
{
    IEnumerable<string> GetValues();
    int Count();
    string? GetById(int id);
    void Add(string value);
    void Edit(int id, string value);
    bool Delete(int id);
}
