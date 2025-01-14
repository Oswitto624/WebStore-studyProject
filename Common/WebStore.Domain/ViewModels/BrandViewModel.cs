﻿namespace WebStore.Domain.ViewModels;

public class BrandViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProductsCount { get; set; }
}