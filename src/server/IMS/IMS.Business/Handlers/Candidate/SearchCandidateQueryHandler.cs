using System;
using Amazon.Runtime.Internal;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Core.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace IMS.Business.Handlers;

public class SearchCandidateQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper) : IRequestHandler<SearchCandidateQuery, PaginatedResult<CandidateViewModel>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;

    public async Task<PaginatedResult<CandidateViewModel>> Handle(SearchCandidateQuery request, CancellationToken cancellationToken)
    {

        // Tao query
        var query = _unitOfWork.CandidateRepository.GetQuery();

        // Check keyword not null or empty, then filter
        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(x => x.FullName.Contains(request.Keyword) || x.Note!.Contains(request.Keyword));
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            query.Where(x => x.Status == request.Status);
        }

        // Dem so luong
        int total = await query.CountAsync(cancellationToken);

        // Sap xep
        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = query.OrderByExtension(request.OrderBy, request.OrderDirection.ToString());
        }
        else
        {
            query = query.OrderBy(x => x.FullName);
        }

        // Lay du lieu
        var items = await query.Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize).Include(c => c.Recruiter)
            .ToListAsync(cancellationToken);

        // Chuyen du lieu sang view model
        var viewModels = _mapper.Map<IEnumerable<CandidateViewModel>>(items);

        // Tra ve ket qua
        return new PaginatedResult<CandidateViewModel>(request.PageNumber, request.PageSize, total, viewModels);
    }
}
