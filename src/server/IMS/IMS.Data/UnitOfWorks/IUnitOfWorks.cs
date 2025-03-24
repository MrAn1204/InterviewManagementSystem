using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Data.Repositories;
using IMS.Domain.Entities;

namespace IMS.Data.UnitOfWorks
{
	public interface IUnitOfWorks
	{
		GenericRepository<RefreshToken> RefreshTokenRepository { get; }

		GenericRepository<ResetPasswordToken> ResetPasswordTokenRepository { get; }
	}
}