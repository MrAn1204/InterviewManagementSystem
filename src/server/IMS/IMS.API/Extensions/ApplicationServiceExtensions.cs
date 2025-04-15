using IMS.Business.Services;
using IMS.Data;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace IMS.API.Extensions
{
	/// <summary>
	/// Extension class used to refactor code for program
	/// </summary>
	public static class ApplicationServiceExtensions
	{
		/// <summary>
		/// Extension used to refactor code for program
		/// </summary>
		/// <param name="services"></param>
		/// <param name="config"></param>
		/// <returns>return service </returns>
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
			services.AddScoped<IJobStatusUpdateService, JobStatusUpdateService>();

			return services;
		}
	}
}