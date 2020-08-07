using CtrlMoney.AppService;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.Domain.Interfaces.Base;
using CtrlMoney.Infra.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CtrlMoney.CrossCutting.Ioc
{
    public class ApplicationBootStrapperModule
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBankAppService, BankAppService>();
        }
    }
}
