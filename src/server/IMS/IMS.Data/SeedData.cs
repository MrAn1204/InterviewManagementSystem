using IMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Data;

public static class SeedData
{
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