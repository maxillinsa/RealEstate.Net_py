using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using CustomRoles;
using Newtonsoft.Json;
using RealEstate.Common;
using RealEstate.Models;
using RealEstate.Models.ViewModels;

namespace RealEstate
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (PerfectRealDataContext dbContext = new PerfectRealDataContext())
            {
                CompanyViewModel myCompany = dbContext.Companies.Select(x => new CompanyViewModel
                {
                    GoogleMapAPI = x.GoogleMapAPI,
                    CompanyName = x.CompanyName,
                    ExchageRateUSD = x.ExchageRateUSD ?? 1,
                    DefaulPageSize = x.DefaulPageSize,
                    NumberOfExpirationDates = x.NumberOfExpirationDates,
                    ImageUrlLogo = x.ImageUrlLogo,
                    CallCenter = x.CallCenter,
                }).FirstOrDefault();
                Security.SaveConfigToCache(myCompany);
            };

        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                newUser.AccountId = serializeModel.AccountId;
                newUser.FirstName = serializeModel.FirstName;
                newUser.LastName = serializeModel.LastName;
                newUser.Email = serializeModel.Email;
                newUser.roles = serializeModel.roles;
                HttpContext.Current.User = newUser;

                
                // create new 15 minutes for cookie
                string userData = JsonConvert.SerializeObject(serializeModel);
                FormsAuthenticationTicket authTicket2 = new FormsAuthenticationTicket(
                           1,
                          newUser.Email,
                           DateTime.Now,
                           DateTime.Now.AddMinutes(15),
                           false,
                           userData);
            }
        }
   
    }
}
