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
                var emailAttachmentProcessor = scope.Resolve<IEmailAttachmentProcessor>(
                    new NamedParameter("emailUsername", ConfigurationManager.AppSettings["EmailUsername"]),
                    new NamedParameter("emailPassword", ConfigurationManager.AppSettings["EmailPassword"]),
                    new NamedParameter("searchFromEmail", ConfigurationManager.AppSettings["SearchFromEmail"]),
                    new NamedParameter("tempFileLocation", ConfigurationManager.AppSettings["TempFileLocation"]),
                    new NamedParameter("connectionString", ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString),
                    new NamedParameter("awsBucketName", ConfigurationManager.AppSettings["AWSBucketName"]),
                    new NamedParameter("awsAccessKey", ConfigurationManager.AppSettings["AWSAccessKey"]),
                    new NamedParameter("awsSecretKey", ConfigurationManager.AppSettings["AWSSecretKey"]));
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