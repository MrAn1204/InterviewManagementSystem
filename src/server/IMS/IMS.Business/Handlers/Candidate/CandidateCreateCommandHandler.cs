using System;
using IMS.Business.Services;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Business.Handlers;

public class CandidateCreateCommandHandler : IRequestHandler<CandidateCreateCommand, int>
{
    private readonly IUnitOfWorks _unitOfWork;
    private readonly IFileService _fileService;

    public CandidateCreateCommandHandler(IUnitOfWorks unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<int> Handle(CandidateCreateCommand request, CancellationToken cancellationToken)
    {
        string filePath = await _fileService.UploadFileAsync(request.CvAttachment);
        Candidate newCandidate = new Candidate
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Gender = request.Gender < 0 ? null : (request.Gender == 0 ? false : true),
            DateOfBirth = request.DOB,
            CurrentPosition = request.Position,
            Note = request.Note,
            Experience = request.YearOfExperience,
            Status = request.Status,
            CV = filePath,
            CreatedDate = DateTime.Now,
            RecruiterId = request.Recruiter,
            LevelId=request.HighestLevel
        };
        _unitOfWork.CandidateRepository.Add(newCandidate);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            throw new DatabaseBadRequestException("Create category failed");
        }

        int newCandidateId = newCandidate.Id;
        var candidateSkills = request.Skills.Select(skillId => new CandidateSkill
        {
            SkillId = skillId,
            CandidateId = newCandidateId
        }).ToArray();

        // Thêm toàn bộ danh sách vào DB cùng lúc
        _unitOfWork.GenericRepository<CandidateSkill>().AddRange(candidateSkills);
        result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
        {
            throw new DatabaseBadRequestException("Create category failed");
        }

        return result;
    }
}
