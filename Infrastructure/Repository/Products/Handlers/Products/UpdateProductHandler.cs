using System;
using Application.DTO.Response;
using Application.Service.Products.Commands.Products;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Products.Handlers.Products;

public class UpdateProductHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<UpdateProductCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var dbContext = contextFactory.CreateDbContext();
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id.Equals(request.ProductModel.Id), cancellationToken: cancellationToken);
            if (product == null) return GeneralDbResponses.ItemNotFound("Product");
            dbContext.Entry(product).State = EntityState.Detached;
            var adaptData = request.ProductModel.Adapt(new Product());
            await dbContext.SaveChangesAsync(cancellationToken);
            return GeneralDbResponses.ItemUpdated(request.ProductModel.Name);
        }
        catch (Exception ex)
        {
            return new ServiceResponse(true, ex.Message);
        }
    }
}

