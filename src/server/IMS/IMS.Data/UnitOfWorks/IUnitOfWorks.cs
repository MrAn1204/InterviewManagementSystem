
using IMS.Data.Repositories;
using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Data.UnitOfWorks
{
	public interface IUnitOfWorks : IDisposable
	{
		ApplicationDbContext Context { get; }

		IGenericRepository<Candidate> CandidateRepository { get; }
		IGenericRepository<Department> DepartmentRepository { get; }
		IGenericRepository<Job> JobRepository { get; }
		IGenericRepository<Offer> OfferRepository { get; }
		// IGenericRepository<OfferDepartment> OfferDepartmentRepository { get; }
		IGenericRepository<Interview> InterviewRepository { get; }
		IGenericRepository<User> UserRepository { get; }
		IGenericRepository<Reminder> ReminderRepository { get; }

		IGenericRepository<T> GenericRepository<T>() where T:class, IBaseEntity;

		int SaveChanges();

		Task<int> SaveChangesAsync();

		Task<IDbContextTransaction> BeginTransactionAsync();

		Task CommitTransactionAsync();

		Task RollbackTransactionAsync();
		IGenericRepository<RefreshToken> RefreshTokenRepository { get; }

		IGenericRepository<ResetPasswordToken> ResetPasswordTokenRepository { get; }
	}
}