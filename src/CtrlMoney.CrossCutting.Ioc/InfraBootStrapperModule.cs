using CtrlMoney.Domain.Interfaces.Base;
using CtrlMoney.Infra.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CtrlMoney.CrossCutting.Ioc
{
    public class InfraBootStrapperModule
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //helper service
            services.AddScoped<IUnitOfWork, UnitOfWork>();
           // services.AddScoped<NotificationContext, NotificationContext>();

        }
    }
}
