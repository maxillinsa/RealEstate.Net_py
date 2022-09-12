using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;
using System;
using System.Web;


namespace Web.Common
{
    public class CaptchaImageResult:ActionResult
    {
        public string GetCaptchaString(int length)
        {
            int intZero = '0';
            int intNine = '9';
            int intA = 'A';
            int intZ = 'Z';
            int intCount = 0;
            int intRandomNumber = 0;
            string strCaptchaString="";

            Random random = new Random(System.DateTime.Now.Millisecond);

            while (intCount < length)
            {
                intRandomNumber = random.Next(intZero, intZ);
                if (((intRandomNumber >= intZero) && (intRandomNumber <= intNine) || (intRandomNumber >= intA) && (intRandomNumber <= intZ)))
                {
                    strCaptchaString = strCaptchaString + (char)intRandomNumber;
                    intCount = intCount + 1;
                }
            }
            return strCaptchaString;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            Bitmap bmp = new Bitmap(100, 30);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Tomato);
            string randomString = GetCaptchaString(5);
            context.HttpContext.Session["captchastring"] = randomString;
            g.DrawString(randomString, new Font("Georgia", 16), new SolidBrush(Color.Yellow), 2, 2);
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "image/jpeg";
            bmp.Save(response.OutputStream,ImageFormat.Jpeg);
            bmp.Dispose();
        }
    }
}