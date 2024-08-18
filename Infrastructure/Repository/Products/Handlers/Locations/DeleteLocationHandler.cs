using System;
using Application.DTO.Response;
using Application.Service.Products.Commands.Locations;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Products.Handlers.Locations;

public class DeleteLocationHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<DeleteLocationCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var dbContext = contextFactory.CreateDbContext();
            var data = await dbContext.Locations.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (data == null) return GeneralDbResponses.ItemNotFound("Location");
            dbContext.Locations.Remove(data);
            await dbContext.SaveChangesAsync(cancellationToken);
            return GeneralDbResponses.ItemDeleted("Location");
           
        }
        catch (Exception ex)
        {
            return new ServiceResponse(true, ex.Message);
        }
    }
}
