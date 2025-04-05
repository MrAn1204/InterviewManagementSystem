using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace IMS.Business.Handlers;

public class InactiveUserCommand : IRequest<Unit>
{
    public int UserId { get; set; }
}
