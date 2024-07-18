using BusinessLayer.Services;
using BusinessLayer.Services.Implements;
using DataLayer.Model;
using DataLayer.Repository;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace SWDProject_BE.AppStarts
{
    public static class DependencyInjectionContainers
	{
		public static void InstallService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddRouting(options =>
			{
				options.LowercaseUrls = true; ;
				options.LowercaseQueryStrings = true;
			});
			services.AddDbContext<SWD392_DBContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("local"));
			});

			services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthServices, AuthServices>();
			services.AddScoped<IUsersService, UsersServices>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubcategoryService, SubcategoryService>();
            services.AddScoped<IExchangedService, ExchangedService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IAppealService, AppealService>();
            services.AddScoped<IBannedAccountService,BannedAccountServices>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSingleton<PayPalService>();

        }
    }
}
