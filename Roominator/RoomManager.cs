using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using Npgsql;


namespace Roominator {
    public class PictureManager {

        public async System.Threading.Tasks.Task<List<Image>> fromDbToPictureAsync(string id) {
            string query = $"SELECT image FROM room_images WHERE Id = '{id}'";
            var res = new List<Image>();
            DataTable dataTable = await Program.databaseManager.ExecQuery(query);
            for (int i = 0; i < dataTable.Rows.Count; i++) {
                Byte[] byteImg = (Byte[])dataTable.Rows[i].ItemArray[0];
                MemoryStream memoryStream = new MemoryStream(byteImg);
                res.Add((Bitmap)Image.FromStream(memoryStream));
            }
            return res;
        }

        public void createFolderWithImages(List<Image> list) { 
            
        }

        //public async System.Threading.Tasks.Task picturesToDbAsync(List<Image> list, string id) {
        //    byte[] imgToDb;
        //    ImageConverter converter = new ImageConverter();
        //    List<Byte[]> byteImgs = new List<Byte[]>();
        //    string query = $"SELECT img FROM room_images WHERE Id = '{id}'";
        //    DataTable imgInDb = await Program.databaseManager.ExecQuery(query);
        //    for (int i = 0; i < imgInDb.Rows.Count; i++) {
        //        byteImgs.Add((Byte[])imgInDb.Rows[i].ItemArray[0]);
        //    }
        //    foreach (var img in list)
        //    {
        //        byte[] img;
        //        ImageConverter imageConverter = new ImageConverter();
        //        img = (byte[])img
        //    }
        //}
    }
}