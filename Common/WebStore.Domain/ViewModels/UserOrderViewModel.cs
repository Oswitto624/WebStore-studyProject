﻿namespace WebStore.Domain.ViewModels;

public class UserOrderViewModel
{
    public int Id { get; set; }

    public string Phone { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string? Description { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTimeOffset Date { get; set; }
}
