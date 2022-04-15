using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Librarium.DataBase;
using Librarium.Classes;
using Microsoft.Win32;
using System.IO;

namespace Librarium.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddReaderWindow.xaml
    /// </summary>
    public partial class AddReaderWindow : Window
    {
        string pathPhoto = null;

        public AddReaderWindow(DataBase.Reader reader)
        { 
            InitializeComponent();

            if (reader.Photo != null)
            {
                using (MemoryStream stream = new MemoryStream(reader.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    imgUser.Source = bitmapImage;

                }
            }

        }

        public AddReaderWindow() //DataBase.Reader reader
        {
            InitializeComponent();

            genderBox.ItemsSource = AppData.Context.Gender.ToList();

            genderBox.DisplayMemberPath = "NameGender";

            genderBox.SelectedIndex = 0;
        }

        private void AddReader_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lastnameField.Text))
            {
                MessageBox.Show("Ошибка", "Пустое поле фамилии", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(nameField.Text))
            {
                MessageBox.Show("Ошибка", "Пустое поле имени", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(numberField.Text))
            {
                MessageBox.Show("Ошибка", "Пустое поле номера телефона", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(emailField.Text))
            {
                MessageBox.Show("Ошибка", "Пустое поле email", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(addressField.Text))
            {
                MessageBox.Show("Ошибка", "Пустое поле адреса", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (numberField.Text.Length > 20 || numberField.Text.Length < 10)           //провека количества символов в номере
            {
                MessageBox.Show("Неправильное количество символов в номере телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (addressField.Text.Length > 50)
            {
                MessageBox.Show("количество символов в графе адрес слишком велико", "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!emailField.Text.Contains("@"))
            {
                MessageBox.Show("В почте должен присутствовать символ @", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!emailField.Text.Contains(".com") || !emailField.Text.Contains(".list") || !emailField.Text.Contains(".mail") || !emailField.Text.Contains(".io"))
            {
                MessageBox.Show("вы забыли дописать домен почты например: .com/.list", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (imgUser.Source == null)
            {
                MessageBox.Show("А фотографию читателя?", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataBase.Reader reader = new DataBase.Reader();
            reader.LastName = lastnameField.Text;
            reader.FirstName = nameField.Text;
            reader.Phone = numberField.Text;
            reader.Email = emailField.Text;
            reader.Address = addressField.Text;
            reader.IDGender = genderBox.SelectedIndex + 1;


            if (pathPhoto != null)
            {
                reader.Photo = File.ReadAllBytes(pathPhoto);
            }


            AppData.Context.Reader.Add(reader);
            AppData.Context.SaveChanges();
            MessageBox.Show("Пользователь добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();

        }

        private void DeterminatePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                imgUser.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                pathPhoto = openFileDialog.FileName;
            }
        }
    }
}
