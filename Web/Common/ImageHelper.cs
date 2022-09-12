using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace Web.Common
{
    public class ImageHelper
    {
        #region Not Use
        private static string MimeTypeFromFileExtension(string strFileName)
        {
            string mimeType = "";
            string strExtension = "";
            if (strFileName != null)
            {
                strExtension = strFileName.Substring(strFileName.LastIndexOf(@"."));
                switch (strExtension.ToLower(CultureInfo.CurrentCulture))
                {
                    case ".gif":
                        mimeType = "image/gif";
                        break;
                    case ".jpeg":
                    case ".jpe":
                    case ".jpg":
                        mimeType = "image/jpeg";
                        break;
                    case ".png":
                        mimeType = "image/png";
                        break;
                    default:
                        mimeType = "application/octet-stream";
                        break;
                }
            }
            else
            {
                mimeType = "application/octet-stream";
            }
            return mimeType;
        }

        /// <summary>
        /// Convert HttpPostedFileBase ---> MemoryStream
        /// </summary>
        /// <param name="f"></param>
        /// <returns>MemoryStream</returns>
        private static MemoryStream ParseStream(HttpPostedFileBase f)
        {
            Stream stream = default(Stream);
            stream = f.InputStream;
            byte[] uFile = new byte[stream.Length + 1];
            stream.Read(uFile, 0, Convert.ToInt32(stream.Length));
            return new MemoryStream(uFile);
        }

        private FileStreamResult Naive(HttpPostedFileBase urlImage, int width, int height)
        {
            MemoryStream original = ParseStream(urlImage);
            MemoryStream result = new MemoryStream();

            Image image = Image.FromStream(original);

            // Don't do this to yourself!
            Image thumbnail = image.GetThumbnailImage(width, height, null, IntPtr.Zero);
            thumbnail.Save(result, image.RawFormat);

            thumbnail.Dispose();
            image.Dispose();

            result.Position = 0;

            return new FileStreamResult(result, "image/jpeg");
        }

        private FileStreamResult Medium(HttpPostedFileBase urlImage, int width, int height)
        {
            MemoryStream original = ParseStream(urlImage);
            MemoryStream result = new MemoryStream();

            Image image = Image.FromStream(original);
            Bitmap bitmap = new Bitmap(width, height);

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle imageRectangle = new Rectangle(0, 0, width, height);
            graphics.DrawImage(image, imageRectangle);

            bitmap.Save(result, image.RawFormat);

            graphics.Dispose();
            bitmap.Dispose();
            image.Dispose();

            result.Position = 0;

            return new FileStreamResult(result, "image/jpeg");
        }

        public FileStreamResult High(HttpPostedFileBase urlImage, int width, int height)
        {
            MemoryStream original = ParseStream(urlImage);
            MemoryStream result = new MemoryStream();

            Image image = Image.FromStream(original);
            Image thumbnail = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(thumbnail);

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            graphics.DrawImage(image, 0, 0, width, height);

            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();

            //string contentType = MimeTypeFromFileExtension(strFileName);
            //ImageCodecInfo codecInfo = GetImageCodec(contentType);

            EncoderParameters encoderParameters;
            encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            thumbnail.Save(result, info[1], encoderParameters);

            result.Position = 0;

            return new FileStreamResult(result, "image/jpeg");
        }
        #endregion
        /// <summary>
        /// Convert image to binary from file name
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Byte[] convertImageToBinary(string filename)
        {
            Byte[] data = null;
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            data = new Byte[stream.Length];
            stream.Read(data, 0, System.Convert.ToInt32(stream.Length));
            return data;
        }
        /// <summary>
        /// Find the right codec
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetImageCodec(string extension)
        {
            extension = extension.ToUpperInvariant();
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FilenameExtension.Contains(extension))
                {
                    return codec;
                }
            }
            return codecs[1];
        }

        public enum ImageModificationType
        {
            Crop,
            Resize
        };

        public static byte[] ModifyImage(Image image, int x, int y, int w, int h, ImageModificationType modType, string extension)
        {
            byte[] result;
            using (Bitmap _bitmap = new Bitmap(w, h))
            {
                _bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (Graphics _graphic = Graphics.FromImage(_bitmap))
                {
                    _graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    _graphic.SmoothingMode = SmoothingMode.HighQuality;
                    _graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    _graphic.CompositingQuality = CompositingQuality.HighQuality;

                    if (modType == ImageModificationType.Crop)
                        _graphic.DrawImage(image, new Rectangle(0, 0, w, h), x, y, w, h, GraphicsUnit.Pixel);
                    else if (modType == ImageModificationType.Resize)
                        _graphic.DrawImage(image, new Rectangle(0, 0, w, h), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

                    using (EncoderParameters encoderParameters = new EncoderParameters(1))
                    {
                        encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                        result = ImageToByteArray(_bitmap, extension, encoderParameters);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Images to byte array.
        /// </summary>
        /// <param name="imageIn">The image in.</param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(Image image)
        {
            return ImageToByteArray(image, null, null);
        }

        /// <summary>
        /// Images to byte array.
        /// </summary>
        /// <param name="image">The image in.</param>
        /// <param name="extension">The extension.</param>
        /// <param name="encoderParameters">The encoder parameters.</param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(Image image, string extension, EncoderParameters encoderParameters)
        {
            MemoryStream ms = new MemoryStream();
            if (!string.IsNullOrEmpty(extension) && encoderParameters != null)
                image.Save(ms, GetImageCodec(extension), encoderParameters);
            else
                image.Save(ms, image.RawFormat);
            return ms.ToArray();
        }

        /// <summary>
        /// Bytes the array to image.
        /// </summary>
        /// <param name="byteArrayIn">The byte array in.</param>
        /// <returns></returns>
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static void InsertImageByLink(string linkImage, string filename)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(linkImage);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Image img = Image.FromStream(response.GetResponseStream());
            img.Save(filename);
            img.Dispose();
        }

        public static void UploadImage(HttpPostedFileBase f, string strNewFile)
        {
            string strFileName = f.FileName;
            string contentType = MimeTypeFromFileExtension(strFileName);
            ImageCodecInfo codecInfo = GetImageCodec(contentType);
            MemoryStream memStream = ParseStream(f);
            Image img = Image.FromStream(memStream);
            Bitmap image = new Bitmap(img);
            Graphics g = Graphics.FromImage(image);
            Bitmap bmpNew = new Bitmap(image);
            g.DrawImage(bmpNew, new Point(0, 0));
            g.Dispose();
            img.Dispose();
            image.Dispose();
            bmpNew.Save(strNewFile, codecInfo, null);
            bmpNew.Dispose();
        }
     
    }

    public class createThumbnail
    {
        public void ResizeImage(string lcFilename, string lcNewFile, int width, int height)
        {
            int lnWidth = width;
            int lnHeight = height;
            System.Drawing.Bitmap bmpOut = null;

            Bitmap loBMP = new Bitmap(lcFilename);
            ImageFormat loFormat = loBMP.RawFormat;

            //decimal lnRatio;
            int lnNewWidth = width;
            int lnNewHeight = height;

            if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                bmpOut = loBMP;
            /*
            if (loBMP.Width > loBMP.Height)
            {
                lnRatio = (decimal)lnWidth / loBMP.Width;
                lnNewWidth = lnWidth;
                decimal lnTemp = loBMP.Height * lnRatio;
                lnNewHeight = (int)lnTemp;
            }
            else
            {
                lnRatio = (decimal)lnHeight / loBMP.Height;
                lnNewHeight = lnHeight;
                decimal lnTemp = loBMP.Width * lnRatio;
                lnNewWidth = (int)lnTemp;
            }
            */
            bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
            Graphics g = Graphics.FromImage(bmpOut);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
            g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

            loBMP.Dispose();
            g.Dispose();
            bmpOut.Save(lcNewFile);
        }

        public void SaveResizedImage(string filePath, string filePathThumb, string resizedFilename, int percent)
        {
            Image origImg = Image.FromFile(Path.Combine(filePath));
            Image resizedImg = ScaleByPercent(origImg, percent);

            resizedImg.Save(Path.Combine(filePathThumb, resizedFilename), ImageFormat.Jpeg);
            resizedImg.Dispose();
            origImg.Dispose();
        }

        public void SaveResizedImage(string filePath, string filePathThumb, string resizedFilename, int width, int height)
        {
            Image origImg = Image.FromFile(Path.Combine(filePath));
            Image resizedImg = FixedSize(origImg, width, height);

            resizedImg.Save(Path.Combine(filePathThumb, resizedFilename), ImageFormat.Jpeg);
            resizedImg.Dispose();
            origImg.Dispose();
        }

        public Image ScaleByPercent(Image imgPhoto, int percent)
        {
            float nPercent = ((float)percent / 100);
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }

        public Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
            grPhoto.Dispose();

            return bmPhoto;
        }
    }
 
}
