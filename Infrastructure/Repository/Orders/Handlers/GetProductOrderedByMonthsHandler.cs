using System;
using System.Globalization;
using Application.DTO.Response.Orders;
using Application.Service.Orders.Queries;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Orders.Handlers;

public class GetProductOrderedByMonthsHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetProductOrderByMonthQuery, IEnumerable<GetProductOrderByMonthsResponseDTO>>
{
  public async Task<IEnumerable<GetProductOrderByMonthsResponseDTO>> Handle(GetProductOrderByMonthQuery request, CancellationToken cancellationToken)
  {
   using var dbContext = contextFactory.CreateDbContext();
   var orders =new List<Order>();
    var data = new List<GetProductOrderByMonthsResponseDTO>();
    if (!string.IsNullOrEmpty(request.UserId))
        orders = await dbContext.Orders.AsNoTracking().Where(x => x.ClientId.ToString() == request.UserId).ToListAsync(cancellationToken: cancellationToken);
    else
        orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
        var selectOrdersWithin12Months = orders.Where(order => order.DateOrdered.Date >= DateTime.Today.AddMonths(-12)).GroupBy(order => new { Month = order.DateOrdered.Month }).ToList();
        foreach (var order in selectOrdersWithin12Months)
        {
            data.Add(new GetProductOrderByMonthsResponseDTO
            {
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(order.Key.Month),
                TotalAmount = order.Sum(x => x.Price),
            });
        }
        return data;
  }
}

