using System;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Business.Handlers;

public class OfferCreateCommandHandler(IUnitOfWorks unitOfWorks) : IRequestHandler<OfferCreateCommand, int>
{
    private readonly IUnitOfWorks _unitOfWork = unitOfWorks;

    public async Task<int> Handle(OfferCreateCommand request, CancellationToken cancellationToken)
    {
        var newOffer = new Offer
        {
            CandidateId = request.CandidateId,
            InterviewId = request.InterviewId,
            DepartmentId = request.DepartmentId,
            Position = request.Position,
            ContractType = request.ContractType,
            ContractStart = request.ContractStart,
            ContractEnd = request.ContractEnd,
            Status = request.Status,
            ApprovedBy = request.ApprovedBy,
            ApprovedDate = request.ApprovedDate,
            SalaryBasic = request.SalaryBasic,
            Note = request.Note,
            DueDate = request.DueDate
        };

        _unitOfWork.OfferRepository.Add(newOffer);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            throw new DatabaseBadRequestException("Create category failed");
        }

        return newOffer.Id;
    }
}
