using IMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Data;

public static class SeedData
{
	public static void Seed(ApplicationDbContext context)
	{
		// 1) Thêm 4 Role nếu chưa có
		if (!context.Roles.Any())
		{
			var roles = new[]
			{
				new Role { RoleName = "Recruiter".ToUpper() },
				new Role { RoleName = "Interviewer".ToUpper() },
				new Role { RoleName = "Admin".ToUpper() },
				new Role { RoleName = "Manager".ToUpper() }
			};
			context.Roles.AddRange(roles);
			context.SaveChanges();
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

			var users = new[]
			{
				new User
				{
					UserName = "nguyenvana",
					Password = "123", // Bảo mật, nên Hash password
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
					Password = "123",
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
			context.Users.AddRange(users);
			context.SaveChanges();
		}

		// 4) Gán UserRole
		// Ví dụ: user "nguyenvana" => Admin, Manager; user "tranthib" => Recruiter, Interviewer
		var userA = context.Users.FirstOrDefault(u => u.UserName == "nguyenvana");
		var userB = context.Users.FirstOrDefault(u => u.UserName == "tranthib");

		var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
		var managerRole = context.Roles.FirstOrDefault(r => r.RoleName == "Manager");
		var recruiterRole = context.Roles.FirstOrDefault(r => r.RoleName == "Recruiter");
		var interviewerRole = context.Roles.FirstOrDefault(r => r.RoleName == "Interviewer");

		if (userA != null && !context.UserRoles.Any(ur => ur.UserId == userA.Id))
		{
			context.UserRoles.Add(new UserRole { UserId = userA.Id, RoleId = adminRole!.Id });
			context.UserRoles.Add(new UserRole { UserId = userA.Id, RoleId = managerRole!.Id });
		}

		if (userB != null && !context.UserRoles.Any(ur => ur.UserId == userB.Id))
		{
			context.UserRoles.Add(new UserRole { UserId = userB.Id, RoleId = recruiterRole!.Id });
			context.UserRoles.Add(new UserRole { UserId = userB.Id, RoleId = interviewerRole!.Id });
		}

		context.SaveChanges();
	}
}