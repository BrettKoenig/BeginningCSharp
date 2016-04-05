using System;
using System.Configuration;
using System.Reflection;
using BadExample.Service;
using Autofac;
using BadExample.Service.Interfaces;

namespace BadExample
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main(string[] args)
        {
            AutofacConfiguration();
            using (var scope = Container.BeginLifetimeScope())
            {
                var emailAttachmentProcessor = scope.Resolve<IEmailAttachmentProcessor>();
                emailAttachmentProcessor.ProcessEmailAttachments();
            }
        }

        private static void AutofacConfiguration()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.Load("BadExample.Service")).AsImplementedInterfaces();
            
            Container = builder.Build();
        }
    }
}