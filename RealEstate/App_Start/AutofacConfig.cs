using Autofac;
using Autofac.Integration.Mvc;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.App_Start
{
    public static class AutofacConfig
    {
        public static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<PerfectRealDataContext>().AsSelf().As<DbContext>().InstancePerRequest();


            builder.RegisterType<DepartmentsRepository>().As<IDepartmentsRepository>().InstancePerRequest();

            builder.RegisterType<AccountRepository>().As<IAccountRepository>().InstancePerRequest();
            builder.RegisterType<AccountRolesRepository>().As<IAccountRolesRepository>().InstancePerRequest();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>().InstancePerRequest();
            builder.RegisterType<Estate_TypesRepository>().As<IEstate_TypesRepository>().InstancePerRequest();
            builder.RegisterType<EstateRepository>().As<IEstateRepository>().InstancePerRequest();
            builder.RegisterType<LoginLogRepository>().As<ILoginLogRepository>().InstancePerRequest();
            builder.RegisterType<RolesRepository>().As<IRolesRepository>().InstancePerRequest();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerRequest();
            builder.RegisterType<AlbumRepository>().As<IAlbumRepository>().InstancePerRequest();
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>().InstancePerRequest();

            builder.RegisterType<CategoryTypeRepository>().As<ICategoryTypeRepository>().InstancePerRequest();
            builder.RegisterType<Certificate_TypeRepository>().As<ICertificate_TypeRepository>().InstancePerRequest();

            builder.RegisterType<GroupCategoryRepository>().As<IGroupCategoryRepository>().InstancePerRequest();
            builder.RegisterType<CompanyRepository>().As<ICompanyRepository>().InstancePerRequest();

            builder.RegisterType<CountryRepository>().As<ICountryRepository>().InstancePerRequest();
            builder.RegisterType<VendorRepository>().As<IVendorRepository>().InstancePerRequest();

            builder.RegisterType<WardRepository>().As<IWardRepository>().InstancePerRequest();
            builder.RegisterType<NotificationRepository>().As<INotificationRepository>().InstancePerRequest();


            builder.RegisterType<Estate_InvestorRepository>().As<IEstate_InvestorRepository>().InstancePerRequest();
            builder.RegisterType<Estate_ProjectRepository>().As<IEstate_ProjectRepository>().InstancePerRequest();
            builder.RegisterType<Estate_GroupRepository>().As<IEstate_GroupRepository>().InstancePerRequest();

            builder.RegisterType<Estate_StatusRepository>().As<IEstate_StatusRepository>().InstancePerRequest();
            builder.RegisterType<TownRepository>().As<ITownRepository>().InstancePerRequest();

            builder.RegisterType<ProvinceRepository>().As<IProvinceRepository>().InstancePerRequest();
            builder.RegisterType<LoginLogRepository>().As<ILoginLogRepository>().InstancePerRequest();
            builder.RegisterType<SaleUnitRepository>().As<ISaleUnitRepository>().InstancePerRequest();
            builder.RegisterType<RentUnitRepository>().As<IRentUnitRepository>().InstancePerRequest();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerRequest();


            builder.RegisterType<ImageRepository>().As<IImageRepository>().InstancePerRequest();
            builder.RegisterType<DistrictRepository>().As<IDistrictRepository>().InstancePerRequest();
            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerRequest();
            builder.RegisterType<Estate_ImageRepository>().As<IEstate_ImageRepository>().InstancePerRequest();
            builder.RegisterType<StreetRepository>().As<IStreetRepository>().InstancePerRequest();
            builder.RegisterType<CustomerCategoriesRepository>().As<ICustomerCategoriesRepository>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}