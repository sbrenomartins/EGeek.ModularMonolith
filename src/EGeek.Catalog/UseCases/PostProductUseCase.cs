using EGeek.Catalog.Domain.Entities;
using EGeek.Catalog.DTOs.Requests;
using EGeek.Catalog.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace EGeek.Catalog.UseCases;

internal static class PostProductUseCase
{
    [Authorize(Roles = "catalog")]
    public static async Task<Created<Guid>> Action(PostProductRequest request, ClaimsPrincipal principal, CatalogDbContext context)
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        var product = new Product(request, email);

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return TypedResults.Created($"/v1/catalog/products/{product.Id}", product.Id);
    }
}
