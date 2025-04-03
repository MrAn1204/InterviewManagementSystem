using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.Business.ViewModels
{
    public class UserDetailViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string[] Roles { get; set; }
        public bool IsActive { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}