using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Data.Repositories;
using IMS.Domain.Entities;

namespace IMS.Data.UnitOfWorks
{
    public class UnitOfWorks(ApplicationDbContext context) : IUnitOfWorks
    {
		private readonly ApplicationDbContext _context = context;
		
        public GenericRepository<RefreshToken> RefreshTokenRepository => new(_context);

        public GenericRepository<ResetPasswordToken> ResetPasswordTokenRepository => new(_context);
    }
}