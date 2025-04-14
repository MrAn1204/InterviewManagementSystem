using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.Business.DTOs
{
    public class CheckUniqueResultDto
    {
        public bool UsernameExists { get; set; }
        public bool EmailExists { get; set; }
    }
}