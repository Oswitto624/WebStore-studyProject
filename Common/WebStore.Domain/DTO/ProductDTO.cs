﻿using System.Diagnostics.CodeAnalysis;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO;

public class ProductDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public SectionDTO Section { get; set; } = null!;

    public BrandDTO? Brand { get; set; }

    public string ImageUrl { get; set; } = null!;

    public decimal Price { get; set; }
}

public class SectionDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public int? ParentId { get; set; }
}

public class BrandDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public int ProductsCount { get; set; }
}

public static class BrandDTOMapper
{
    [return: NotNullIfNotNull("brand")]
    public static BrandDTO? ToDTO(this Brand? brand) => brand is null
        ? null
        : new BrandDTO
        {
            Id = brand.Id,
            Name = brand.Name,
            Order = brand.Order,
            ProductsCount = brand.Products.Count,
        };

    [return: NotNullIfNotNull("brand")]
    public static Brand? FromDTO(this BrandDTO? brand) => brand is null
            ? null
            : new Brand
            {
                Id = brand.Id,
                Name = brand.Name,
                Order = brand.Order,
                Products = new Product[brand.ProductsCount],
            };

    public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand>? brands) => brands?.Select(ToDTO)!;
    public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDTO>? brands) => brands?.Select(FromDTO)!;
}

public static class SectionDTOMapper
{
    [return: NotNullIfNotNull("section")]
    public static SectionDTO? ToDTO(this Section? section) => section is null
        ? null
        : new SectionDTO
        {
            Id = section.Id,
            Name = section.Name,
            Order = section.Order,
            ParentId = section.ParentId,
        };

    [return: NotNullIfNotNull("section")]
    public static Section? FromDTO(this SectionDTO? section) => section is null
            ? null
            : new Section
            {
                Id = section.Id,
                Name = section.Name,
                Order = section.Order,
                ParentId= section.ParentId,
            };

    public static IEnumerable<SectionDTO> ToDTO(this IEnumerable<Section>? sections) => sections?.Select(ToDTO)!;
    public static IEnumerable<Section> FromDTO(this IEnumerable<SectionDTO>? sections) => sections?.Select(FromDTO)!;
}

public static class ProductDTOMapper
{
    [return: NotNullIfNotNull("product")]
    public static Page<ProductDTO>? ToDTO(this Page<Product>? page) => page is null
        ? null
        : new(page.Items.ToDTO(), page.PageNumber, page.PageSize, page.TotalCount);

    [return: NotNullIfNotNull("product")]
    public static Page<Product>? FromDTO(this Page<ProductDTO>? page) => page is null
        ? null
        : new(page.Items.FromDTO(), page.PageNumber, page.PageSize, page.TotalCount);

    [return: NotNullIfNotNull("product")]
    public static ProductDTO? ToDTO(this Product? product) => product is null
        ? null
        : new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Order = product.Order,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand.ToDTO(),
            Section = product.Section.ToDTO(),
        };

    [return: NotNullIfNotNull("product")]
    public static Product? FromDTO(this ProductDTO? product) => product is null
    ? null
    : new Product
    {
        Id = product.Id,
        Name = product.Name,
        Order = product.Order,
        Price = product.Price,
        ImageUrl = product.ImageUrl,
        Brand = product.Brand.FromDTO(),
        Section = product.Section.FromDTO(),
    };

    public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product>? sections) => sections?.Select(ToDTO)!;
    public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO>? sections) => sections?.Select(FromDTO)!;
}