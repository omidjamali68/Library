using Autofac;
using System.Reflection;


namespace Library.InfraStructure
{
    public static class AutoFac
    {
        public static void AutofacConfig(this ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<IScopeDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<ITransientDependency>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<ISingletoneDependency>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
        public interface IScopeDependency { }
        public interface ISingletoneDependency { }
        public interface ITransientDependency { }
    }
}
