using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers;

public class GetUserByIdQuery : IRequest<UserDetailViewModel>
{
    public int UserId { get; set; }
}
