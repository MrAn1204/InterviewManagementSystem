using System;
using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers;

public class GetUserByRoleQuery : IRequest<IEnumerable<UserByRoleViewModel>>
{
    public List<string> Roles { get; set; } = [];
}
