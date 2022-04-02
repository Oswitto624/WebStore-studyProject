﻿using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InMemory;

[Obsolete("Устарел, использовать SqlProductData", error: false)]
public class InMemoryProductData : IProductData
{
    public IEnumerable<Section> GetSections() => TestData.Sections;
    public Section? GetSectionById(int Id) => TestData.Sections.FirstOrDefault(s => s.Id == Id);

    public IEnumerable<Brand> GetBrands() => TestData.Brands;
    public Brand? GetBrandById(int Id) => TestData.Brands.FirstOrDefault(b => b.Id == Id);

    public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
    {
        IEnumerable<Product> query = TestData.Products;

        //if (Filter != null && Filter.SectionId != null)
        //    query = query.Where(p => p.SectionId == Filter.SectionId);

        if(Filter?.SectionId is { } section_id)
            query = query.Where(p => p.SectionId == Filter.SectionId);

        if(Filter?.BrandId is { } brand_id)
            query = query.Where(p => p.BrandId == Filter.BrandId);

        return query;
    }
    public Product? GetProductById(int Id) => TestData.Products.FirstOrDefault(p => p.Id == Id);

}
