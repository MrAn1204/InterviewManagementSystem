using IMS.Business.Services;
using IMS.Data;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace IMS.API.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services,
		IConfiguration config)
		{
			services.AddControllers();
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
			);

			services.AddCors();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IUnitOfWorks, UnitOfWorks>();
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			services.AddSignalR();

			return services;
		}
	}
}