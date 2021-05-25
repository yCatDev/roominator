using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Roominator
{
    public static class FacebookShareHelper
    {
        private static string SaveRoomAsImage(Image image)
        {
            string ImagesDirectory = "../Roominator/wwwroot/images/facebookshare/";
            DirectoryInfo dirInfo = new DirectoryInfo(ImagesDirectory);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
            image.Save(ImagesDirectory + "image.png", System.Drawing.Imaging.ImageFormat.Png);
            return ImagesDirectory + "image.png";
        }

        private static Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream memstr = new MemoryStream(byteArray))
            {
                Image img = Image.FromStream(memstr);
                return img;
            }
        }

        public static void Share(byte[] byteArray)
        {
            Image img = ByteArrayToImage(byteArray);
            SaveRoomAsImage(img);
        }
    }
}
