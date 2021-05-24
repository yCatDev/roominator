using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Roominator
{
    public static class DesignerBridge
    {


        //Метод который будет показывать все доступные комнаты которые есть у человека, принимает его айди (чем оно будет решайте сами)
        public static async Task<UserRoom[]> GetUserRooms(string userId)
        {
            var result = new List<UserRoom>();
            // Берем данные с бд
            string query = $"SELECT * FROM public.room WHERE Id = '{userId}'";
            DataTable dataTable = await Program.databaseManager.ExecQuery(query);
            while (dataTable == null) { }

            UserRoom userRoom;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                userRoom = new UserRoom();
                // Заполняем
                userRoom.Json = (string)dataTable.Rows[i]["room_furniture"];
                userRoom.Preview = (byte[])dataTable.Rows[i]["room_image"];
                userRoom.Name = (string)dataTable.Rows[i]["room_name"];
                userRoom.RoomId = (int)dataTable.Rows[i]["room_id"];

                result.Add(userRoom);
            }
            return result.ToArray();
        }


        // Метод для получения конкретной комнаты которую выберет пользователь
        public static async Task<UserRoom> GetUserRoomById(string userId, int roomId)
        {
            UserRoom userRoom = new UserRoom();

            string query = $"SELECT * FROM public.room WHERE Id = '{userId}' AND room_id = {roomId}";
            DataTable dataTable = await Program.databaseManager.ExecQuery(query);

            if (dataTable.Rows.Count > 0)
            {
                userRoom.Json = (string)dataTable.Rows[0]["room_furniture"];
                userRoom.Preview = (byte[])dataTable.Rows[0]["room_image"];
                userRoom.Name = (string)dataTable.Rows[0]["room_name"];
                userRoom.RoomId = (int)dataTable.Rows[0]["room_id"];
                return userRoom;
            }
            // Вдруг нету
            return null;
        }

        // Метод для создания комнаты. Возрващает ее айди
        public static async Task<int> CreateRoom(string userId)
        {
            string query = $"INSERT INTO public.room (id) VALUES ('{userId}')";
            await Program.databaseManager.ExecQuery(query);
            query = "SELECT room_id FROM public.room ORDER BY room_id DESC";
            DataTable dataTable = await Program.databaseManager.ExecQuery(query);
            while (dataTable == null) { }
            return (int)dataTable.Rows[0].ItemArray[0];
        }

        // Метод для удаления комнаты.
        public static async Task RemoveRoom(string userId, int roomId)
        {
            string query = $"DELETE FROM public.room WHERE Id = '{userId}' AND room_id = {roomId}";
            await Program.databaseManager.ExecQuery(query);
        }

        // Метод который будет сохранять комнату передавая из юнити ид юзера и экземпляр комнаты
        public static async Task SaveUserRoom(string userId, UserRoom userRoom)
        {
            string img = System.Text.Encoding.Default.GetString(userRoom.Preview);
            string query = $"UPDATE public.room SET Id = '{userId}', room_name = '{userRoom.Name}', " +
                $"room_furniture = '{userRoom.Json}', room_image = '{img}' WHERE room_id = {userRoom.RoomId}";
            await Program.databaseManager.ExecQuery(query);
        }

        // Метод который устанавливает массив байтов для картинки комнаты. Возвращает обновленную комнату
        public static UserRoom SetImageForUserRoom(UserRoom userRoom, string path)
        {
            byte[] imageBytes = File.ReadAllBytes(path);
            userRoom.Preview = imageBytes;
            return userRoom;
        }

        // Приблизительный метод для дешифровки байтов в картинку на блазоре. Картинку генерировать будет юнити это нужно только для превью
        internal static string GetPreviewImage(UserRoom userRoom)
        {
            string base64String = Convert.ToBase64String(userRoom.Preview, 0, userRoom.Preview.Length); // Convert the bytes to base64 string  
            return "data:image/png;base64," + base64String;


            /*   
              ... - примерный код для разор пейджа чтобы отобразить все эти превью
              var rooms = DesignerBridge.GetUserRooms(userId);
              var preview = new string[rooms.Lenght];
              for (int i = 0; i<rooms.Lenght; i++)
              {
                preview[i] = DesignerBridge.GetPreviewImage(rooms[i]);
              }  
              ...
              @foreach (var src in preview)
              {
                <div class="preview"> 
                  <img src="@src" /> 
                </div>
               }
            */
        }

    }

    // Мост между комнатами в БД и конструктором
    // Пердставляет собой набор самых необходимых полей чтобы позволить без костылей передавать базовые данные для сохранения\загрузки
    public class UserRoom
    {
        public string Json; // Сам гсон с данными о комнате
        public byte[] Preview; // Набор байтов в котором будет представленна картинка превьюшки
        public string Name; // Имя комнаты 
        public int RoomId; // ID комнаты для обращения в БД.
    }
}
