using System;
using Application.DTO.Response.Orders;
using MediatR;

namespace Application.Service.Orders.Queries;

public record GetProductOrderByMonthQuery(string UserId=null):IRequest<IEnumerable<GetProductOrderByMonthsResponseDTO>>;

