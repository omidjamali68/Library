using Autofac;
using Library.Infrastructure.Application;
using Library.Persistence.EF;
using Library.Persistence.EF.BookCategories;
using Library.Persistence.EF.Books;
using Library.Persistence.EF.Entrusts;
using Library.Persistence.EF.Members;
using Library.RestApi;
using Library.Services.BookCategories;
using Library.Services.Books;
using Library.Services.Entrusts;
using Library.Services.Members;
using Microsoft.Extensions.Configuration;

namespace Library.RestApi.Configs
{
    class ServicesConfig : Configuration
    {
        private string _dbConnectionString;

        public override void Initialized()
        {
            _dbConnectionString = AppSettings.GetValue<string>("dbConnectionString");
        }

        public override void ConfigureServiceContainer(ContainerBuilder container)
        {
            container.RegisterAssemblyTypes(typeof(BookAppService).Assembly)
                     .AssignableTo<Service>()
                     .AsImplementedInterfaces()
                     .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(EntrustAppService).Assembly)
                .AssignableTo<Service>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(MemberAppService).Assembly)
                .AssignableTo<Service>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(BookCategoryAppService).Assembly)
                .AssignableTo<Service>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            


            container.RegisterAssemblyTypes(typeof(EFBookRepository).Assembly)
                    .AssignableTo<Repository>()
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(EFEntrustRepository).Assembly)
                .AssignableTo<Repository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(EFBookCategoryRepository).Assembly)
                .AssignableTo<Repository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            container.RegisterAssemblyTypes(typeof(EFMemberRepository).Assembly)
                .AssignableTo<Repository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            container.RegisterType<EFUnitOfWork>()
                .As<UnitOfWork>()
                .InstancePerLifetimeScope();
            container.RegisterType<EFDataContext>()
                .WithParameter("connectionString", _dbConnectionString)
                .AsSelf()
                .InstancePerLifetimeScope();
            //container.RegisterType<UserTokenAppService>()
            //    .As<UserTokenService>()
            //    .InstancePerLifetimeScope();

        }
    }
}
