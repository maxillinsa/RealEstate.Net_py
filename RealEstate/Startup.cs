using Microsoft.Owin;
using Owin;
using RealEstate.App_Start;
using RealEstate.AutoMapper;

[assembly: OwinStartupAttribute(typeof(RealEstate.Startup))]
namespace RealEstate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AutoMapperConfig.Configure();
            AutofacConfig.RegisterComponents();
        }
    }
}
