﻿using MediatR;

namespace E_Commerce.Application.Features.Commands.Orders.CompleteOrder
{
	public class CompleteOrderCommandRequest : IRequest<CompleteOrderCommandResponse>
	{
        public string Id { get; set; }
    }
}