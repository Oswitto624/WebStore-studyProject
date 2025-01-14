﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities;

[Index(nameof(Name), IsUnique = false)]
public class Section : NamedEntity, IOrderedEntity  
{
    public int Order { get; set; }

    public int? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public Section? Parent { get; set; } 

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}
