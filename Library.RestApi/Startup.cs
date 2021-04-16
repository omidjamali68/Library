using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Library.Infrastructure.Application;
using Library.Persistence.EF;
using Library.Persistence.EF.BookCategories;
using Library.Persistence.EF.Books;
using Library.Persistence.EF.Entrusts;
using Library.Persistence.EF.Members;
using Library.Services.BookCategories;
using Library.Services.BookCategories.Contracts;
using Library.Services.Books;
using Library.Services.Books.Contracts;
using Library.Services.Entrusts;
using Library.Services.Entrusts.Contracts;
using Library.Services.Members;
using Library.Services.Members.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.RestApi
{
    public class Startup
    {
        public IConfiguration Configuration { get;}
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",new OpenApiInfo
                {
                    Title = "Api Swagger",
                    Version = "v1"
                });
            });
            services.AddControllers();
            services.AddDbContext<EFDataContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("LibraryDb"));
            });

            #region IoC

            services.AddScoped<UnitOfWork, EFUnitOfWork>();

            services.AddScoped<BookCategoryService, BookCategoryAppService>();
            services.AddScoped<BookCategoryRepository, EFBookCategoryRepository>();

            services.AddScoped<BookRepository, EFBookRepository>();
            services.AddScoped<BookService, BookAppService>();

            services.AddScoped<MemberRepository, EFMemberRepository>();
            services.AddScoped<MemberService, MemberAppService>();

            services.AddScoped<EntrustRepository, EFEntrustRepository>();
            services.AddScoped<EntrustService, EntrustAppService>();

            #endregion


            //var container = new ContainerBuilder();
            //container.Populate(services);
            //ConfigureContainer(container);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Swagger");
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        //public void ConfigureContainer(ContainerBuilder containerBuilder)
        //{
        //    containerBuilder.AutofacConfig(typeof(Startup).Assembly);
        //}
    }
}
