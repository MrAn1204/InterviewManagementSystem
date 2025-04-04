using IMS.Business.Services;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Business.Handlers;

public class CandidateCreateCommandHandler : IRequestHandler<CandidateCreateCommand, bool>
{
    private readonly IUnitOfWorks _unitOfWork;
    private readonly IFileService _fileService;

    public CandidateCreateCommandHandler(IUnitOfWorks unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<bool> Handle(CandidateCreateCommand request, CancellationToken cancellationToken)
    {
        string filePath = "";
        if (request.CvAttachment != null)
        {
            filePath = await _fileService.UploadFileAsync(request.CvAttachment);
        }
        Candidate newCandidate = new Candidate
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber ?? "",
            Address = request.Address ?? "",
            Gender = request.Gender < 0 ? null : (request.Gender == 0 ? false : true),
            DateOfBirth = request.DOB,
            CurrentPosition = request.Position,
            Note = request.Note,
            Experience = request.Experience,
            Status = request.Status,
            CV = filePath,
            CreatedDate = DateTime.Now,
            RecruiterId = request.Recruiter,
            LevelId = request.HighestLevel,
            CandidateSkills = [.. request.Skills.Select(skillId => new CandidateSkill
            {
                SkillId = skillId,
            })]
        };
        _unitOfWork.CandidateRepository.Add(newCandidate);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            throw new DatabaseBadRequestException("Create candidate failed");
        }

        return result > 0;
    }
}
