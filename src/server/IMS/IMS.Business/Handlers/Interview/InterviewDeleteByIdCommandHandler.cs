using AutoMapper;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class InterviewDeleteByIdCommandHandler(IUnitOfWorks unitOfWork, IMapper mapper)
    : BaseHandler(unitOfWork, mapper), IRequestHandler<InterviewDeleteByIdCommand, bool>
{
    public async Task<bool> Handle(InterviewDeleteByIdCommand request, CancellationToken cancellationToken)
    {
        var interview = await _unitOfWork.InterviewRepository.GetByIdAsync(request.Id)
            ?? throw new ResourceNotFoundException("Interview not found");

        _unitOfWork.InterviewRepository.Delete(interview);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }
}
