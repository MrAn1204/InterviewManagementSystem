using System.Threading.Tasks;
using IMS.Domain;
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace IMS.Data;

public static class SeedData
{
	public static async Task SeedRoles(ApplicationDbContext context, RoleManager<Role> roleManager)
	{
		var seedRoles = new[]
		{
			new Role { Name = "Recruiter".ToUpper() },
			new Role { Name = "Interviewer".ToUpper() },
			new Role { Name = "Admin".ToUpper() },
			new Role { Name = "Manager".ToUpper() }
		};

		if (!context.Roles.Any())
		{
			foreach (var role in seedRoles)
			{
				await roleManager.CreateAsync(role);
				await context.SaveChangesAsync();
			}
		}
	}

	public static async Task SeedDepartments(ApplicationDbContext context)
	{
		var seedDepartments = new[]
		{
			new Department
			{
				DepartmentName = "Nhân Sự",
			},
			new Department
			{
				DepartmentName = "Kỹ Thuật",
			}
		};

		if (!context.Departments.Any())
		{
			context.Departments.AddRange(seedDepartments);
			await context.SaveChangesAsync();
		}
	}

	public static async Task SeedUsers(ApplicationDbContext context, UserManager<User> userManager)
	{
		var seedUsers = new[]
		{
			new User
			{
				UserName = "nguyenvana",
				Email = "vana@example.com",
				FullName = "Nguyễn Văn A",
				PhoneNumber = "0901123456",
				Address = "Hà Nội",
				Note = "Quản lý chung",
				DOB = new DateTime(1988, 5, 20),
				IsActive = true,
				DepartmentId = 1,
				CreatedDate = DateTime.UtcNow
			},
			new User
			{
				UserName = "tranthib",
				Email = "thib@example.com",
				FullName = "Trần Thị B",
				PhoneNumber = "0902233445",
				Address = "TP.HCM",
				DOB = new DateTime(1992, 10, 15),
				IsActive = true,
				DepartmentId = 2,
				CreatedDate = DateTime.UtcNow
			}
		};

		if (!context.Users.Any())
		{
			var passwordHasher = new PasswordHasher<User>();

			foreach (var user in seedUsers)
			{
				user.PasswordHash = passwordHasher.HashPassword(user, "Abc@123");
				await userManager.CreateAsync(user);
				await context.SaveChangesAsync();
			}
		}

		if (context.Roles.Any())
		{
			var userA = context.Users.FirstOrDefault(u => u.UserName == "nguyenvana");
			var userB = context.Users.FirstOrDefault(u => u.UserName == "tranthib");

			var adminRole = context.Roles.FirstOrDefault(r => r.Name == "ADMIN");
			var managerRole = context.Roles.FirstOrDefault(r => r.Name == "MANAGER");
			var recruiterRole = context.Roles.FirstOrDefault(r => r.Name == "RECRUITER");
			var interviewerRole = context.Roles.FirstOrDefault(r => r.Name == "INTERVIEWER");

			var userRoles = new[] {
				new { User = userA!, RoleName = adminRole!.Name },
				new { User = userA!, RoleName = managerRole!.Name },
				new { User = userB!, RoleName = recruiterRole!.Name },
				new { User = userB!, RoleName = interviewerRole!.Name },
			};

			foreach (var userRole in userRoles)
			{
				await userManager.AddToRoleAsync(userRole.User, userRole.RoleName!);
			}
		}
	}

	public static async Task SeedCandidates(ApplicationDbContext context)
	{
		var seedCandidates = new[]
		{
			new Candidate
			{
				FullName = "Alice Smith",
				Email = "alice.smith@example.com",
				PhoneNumber = "555-123-4567",
				Address = "456 Oak Avenue, Cityville"
			},
			new Candidate
			{
				FullName = "Brian Carter",
				Email = "brian.carter@example.com",
				PhoneNumber = "555-987-6543",
				Address = "789 Maple Street, Townsville"
			}
		};

		if (!context.Candidates.Any())
		{
			context.Candidates.AddRange(seedCandidates);
			await context.SaveChangesAsync();
		}
	}

	public static async Task SeedJobs(ApplicationDbContext context)
	{
		var seedJobs = new[]
		{
			new Job
			{
				Title = "Java Developer",
				WorkingAddress = "123 Main Street, Anytown",
				SalaryMin = 50000,
				SalaryMax = 80000,
			},
			new Job
			{
				Title = "Project Manager",
				WorkingAddress = "123 Main Street, Anytown",
				SalaryMin = 100000,
				SalaryMax = 200000,
			}
		};

		if (!context.Jobs.Any())
		{

			context.Jobs.AddRange(seedJobs);
			await context.SaveChangesAsync();
		}
	}

