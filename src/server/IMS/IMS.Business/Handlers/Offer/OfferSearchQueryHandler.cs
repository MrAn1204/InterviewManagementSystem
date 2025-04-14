using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Core.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class OfferSearchQueryHandler(IMapper mapper, IUnitOfWorks unitOfWork) : IRequestHandler<OfferSearchQuery, PaginatedResult<OfferViewModel>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;

    public async Task<PaginatedResult<OfferViewModel>> Handle(OfferSearchQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.OfferRepository.GetQuery()
            .Include(o => o.Department)
            .Include(o => o.Candidate)
            .Include(o => o.UserApproved).AsQueryable()
            ;

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            string keyword = request.Keyword.ToLower();
            
            query = query.Where(o =>
                (o.Candidate != null && ( o.Candidate.FullName.ToLower().Contains(keyword) || o.Candidate.Email.ToLower().Contains(keyword) )) ||
                (o.UserApproved != null && o.UserApproved.FullName.ToLower().Contains(keyword))
);
        }

        if (!string.IsNullOrEmpty(request.departmentName))
        {
            query = query.Where(o => o.Department != null && o.Department.DepartmentName.Contains(request.departmentName));
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(o => o.Candidate != null && o.Candidate.Status.Contains(request.Status));
        }

        int total = await query.CountAsync(cancellationToken);

        //Sap xep
        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = query.OrderByExtension(request.OrderBy, request.OrderDirection.ToString());
        }
        else
        {
            query = query.OrderBy(o => o.Candidate != null ? o.Candidate.FullName : "");
        }

        //Lay du lieu
        var items = await query.Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var viewModels = _mapper.Map<IEnumerable<OfferViewModel>>(items);

        return new PaginatedResult<OfferViewModel>(request.PageNumber, request.PageSize, total, viewModels);
    }
}
