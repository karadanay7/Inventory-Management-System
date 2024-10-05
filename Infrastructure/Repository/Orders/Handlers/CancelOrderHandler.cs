using System;
using Application.DTO.Response;
using Application.Extension;
using Application.Service.Orders.Commands;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using Infrastructure.Repository.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Orders.Handlers;

public class CancelOrderHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<CancelOrderCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var dbContext = contextFactory.CreateDbContext();
            var order = await dbContext.Orders.Where(x => x.Id == request.OrderId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (order == null) return new ServiceResponse(true, "Order not found");
          order.OrderState = OrderState.Cancelled;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ServiceResponse(true, "Order cancelled successfully");
           
        }
        catch (Exception ex)
        {
            return new ServiceResponse(false, ex.Message);
        }
    }
}