	public static async Task SeedInterviews(ApplicationDbContext context)
	{
		var candidates = context.Candidates.ToList();
		var jobs = context.Jobs.ToList();
		var users = context.Users.ToList();

		var seedInterviews = new[]
		{
			new Interview
			{
				Title = "Java Developer Interview",
				InterviewDate = DateOnly.FromDateTime(DateTime.Now),
				StartTime = TimeOnly.FromDateTime(DateTime.Now),
				EndTime = TimeOnly.FromDateTime(DateTime.Now).AddHours(1),
				CandidateId = candidates[0].Id,
				Candidate = candidates[0],
				JobId = jobs[0].Id,
				Job = jobs[0],
				RecruiterId = users[0].Id,
				Recruiter = users[0],
				Interviewers = [users[0]],
				UserCreated = users[0],
			},
			new Interview
			{
				Title = "Project Manager Interview",
				InterviewDate = DateOnly.FromDateTime(DateTime.Now),
				StartTime = TimeOnly.FromDateTime(DateTime.Now),
				EndTime = TimeOnly.FromDateTime(DateTime.Now).AddHours(1),
				CandidateId = candidates[1].Id,
				Candidate = candidates[1],
				JobId = jobs[1].Id,
				Job = jobs[1],
				RecruiterId = users[1].Id,
				Recruiter = users[1],
				Interviewers = [users[1]],
				UserCreated = users[0],
				Status = InterviewStatus.Invited
			}
		};

		if (!context.Interviews.Any())
		{
			context.Interviews.AddRange(seedInterviews);
			await context.SaveChangesAsync();
		}
	}

	public static void Seed(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
	{
		// Create transaction
		using var transaction = context.Database.BeginTransaction();

		// 1) Thêm 4 Role nếu chưa có
		if (!context.Roles.Any())
		{
			var roles = new[]
			{
				new Role { Name = "Recruiter".ToUpper() },
				new Role { Name = "Interviewer".ToUpper() },
				new Role { Name = "Admin".ToUpper() },
				new Role { Name = "Manager".ToUpper() }
			};

			foreach (var role in roles)
			{
				roleManager.CreateAsync(role).Wait();
			}
		}

		// 2) Thêm Department (VD: "Nhân Sự", "Kỹ Thuật") nếu cần
		if (!context.Departments.Any())
		{
			var departments = new[]
			{
				new Department
				{
					DepartmentName = "Nhân Sự",
				},
				new Department
				{
					DepartmentName = "Kỹ Thuật",
				}
			};
			context.Departments.AddRange(departments);
			context.SaveChanges();
		}

		// 3) Thêm User nếu chưa có
		if (!context.Users.Any())
		{
			// Giả sử "Nhân Sự" = Id=1, "Kỹ Thuật"= Id=2,
			// tuỳ ID do DB generate hay not
			var deptHr = context.Departments.FirstOrDefault(d => d.DepartmentName == "Nhân Sự");
			var deptIt = context.Departments.FirstOrDefault(d => d.DepartmentName == "Kỹ Thuật");

			var passwordHasher = new PasswordHasher<User>();

			var users = new[]
			{
				new User
				{
					UserName = "nguyenvana",
					Email = "vana@example.com",
					FullName = "Nguyễn Văn A",
					PhoneNumber = "0901123456",
					Address = "Hà Nội",
					Note = "Quản lý chung",
					DOB = new DateTime(1988, 5, 20),
					IsActive = true,
					DepartmentId = deptHr?.Id ?? 1,
					CreatedDate = DateTime.UtcNow
				},
				new User
				{
					UserName = "tranthib",
					Email = "thib@example.com",
					FullName = "Trần Thị B",
					PhoneNumber = "0902233445",
					Address = "TP.HCM",
					DOB = new DateTime(1992, 10, 15),
					IsActive = true,
					DepartmentId = deptIt?.Id ?? 2,
					CreatedDate = DateTime.UtcNow
				}
			};

			foreach (var user in users)
			{
				user.PasswordHash = passwordHasher.HashPassword(user, "Abc@123");
				userManager.CreateAsync(user).Wait();
			}
		}

		// 4) Gán UserRole
		// Ví dụ: user "nguyenvana" => Admin, Manager; user "tranthib" => Recruiter, Interviewer
		var userA = context.Users.FirstOrDefault(u => u.UserName == "nguyenvana");
		var userB = context.Users.FirstOrDefault(u => u.UserName == "tranthib");

		var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
		var managerRole = context.Roles.FirstOrDefault(r => r.Name == "Manager");
		var recruiterRole = context.Roles.FirstOrDefault(r => r.Name == "Recruiter");
		var interviewerRole = context.Roles.FirstOrDefault(r => r.Name == "Interviewer");

		var userRoles = new[] {
			new { User = userA!, RoleName = adminRole!.Name },
			new { User = userA!, RoleName = managerRole!.Name },
			new { User = userB!, RoleName = recruiterRole!.Name },
			new { User = userB!, RoleName = interviewerRole!.Name },
		};

		foreach (var userRole in userRoles)
		{
			userManager.AddToRoleAsync(userRole.User, userRole.RoleName!).Wait();
		}

		context.SaveChanges();

		transaction.Commit();
	}
}