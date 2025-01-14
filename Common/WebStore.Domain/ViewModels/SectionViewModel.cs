﻿namespace WebStore.Domain.ViewModels;

public class SectionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Order { get; set; }
    public List<SectionViewModel> ChildSections { get; set; } = new();
}
