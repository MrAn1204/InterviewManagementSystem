using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class OfferUpdateCommandHandler(IMapper mapper, IUnitOfWorks unitOfWork) : IRequestHandler<OfferUpdateCommand, OfferViewModel>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;

    public async Task<OfferViewModel> Handle(OfferUpdateCommand request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.OfferRepository.GetQuery();
        var result = await query.Include(o => o.Department).FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException("Offer not found");

        result.CandidateId = request.CandidateId;
        result.LevelId = request.LevelId;
        result.InterviewId = request.InterviewId;
        result.DepartmentId = request.DepartmentId;
        result.Position = request.Position;
        result.ContractType = request.ContractType;
        result.ContractStart = request.ContractStart;
        result.ContractEnd = request.ContractEnd;
        result.Status = request.Status;
        result.ApprovedBy = request.ApprovedBy;
        result.ApprovedDate = request.ApprovedDate;
        result.SalaryBasic = request.SalaryBasic;
        result.Note = request.Note;
        result.DueDate = request.DueDate;

        _unitOfWork.OfferRepository.Update(result);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OfferViewModel>(result);
    }
}
