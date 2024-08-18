using System;
using Application.DTO.Response;
using Application.Service.Products.Commands.Products;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Products.Handlers.Products;

public class CreateProductHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<CreateProductCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var dbContext = contextFactory.CreateDbContext();
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(request.ProductModel.Name.ToLower()), cancellationToken: cancellationToken);
            if (product != null)
                return GeneralDbResponses.ItemAlreadyExists(request.ProductModel.Name);
            var data = request.ProductModel.Adapt(new Product());
            dbContext.Products.Add(data);
            await dbContext.SaveChangesAsync(cancellationToken);
            return GeneralDbResponses.ItemCreated(request.ProductModel.Name);

        }
        catch (Exception ex)

        {
            return new ServiceResponse(true, ex.Message);
        }

    }
}
