using System;
using Application.DTO.Response;
using Application.Service.Products.Commands.Locations;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Products.Handlers.Locations;

public class CreateLocationHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<CreateLocationCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var dbContext = contextFactory.CreateDbContext();
            var location = await dbContext.Locations.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(request.LocationModel.Name.ToLower()), cancellationToken: cancellationToken);
            if (location != null)
                return GeneralDbResponses.ItemAlreadyExists(request.LocationModel.Name);
            var data = request.LocationModel.Adapt(new Location());
            dbContext.Locations.Add(data);
            await dbContext.SaveChangesAsync(cancellationToken);
            return GeneralDbResponses.ItemCreated(request.LocationModel.Name);

        }
        catch (Exception ex)

        {
            return new ServiceResponse(true, ex.Message);
        }

    }
}
