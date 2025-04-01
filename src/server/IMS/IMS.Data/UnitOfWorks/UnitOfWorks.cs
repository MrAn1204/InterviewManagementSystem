using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Data.Repositories;
using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Data.UnitOfWorks
{
    public class UnitOfWorks(ApplicationDbContext context) : IUnitOfWorks
    {
        private readonly ApplicationDbContext _context = context;
        private bool _disposed = false;

        public IGenericRepository<RefreshToken> RefreshTokenRepository => new GenericRepository<RefreshToken>(_context);

        public IGenericRepository<ResetPasswordToken> ResetPasswordTokenRepository => new GenericRepository<ResetPasswordToken>(_context);

        public ApplicationDbContext Context => _context;

        private IGenericRepository<Candidate>? _candidateRepository;
        public IGenericRepository<Candidate> CandidateRepository => _candidateRepository ??= new GenericRepository<Candidate>(_context);

        private IGenericRepository<Job>? _jobRepository;
        public IGenericRepository<Job> JobRepository => _jobRepository ??= new GenericRepository<Job>(_context);

        private IGenericRepository<Offer>? _offerRepository;
        public IGenericRepository<Offer> OfferRepository => _offerRepository ??= new GenericRepository<Offer>(_context);

        private IGenericRepository<Interview>? _interviewRepository;
        public IGenericRepository<Interview> InterviewRepository => _interviewRepository ??= new GenericRepository<Interview>(_context);

        private IGenericRepository<Department>? _departmentRepository;
        public IGenericRepository<Department> DepartmentRepository => _departmentRepository ??= new GenericRepository<Department>(_context);

        private IGenericRepository<OfferDepartment>? _offerDepartmentRepository;
        public IGenericRepository<OfferDepartment> OfferDepartmentRepository => _offerDepartmentRepository ??= new GenericRepository<OfferDepartment>(_context);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        ~UnitOfWorks()
        {
            Dispose(false);
        }
    }
}