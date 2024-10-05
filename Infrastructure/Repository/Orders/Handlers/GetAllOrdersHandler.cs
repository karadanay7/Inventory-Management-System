using System;
using System.Collections;
using Application.DTO.Response.Orders;
using Application.Extension.Identity;
using Application.Service.Orders.Queries;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Orders.Handlers;

public class GetAllOrdersHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory, UserManager<ApplicationUser> userManager) : IRequestHandler<GetAllOrdersQuery, IEnumerable<GetOrderResponseDTO>>
{
  public async Task<IEnumerable<GetOrderResponseDTO>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
{
    using var dbContext = contextFactory.CreateDbContext();
    var orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    var products = await dbContext.Products.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    var users = await userManager.Users.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

    return orders.Select(order => 
    {
        var product = products.FirstOrDefault(x => x.Id == order.ProductId);
        var user = users.FirstOrDefault(x => x.Id == order.ClientId);

        return new GetOrderResponseDTO
        {
            Id = order.Id,
            ProductName = product?.Name ?? "Unknown Product", // Provide default if product is null
            Price = order.Price,
            DeliveringDate = order.DeliveringDate,
            OrderedDate = order.DateOrdered,
            ProductId = order.ProductId,
            ProductImage = product?.Base64Image ?? string.Empty, // Provide default if product image is null
            Quantity = order.Quantity,
            SerialNumber = product?.SerialNumber ?? "N/A", // Provide default if serial number is null
            State = order.OrderState,
            ClientId = order.ClientId,
            ClientName = user?.Name ?? "Unknown Client" // Provide default if user is null
        };
    }).ToList();
}
}

