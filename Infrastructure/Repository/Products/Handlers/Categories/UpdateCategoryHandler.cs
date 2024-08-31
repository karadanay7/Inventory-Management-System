using System;
using Application.DTO.Response;
using Application.Service.Products.Commands.Categories;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Products.Handlers.Categories;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, ServiceResponse>
{
    private readonly DataAccess.IDbContextFactory<AppDbContext> _contextFactory;

    public UpdateCategoryHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<ServiceResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var dbContext = _contextFactory.CreateDbContext();

            var category = await dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == request.CategoryModel.Id, cancellationToken);

            if (category == null)
                return GeneralDbResponses.ItemNotFound(request.CategoryModel.Name);

            // Update the properties of the existing category
            category.Name = request.CategoryModel.Name;

            // No need to attach or detach, just update the entity's properties
            dbContext.Categories.Update(category);

            await dbContext.SaveChangesAsync(cancellationToken);

            return GeneralDbResponses.ItemUpdated(request.CategoryModel.Name);
        }
        catch (Exception ex)
        {
            return new ServiceResponse(false, ex.Message);
        }
    }
}

