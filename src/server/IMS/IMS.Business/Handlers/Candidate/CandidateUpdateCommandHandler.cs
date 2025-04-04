using System;
using Amazon.S3;
using IMS.Business.Services;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class CandidateUpdateCommandHandler : IRequestHandler<CandidateUpdateCommand, bool>
{
    private readonly IUnitOfWorks _unitOfWork;
    private readonly IFileService _fileService;

    public CandidateUpdateCommandHandler(IUnitOfWorks unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<bool> Handle(CandidateUpdateCommand request, CancellationToken cancellationToken)
    {
        string filePath = "";
        if (request.CvAttachment != null)
        {
            bool isDeleted = await _fileService.DeleteFileAsync(request.OldFilePath);
            if (isDeleted == false)
            {
                throw new AmazonS3Exception("Remove error");
            }
            filePath = await _fileService.UploadFileAsync(request.CvAttachment);
        }

        Candidate candidate = await _unitOfWork.CandidateRepository.GetQuery().Include(c => c.Recruiter).Include(c => c.HighestLevel).Include(c => c.CandidateSkills).ThenInclude(cs => cs.Skill)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken) ??
            throw new ResourceNotFoundException("Candidate not found");
        if (candidate == null)
        {
            return false;
        }

        candidate.FullName = request.FullName;
        candidate.Email = request.Email;
        candidate.PhoneNumber = request.PhoneNumber ?? "";
        candidate.Address = request.Address ?? "";
        candidate.Gender = request.Gender < 0 ? null : (request.Gender == 0 ? false : true);
        candidate.DateOfBirth = request.DOB;
        candidate.CurrentPosition = request.Position;
        candidate.Note = request.Note;
        candidate.Experience = request.Experience;
        candidate.Status = request.Status;
        candidate.UpdatedDate = DateTime.Now;
        candidate.RecruiterId = request.Recruiter;
        candidate.LevelId = request.HighestLevel;
        if (!string.IsNullOrEmpty(filePath))
        {
            candidate.CV = filePath;
        }
        if (candidate.CandidateSkills != null)
        {
            candidate.CandidateSkills.Clear();
        }
        else
        {
            candidate.CandidateSkills = new List<CandidateSkill>();
        }

        var candidateSkills = request.Skills.Select(skillId => new CandidateSkill
        {
            SkillId = skillId,
            CandidateId = candidate.Id
        }).ToArray();

        foreach (CandidateSkill cs in candidateSkills)
        {
            candidate.CandidateSkills?.Add(cs);
        }

        _unitOfWork.CandidateRepository.Update(candidate);
        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
    }
}
