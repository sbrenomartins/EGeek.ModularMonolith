using EGeek.Catalog.DTOs.Requests;
using System.ComponentModel.DataAnnotations;

namespace EGeek.Catalog.Domain.Entities;

internal class Product
{
    [Key]
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int QuantityInStock { get; private set; }
    public int WeightInGrams { get; private set; }
    public int HeightInCentimeters { get; private set; }
    public int WidthInCentimeters { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Product() { }

    internal Product(PostProductRequest request, string email)
    {
        if (string.IsNullOrEmpty(request.Name))
            throw new ArgumentException("Product name is required.");
        if (string.IsNullOrEmpty(request.Description))
            throw new ArgumentException("Product description is required.");
        if (request.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero.");
        if (request.QuantityInStock < 0)
            throw new ArgumentException("Product quantity in stock must be equal or greater than zero.");
        if (request.WeightInGrams <= 0)
            throw new ArgumentException("Product weight in grams must be greater than zero.");
        if (request.HeightInCentimeters <= 0)
            throw new ArgumentException("Product weight in grams must be greater than zero.");
        if (request.WidthInCentimeters <= 0)
            throw new ArgumentException("Product weight in grams must be greater than zero.");

        Id = Guid.CreateVersion7();
        Name = request.Name;
        Description = request.Description;
        Price = request.Price;
        QuantityInStock = request.QuantityInStock;
        WeightInGrams = request.WeightInGrams;
        HeightInCentimeters = request.HeightInCentimeters;
        WidthInCentimeters = request.WidthInCentimeters;
        CreatedBy = email;
        CreatedAt = DateTime.UtcNow;
    }
}
