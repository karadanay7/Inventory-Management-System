using System;
using Application.DTO.Response.Orders;
using Application.Extension;
using Application.Service.Orders.Queries;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Orders.Handlers;

public class GetGenericOrdersCountHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetGenericOrdersCountQuery, GetOrdersCountResponseDTO>
{
  public async Task<GetOrdersCountResponseDTO> Handle(GetGenericOrdersCountQuery request, CancellationToken cancellationToken)
  {
    using var dbContext = contextFactory.CreateDbContext();
    var list = new List<Order>();
    if(!request.IsAdmin)
    list = await dbContext.Orders.AsNoTracking().Where(x => x.ClientId.ToString()==request.UserId).ToListAsync(cancellationToken: cancellationToken);
    else
    list = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    int ProcessingCount = list.Count(x => x.OrderState == OrderState.Processing);
  
    int DeliveringCount = list.Count(x => x.OrderState == OrderState.Delivering);
      int DeliveredCount = list.Count(x => x.OrderState == OrderState.Delivered);
    int CancelledCount = list.Count(x => x.OrderState == OrderState.Cancelled);
    return new GetOrdersCountResponseDTO(ProcessingCount,  DeliveringCount,DeliveredCount, CancelledCount);
    
  }
}

