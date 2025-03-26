using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Data.Repositories;
using IMS.Domain.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Data.UnitOfWorks
{
	public interface IUnitOfWorks : IDisposable
	{
		ApplicationDbContext Context { get; }

		IGenericRepository<Candidate> CandidateRepository { get; }
		IGenericRepository<Job> JobRepository { get; }
		IGenericRepository<Offer> OfferRepository { get; }
		IGenericRepository<Interview> InterviewRepository { get; }

		IGenericRepository<T> GenericRepository<T>() where T:class;

		int SaveChanges();

		Task<int> SaveChangesAsync();

		Task<IDbContextTransaction> BeginTransactionAsync();

		Task CommitTransactionAsync();

		Task RollbackTransactionAsync();
		GenericRepository<RefreshToken> RefreshTokenRepository { get; }

		GenericRepository<ResetPasswordToken> ResetPasswordTokenRepository { get; }
	}
}