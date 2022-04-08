using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PostButton_Click(object sender, RoutedEventArgs e)
        {

            Models.User user = new Models.User
            {
                FirstName = "Test",
                LastName = "Test",
                Login = "1",
                Password = "1",
                Patronymic = "Test"
            };

            WebRequest request = WebRequest.Create("http://localhost:54047/api/Users");
            request.Method = "POST";
            // Данные для отправки
            string data = System.Text.Json.JsonSerializer.Serialize(user);
            // преобразуем данные в массив байтов
            byte[] array = System.Text.Encoding.UTF8.GetBytes(data); ;
            // Устанавливаем тип содержимого
            request.ContentType = "application/json";
            // Устанавливаем заголовок
            request.ContentLength = array.Length;

            // Записываем данные в поток
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(array, 0, array.Length);
            }
            var line = "";
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using(StreamReader reader = new StreamReader(stream))
                {
                    line = reader.ReadToEnd();
                }
            }
            response.Close();
            var resUser = System.Text.Json.JsonSerializer.Deserialize<Models.User>(line);
            var items = new List<Models.User> { resUser };
            List.ItemsSource = items;




        }

        private void GetButton_Click(object sender, RoutedEventArgs e)
        {
            using(var client = new HttpClient())
            {
                var res = client.GetStringAsync("http://localhost:54047/api/Users").Result;
                var users = System.Text.Json.JsonSerializer.Deserialize<List<Models.User>>(res);
                
                List.ItemsSource = users;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
