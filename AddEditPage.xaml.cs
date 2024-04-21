using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace BAIGUZINLANGUAGE
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Client _currentClient = new Client();
        public AddEditPage(Client currentClient)
        {
            InitializeComponent();
            if (currentClient != null)
                _currentClient = currentClient;
            DataContext = _currentClient;
            if (currentClient == null)
            {
                var ddd = BaiguzinLanguageEntities.GetContext().Client;
                int a = ddd.Max(p => p.ID);
                a = a + 1;
                IDClient.Text = a.ToString();
            }
        }
        public static bool IsValidFIO(string fio)
        {
            // Регулярное выражение для проверки ФИО
            string pattern = @"^[A-Za-zА-Яа-яЁё\s\-']{1,50}$";

            // Проверка совпадения с шаблоном
            return Regex.IsMatch(fio, pattern);
        }
        public static bool IsValidPhone(string phone)
        {
            // Регулярное выражение для проверки телефона
            string pattern = @"^[+()0-9\s-]+$";

            // Проверка совпадения с шаблоном
            return Regex.IsMatch(phone, pattern);
        }
        public static bool IsValidEmail(string email)
        {
            // Регулярное выражение для проверки email
            string pattern = @"^.+@.+\..+$";

            // Проверка совпадения с шаблоном
            return Regex.IsMatch(email, pattern);
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentClient.FirstName))
            {
                errors.AppendLine("Укажите Фамилию");
            }
            if (string.IsNullOrWhiteSpace(_currentClient.LastName))
            {
                errors.AppendLine("Укажите Имя");
            }
            if (string.IsNullOrWhiteSpace(_currentClient.Patronymic))
            {
                errors.AppendLine("Укажите Отчество");
            }
            if (string.IsNullOrWhiteSpace(_currentClient.Phone))
            {
                errors.AppendLine("Укажите Телефон");
            }
            if (string.IsNullOrWhiteSpace(_currentClient.Email))
            {
                errors.AppendLine("Укажите Почту");
            }
            if (string.IsNullOrWhiteSpace(_currentClient.Birthday.ToString()))
            {
                errors.AppendLine("Укажите Дату рождения");
            }
            if (CB_Sel.SelectedItem == null)
            {
                errors.AppendLine("Укажите пол");
            }
            if (!IsValidFIO(Famil.Text))
            {
                errors.AppendLine("Укажите фамилию правильно(Поля фамилии, имени и отчества не могут быть длиннее 50 символов)");
            }
            if (!IsValidFIO(Name.Text))
            {
                errors.AppendLine("Укажите имя правильно(Поля фамилии, имени и отчества не могут быть длиннее 50 символов)");
            }
            if (!IsValidFIO(Otchestvo.Text))
            {
                errors.AppendLine("Укажите отчество правильно(Поля фамилии, имени и отчества не могут быть длиннее 50 символов)");
            }
            if (!IsValidPhone(Phone.Text))
            {
                errors.AppendLine("Укажите телефон правильно");
            }
            if (!IsValidEmail(Email.Text))
            {
                errors.AppendLine("Укажите почту правильно");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (CB_Sel.SelectedIndex == 0)
                _currentClient.GenderCode = "ж";
            else
                _currentClient.GenderCode = "м";
            _currentClient.RegistrationDate = DateTime.Now;
            if (_currentClient.ID == 0)
            {
                BaiguzinLanguageEntities.GetContext().Client.Add(_currentClient);
            }
            try
            {
                BaiguzinLanguageEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                CLientPage clientPage = Manager.MainFrame.Content as CLientPage;
                if (clientPage != null)
                {
                    clientPage.ClientUpdate();
                }
                
                Manager.MainFrame.Navigate(new CLientPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _currentClient.PhotoPath = openFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }


    }
}