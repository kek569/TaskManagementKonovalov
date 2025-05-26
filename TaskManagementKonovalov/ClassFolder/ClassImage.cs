using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using Rectangle = System.Drawing.Rectangle;

namespace TaskManagementKonovalov.ClassFolder
{
    internal class ClassImage
    {
        public static BitmapImage ConvertByteArrayToImage(byte[] array)
        {

            /// <summary>
            /// Метод для конвертации из БД в Image на окне
            /// </summary>
            /// <param name="array"></param>
            /// <returns>Изображение</returns>
            //если байт массив не пустой
            if (array != null)
            {
                //записываем полученные данные в массив для дальнейшей работы
                using (var ms = new MemoryStream(array, 0, array.Length))
                {
                    //инициализируется новый экземпляр класса BitmapImage,
                    //который необходим для поддержки синтаксиса XAML
                    //и предоставляет дополнительные свойства для загрузки
                    //битовой карты
                    var image = new BitmapImage();
                    //инициализируем объект BitmapImage
                    image.BeginInit();
                    //обработка кэша из хранилища
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    //получаем исходный поток
                    image.StreamSource = ms;
                    //завершаем инициализацию
                    image.EndInit();
                    return image;
                }
            }
            return null;
        }

        public static byte[] ConvertImageToArray(string fileName)
        {
            Bitmap bitmap = new Bitmap(fileName);
            ImageFormat bmpFormat = bitmap.RawFormat;
            var imageConvert = Image.FromFile(fileName);

            using (var ms = new MemoryStream())
            {
                imageConvert.Save(ms, bmpFormat);
                return ms.ToArray();
            }
        }

        public static System.Drawing.Image ConvertToImage(BitmapImage image)
        {
            int stride = image.PixelWidth * 4;
            byte[] buffer = new byte[stride * image.PixelHeight];
            image.CopyPixels(buffer, stride, 0);

            // create bitmap
            System.Drawing.Bitmap bitmap =
                new System.Drawing.Bitmap(
                    image.PixelWidth,
                    image.PixelHeight,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // lock bitmap data
            System.Drawing.Imaging.BitmapData bitmapData =
                bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    bitmap.PixelFormat);

            

            // copy byte array to bitmap data
            System.Runtime.InteropServices.Marshal.Copy(
                buffer, 0, bitmapData.Scan0, buffer.Length);

            // unlock
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        public static BitmapImage ConvertToBitmapImage(System.Drawing.Image image)
        {
            using (var memory = new MemoryStream())
            {
                image.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static System.Drawing.Image FixedSize(Image image, int Width, int Height, bool needToFill)
        {
            #region calculations
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            double destX = 0;
            double destY = 0;

            double nScale = 0;
            double nScaleW = 0;
            double nScaleH = 0;

            nScaleW = ((double)Width / (double)sourceWidth);
            nScaleH = ((double)Height / (double)sourceHeight);
            if (!needToFill)
            {
                nScale = Math.Min(nScaleH, nScaleW);
            }
            else
            {
                nScale = Math.Max(nScaleH, nScaleW);
                destY = (Height - sourceHeight * nScale) / 2;
                destX = (Width - sourceWidth * nScale) / 2;
            }

            if (nScale > 1)
                nScale = 1;

            int destWidth = (int)Math.Round(sourceWidth * nScale);
            int destHeight = (int)Math.Round(sourceHeight * nScale);
            #endregion

            System.Drawing.Bitmap bmPhoto = null;
            try
            {
                bmPhoto = new System.Drawing.Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
                    destWidth, destX, destHeight, destY, Width, Height), ex);
            }
            using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto))
            {
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.CompositingQuality = CompositingQuality.HighQuality;
                grPhoto.SmoothingMode = SmoothingMode.HighQuality;

                Rectangle to = new System.Drawing.Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
                Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
                //Console.WriteLine("From: " + from.ToString());
                //Console.WriteLine("To: " + to.ToString());
                grPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);
                //bmPhoto.Save(@"D:\final1.jpg");
                return bmPhoto;
            }
        }
    }
}
