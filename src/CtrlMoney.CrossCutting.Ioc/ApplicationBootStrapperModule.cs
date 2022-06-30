using CtrlMoney.AppService;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.WorkSheet.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CtrlMoney.CrossCutting.Ioc
{
    public class ApplicationBootStrapperModule
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBankAppService, BankAppService>();
            services.AddScoped<IRegisterAppService, RegisterAppService>();
            services.AddScoped<IParentTreeAppService, ParentTreeAppService>();
            services.AddScoped<IChildTreeAppService, ChildTreeAppService>();
            services.AddScoped<IGrandChildTreeAppService, GrandChildTreeAppService>();
            services.AddScoped<IXLWorkbookService, XLWorkbookService>();
        
        }
    }
}
