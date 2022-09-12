using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CustomRoles;
using Newtonsoft.Json;
using RealEstate.Models;

namespace RealEstate.Common
{
    public class Functions : Controller
    {
        public static string generateAlias(string content)
        {
             Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = content.Normalize(NormalizationForm.FormD).Trim();

            string kq= regex.Replace(temp, String.Empty)
                        .Replace('\u0111', 'd')
                        .Replace('\u0110', 'D')
                        .Replace(",", "-")
                        .Replace(".", "-")
                        .Replace("!", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace(";", "-")
                        .Replace("/", "-")
                        .Replace("%", "ptram")
                        .Replace("&", "va")
                        .Replace("?", "")
                        .Replace('"', '-')
                        .Replace(' ', '-');
            while (kq.Contains("--"))
                kq = kq.Replace("--", "-");
            while (kq.Contains("  "))
                kq = kq.Replace("  ", " ");
            return kq;
        }
        public static bool SendEmail(string userName, string Password, string host, int port, string subject, string body, string email)
        {
            try
            {
                using (var smtpclient = new SmtpClient())
                {
                    smtpclient.EnableSsl = true;
                    smtpclient.Host = host;
                    smtpclient.Port = port;
                    smtpclient.UseDefaultCredentials = true;
                    smtpclient.Credentials = new NetworkCredential(userName, Password);
                    var msg = new MailMessage
                    {
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(userName),
                        Subject = subject,
                        Body = body,
                        Priority = MailPriority.Normal,
                    };

                    msg.To.Add(email);

                    smtpclient.Send(msg);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool MakeSessionFromCookie(HttpRequestBase Request, HttpResponseBase Response, HttpContextBase HttpContext)
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
                // create new 15 minutes for cookie
                string userData = JsonConvert.SerializeObject(serializeModel);
                FormsAuthenticationTicket authTicket2 = new FormsAuthenticationTicket(
                           1,
                          newUser.Email,
                           DateTime.Now,
                           DateTime.Now.AddMinutes(15),
                           false,
                           userData);
                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);
                HttpContext.Session["bds_Acc_id"] = newUser.AccountId;
                return true;
            }
            else
            {
                return false;
            }
        }
        //public long getIDSession(HttpContextBase HttpContext)
        //{
        //    var id_session = Convert.ToInt32(HttpContext.Session["bds_Acc_id"]);
        //    if (id_session == null || id_session == 0)
        //    {
        //        return -1;
        //    }
        //    return id_session;
        //}

        public static RangePriceDto rangePriceDtoConvert(int value)
        {

            var from = 0;
            var to = 1000000000000;
            switch (value)
            {
                case 1:
                    from = 1; to = 500;
                    break;
                case 2:
                    from = 501; to = 800;
                    break;
                case 3:
                    from = 801; to = 1000;
                    break;
                case 4:
                    from = 1001; to = 2000;
                    break;
                case 5:
                    from = 2001; to = 3000;
                    break;
                case 6:
                    from = 3001; to = 5000;
                    break;
                case 7:
                    from = 5001; to = 7000;
                    break;
                case 8:
                    from = 7001; to = 10000;
                    break;
                case 9:
                    from = 10001; to = 20000;
                    break;
                case 10:
                    from = 20001; to = 30000;
                    break;
                case 11:
                    from = 30001; to = 100000000;
                    break;
              



                case 20:
                    from = 1; to = 1000;
                    break;
                case 21:
                    from = 1001; to = 3000;
                    break;
                case 22:
                    from = 3001; to = 5000;
                    break;
                case 23:
                    from = 5001; to = 10000;
                    break;
                case 24:
                    from = 100001; to = 40000;
                    break;
                case 25:
                    from = 40001; to = 70000;
                    break;
                case 26:
                    from = 70001; to = 1000000;
                    break;
                case 27:
                    from = 100001; to = 1000000000;
                    break;
               
                default:
                    break;
            }

            var model = new RangePriceDto
            {
                From = from,
                To = to
            };
            return model;

        }
        public static RangeAreaDto rangeAreaDtoConvert(int value)
        {

            var from = 0;
            var to = 1000000000000;
            switch (value)
            {
                case 1:
                    from = 1; to = 30;
                    break;
                case 2:
                    from = 30; to = 80;
                    break;
                case 3:
                    from = 80; to = 100;
                    break;
                case 4:
                    from =100; to = 150;
                    break;
                case 5:
                    from = 150; to = 200;
                    break;
                case 6:
                    from = 200; to = 300;
                    break;
                case 7:
                    from = 300; to = 500;
                    break;
                case 8:
                    from = 500; to = 800;
                    break;
                case 9:                   
                    from = 800; to = 10000000;
                    break;
                default:
                    break;
            }

            var model = new RangeAreaDto
            {
                From = from,
                To = to
            };
            return model;

        }
    }

}