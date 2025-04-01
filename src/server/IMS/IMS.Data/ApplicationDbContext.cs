using IMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace IMS.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

	// DbSet cho các entity
	public DbSet<Department> Departments { get; set; } = null!;
	public DbSet<Candidate> Candidates { get; set; } = null!;
	public DbSet<Skill> Skills { get; set; } = null!;
	public DbSet<CandidateSkill> CandidateSkills { get; set; } = null!;
	public DbSet<Job> Jobs { get; set; } = null!;
	public DbSet<CandidateJob> CandidateJobs { get; set; } = null!;
	public DbSet<Interview> Interviews { get; set; } = null!;
	public DbSet<Offer> Offers { get; set; } = null!;
	public DbSet<Benefit> Benefits { get; set; } = null!;
	public DbSet<JobBenefit> JobBenefits { get; set; } = null!;
	public DbSet<Level> Levels { get; set; } = null!;
	public DbSet<JobSkill> JobSkills { get; set; } = null!;
	public DbSet<JobLevel> JobLevels { get; set; } = null!;
	public DbSet<OfferDepartment> OfferDepartments { get; set; } = null!;
	public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
	public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Composite key cho bảng trung gian
		modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
		modelBuilder.Entity<CandidateSkill>().HasKey(cs => new { cs.CandidateId, cs.SkillId });
		modelBuilder.Entity<CandidateJob>().HasKey(cj => new { cj.CandidateId, cj.JobId });
		modelBuilder.Entity<JobBenefit>().HasKey(jb => new { jb.JobId, jb.BenefitId });
		modelBuilder.Entity<JobSkill>().HasKey(js => new { js.JobId, js.SkillId });
		modelBuilder.Entity<JobLevel>().HasKey(jl => new { jl.JobId, jl.LevelId });
		modelBuilder.Entity<OfferDepartment>().HasKey(od => new { od.OfferId, od.DepartmentId });

		modelBuilder.Entity<IdentityUserLogin<int>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
		modelBuilder.Entity<IdentityUserRole<int>>().HasKey(r => new { r.UserId, r.RoleId });
		modelBuilder.Entity<IdentityUserToken<int>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });


		modelBuilder.Entity<User>().ToTable("Users");
		modelBuilder.Entity<Role>().ToTable("Roles");
		modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
		modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
		modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
		modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
		modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

		// 1:N => Department -> User
		modelBuilder.Entity<User>()
			.HasOne(u => u.Department)
			.WithMany(d => d.Users)
			.HasForeignKey(u => u.DepartmentId)
			.OnDelete(DeleteBehavior.Restrict);

		// 1:N => Job.CreatedBy -> User
		modelBuilder.Entity<Job>()
			.HasOne(j => j.UserCreated)
			.WithMany(u => u.JobsCreated)
			.HasForeignKey(j => j.CreatedBy)
			.OnDelete(DeleteBehavior.Restrict);

		// 1:N => Offer.ApprovedBy -> User
		modelBuilder.Entity<Offer>()
			.HasOne(o => o.UserApproved)
			.WithMany(u => u.OffersApproved)
			.HasForeignKey(o => o.ApprovedBy)
			.OnDelete(DeleteBehavior.Restrict);

		// 1:N => Interview.CreatedBy -> User
		modelBuilder.Entity<Interview>()
			.HasOne(i => i.UserCreated)
			.WithMany(u => u.InterviewsCreated)
			.HasForeignKey(i => i.CreatedBy)
			.OnDelete(DeleteBehavior.Restrict);

		// 1:N => Offer -> Candidate
		modelBuilder.Entity<Offer>()
			.HasOne(o => o.Candidate)
			.WithMany(c => c.Offers)
			.HasForeignKey(o => o.CandidateId)
			.OnDelete(DeleteBehavior.Restrict);

		// 1:N => Offer -> Job
		modelBuilder.Entity<Offer>()
			.HasOne(o => o.Job)
			.WithMany(j => j.Offers)
			.HasForeignKey(o => o.JobId)
			.OnDelete(DeleteBehavior.Restrict);

		// 1:N => Interview -> Job
		modelBuilder.Entity<Interview>()
			.HasOne(i => i.Job)
			.WithMany(j => j.Interviews!)
			.HasForeignKey(i => i.JobId)
			.OnDelete(DeleteBehavior.Restrict);

		// 1:N => Interview -> Candidate (nếu 1:1)
		modelBuilder.Entity<Interview>()
			.HasOne(i => i.Candidate)
			.WithMany()
			.HasForeignKey(i => i.CandidateId)
			.OnDelete(DeleteBehavior.Restrict);

		// 1:N => Offer -> Interview
		modelBuilder.Entity<Offer>()
			.HasOne(o => o.Interview)
			.WithMany(i => i.Offers!)
			.HasForeignKey(o => o.InterviewId)
			.OnDelete(DeleteBehavior.Restrict);

		// ...

		modelBuilder.Entity<Job>()
			.Property(j => j.SalaryMin)
			.HasColumnType("decimal(18,2)");

		modelBuilder.Entity<Job>()
			.Property(j => j.SalaryMax)
			.HasColumnType("decimal(18,2)");

		modelBuilder.Entity<Offer>()
			.Property(o => o.SalaryBasic)
			.HasColumnType("decimal(18,2)");

		modelBuilder.Entity<CandidateSkill>()
			.HasOne(cs => cs.Candidate)
			.WithMany(c => c.CandidateSkills)
			.HasForeignKey(cs => cs.CandidateId);

		modelBuilder.Entity<CandidateSkill>()
			.HasOne(cs => cs.Skill)
			.WithMany(s => s.CandidateSkills)
			.HasForeignKey(cs => cs.SkillId);

	}
}