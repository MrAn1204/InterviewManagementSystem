using System;
using Amazon.Runtime.Internal;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class GetCandidateByIdQuery : IRequest<CandidateViewModel>
{
    public int Id { get; set; }
}
