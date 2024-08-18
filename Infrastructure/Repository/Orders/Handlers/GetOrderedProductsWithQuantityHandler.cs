using System;
using Application.DTO.Response.Orders;
using Application.Service.Orders.Queries;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Orders.Handlers;

public class GetOrderedProductsWithQuantityHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetOrderedProductsWithQuantityQuery, IEnumerable<GetOrderedProductsWithQuantityResponseDTO>>
{
    public async Task<IEnumerable<GetOrderedProductsWithQuantityResponseDTO>> Handle(GetOrderedProductsWithQuantityQuery request, CancellationToken cancellationToken)
    {
        using var dbContext = contextFactory.CreateDbContext();
        var orders = new List<Order>();
        var data = new List<GetOrderedProductsWithQuantityResponseDTO>();
        if (!string.IsNullOrEmpty(request.UserId))
            orders = await dbContext.Orders.AsNoTracking().Where(x => x.ClientId.ToString() == request.UserId).ToListAsync(cancellationToken: cancellationToken);
        else
            orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

        var selectOrdersWithin12Months = orders.Where(order => order.DateOrdered.Date >= DateTime.Today.AddMonths(-12)).GroupBy(order => new { Name = order.ProductId }).ToList();

        foreach (var order in selectOrdersWithin12Months)
        {
            data.Add(new GetOrderedProductsWithQuantityResponseDTO
            {
                ProductName = (await dbContext.Products.FirstOrDefaultAsync(x => x.Id == order.Key.Name, cancellationToken: cancellationToken)).Name,
                QuantityOrdered = order.Sum(x => x.Quantity),
            });
        }
        return data;

    }

}
