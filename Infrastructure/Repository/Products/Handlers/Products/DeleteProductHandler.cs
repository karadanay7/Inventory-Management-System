using System;
using Application.DTO.Response;
using Application.Service.Products.Commands.Products;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Products.Handlers.Products;

public class DeleteProductHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<DeleteProductCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var dbContext = contextFactory.CreateDbContext();
            var data = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (data == null) return GeneralDbResponses.ItemNotFound("Product");
            dbContext.Products.Remove(data);
            await dbContext.SaveChangesAsync(cancellationToken);
            return GeneralDbResponses.ItemDeleted("Product");
           
        }
        catch (Exception ex)
        {
            return new ServiceResponse(true, ex.Message);
        }
    }
}
