using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Sb.Interfaces;
using Sb.Interfaces.Services;
using Sb.Services;
using Sb.Services.Loaders;
using Sb.Services.Managers;
using Sb.Services.Web;

namespace Sb.Ioc
{
    public class Bootstrapper
    {
        public static void Initialise()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.Load("Sb.Web"));

            builder.RegisterType<AppDataFilePathResolver>().As<IFilePathResolver>();
            builder.RegisterType<FileQuestionLoader>().As<IQuestionLoader>();

            builder.RegisterType<FileQuestionLoader>()
                .As<IQuestionLoader>()
                .WithParameter("directoryName", "Questions")
                .WithParameter("cacheName", "Questions");

            builder.RegisterType<FileRulesetLoader>()
                .As<IRulesetLoader>()
                .WithParameter("directoryName", "Rules")
                .WithParameter("cacheName", "Rules");

            builder.RegisterType<FileObligationLoader>()
                .As<IObligationLoader>()
                .WithParameter("directoryName", "Obligations")
                .WithParameter("cacheName", "Obligations");

            builder.RegisterType<FilePersonaLoader>()
                .As<IPersonaLoader>()
                .WithParameter("directoryName", "Personas")
                .WithParameter("cacheName", "Personas");

            builder.RegisterType<QuestionManager>().As<IQuestionManager>();
            builder.RegisterType<ObligationManager>().As<IObligationManager>();
            builder.RegisterType<AnswerManager>().As<IAnswerManager>();
            builder.RegisterType<PersonaManager>().As<IPersonaManager>();
            builder.RegisterType<HttpCookieContext>().As<ICookieContext>();
            builder.RegisterType<HttpWrapper>().As<IHttpWrapper>();
            builder.RegisterType<CacheManager>().As<ICacheManager>();
            builder.RegisterType<AppSettingsReader>().As<ISettingsReader>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            ServiceLocator.SetContainer(container);
        }
    }
}
