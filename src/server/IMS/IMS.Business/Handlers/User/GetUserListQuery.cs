
using IMS.Business.ViewModels;
using IMS.Core.ViewModels;
using MediatR;

namespace IMS.Business.Handlers
{
    public class GetUserListQuery : IRequest<PaginatedResult<UserDetailViewModel>>
    {
        /// <summary>
        /// Tìm kiếm trên các trường: UserName, Email, FullName, Address
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Lọc theo tên Role (ví dụ: Admin, Recruiter,...)
        /// </summary>
        public string[]? Roles { get; set; }

        /// <summary>
        /// Lọc theo DepartmentId
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Lọc theo trạng thái hoạt động (true: Active, false: Inactive)
        /// </summary>
        public bool? IsActive { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = int.MaxValue;
    }
}