    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для CLientPage.xaml
    /// </summary>
    public partial class CLientPage : Page
    {
        List<Client> CurrentPageList = new List<Client>();
        int CountRecords;
        int CountRecordsMax = BaiguzinLanguageEntities.GetContext().Client.ToList().Count();
        int CountPage;
        int actualSizeLW = 0;
        int CurrentPage = 0;
        List<Client> TableList;

        public CLientPage()
        {
            InitializeComponent();
            List<Client> currentClients = BaiguzinLanguageEntities.GetContext().Client.ToList();

            ClientListView.ItemsSource = currentClients;
            ComboType.SelectedIndex = 0;
            FiltrBox.SelectedIndex = 0;
            SortBox.SelectedIndex = 0;
            TBAllRecords.Text = CountRecordsMax.ToString();
           
            ClientUpdate();
        }


        public void ClientUpdate()
        {
            var currentClients = BaiguzinLanguageEntities.GetContext().Client.ToList();

            if (SortBox.SelectedIndex == 1)
            {
                currentClients = currentClients.OrderBy(p => p.FirstName).ToList();
            }
            else if (SortBox.SelectedIndex == 2)
            {
                currentClients = currentClients.OrderByDescending(p => p.FirstName).ToList();
            }
            else if (SortBox.SelectedIndex == 3)
            {
                currentClients = (List<Client>)currentClients.OrderByDescending(p => DateTime.Parse((p.LastVisitDateN.ToString() != "Нет") ? p.LastVisitDateN.ToString() : "01.01.1991 09:00")).ToList();
            }
            else if (SortBox.SelectedIndex == 4)
            {
                currentClients = currentClients.OrderByDescending(p => p.VisitCount).ToList();
            }

            if (FiltrBox.SelectedIndex == 1)
            {
                currentClients = currentClients.Where(p => p.GenderCode == "ж").ToList();
            }
            else if (FiltrBox.SelectedIndex == 2)
            {
                currentClients = currentClients.Where(p => p.GenderCode == "м").ToList();
            }

            currentClients = currentClients.Where(p => p.LastName.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.FirstName.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Patronymic.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Phone.Replace("+", "").Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").ToLower().Contains(TBoxSearch.Text.Replace("+", "").Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").ToLower())).ToList();
 
            TBAllRecords.Text = BaiguzinLanguageEntities.GetContext().Client.ToList().Count().ToString();
            TBCount.Text = currentClients.Count().ToString();

            ClientListView.ItemsSource = currentClients;

            if (ComboType.SelectedIndex == 0)
            {
                PageListBox.Visibility = Visibility.Visible;
                LeftDirButton.Visibility = Visibility.Visible;
                RightDirButton.Visibility = Visibility.Visible;
            }
            if (ComboType.SelectedIndex == 1)
            {
                PageListBox.Visibility = Visibility.Visible;
                LeftDirButton.Visibility = Visibility.Visible;
                RightDirButton.Visibility = Visibility.Visible;
            }
            if (ComboType.SelectedIndex == 2)
            {
                PageListBox.Visibility = Visibility.Visible;
                LeftDirButton.Visibility = Visibility.Visible;
                RightDirButton.Visibility = Visibility.Visible;
            }
            if (ComboType.SelectedIndex == 3)
            {
                PageListBox.Visibility = Visibility.Hidden;
                LeftDirButton.Visibility = Visibility.Hidden;
                RightDirButton.Visibility = Visibility.Hidden;
            }
            ClientListView.ItemsSource = currentClients.ToList();

            TableList = currentClients;
            CountPage = TableList.Count;
            CountRecords = TableList.Count;
            actualSizeLW = TableList.Count;
            TBAllRecords.Text = " из " + CountRecordsMax.ToString();
            ChangePage(0, 0);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, PageListBox.SelectedIndex);
        }
        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();

            int recordsPerPage;
            switch (ComboType.SelectedIndex)
            {
                case 0:
                    recordsPerPage = 10;
                    break;
                case 1:
                    recordsPerPage = 50;
                    break;
                case 2:
                    recordsPerPage = 200;
                    break;
                default:
                    recordsPerPage = 10000;
                    break;
            }

            CountPage = (int)Math.Ceiling((double)CountRecords / recordsPerPage);

            var ifUpdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage < CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = Math.Min((CurrentPage + 1) * recordsPerPage, CountRecords);
                    for (int i = CurrentPage * recordsPerPage; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = Math.Min((CurrentPage + 1) * recordsPerPage, CountRecords);
                            for (int i = CurrentPage * recordsPerPage; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            ifUpdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = Math.Min((CurrentPage + 1) * recordsPerPage, CountRecords);
                            for (int i = CurrentPage * recordsPerPage; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            ifUpdate = false;
                        }
                        break;
                }
            }
            if (ifUpdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = Math.Min((CurrentPage + 1) * recordsPerPage, CountRecords);

                ClientListView.ItemsSource = CurrentPageList;
                ClientListView.Items.Refresh();
            }
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }


        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientUpdate();
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Client;

            if (currentClient.VisitCount == 0)
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    BaiguzinLanguageEntities.GetContext().Client.Remove(currentClient);
                    BaiguzinLanguageEntities.GetContext().SaveChanges();
                    ClientListView.ItemsSource = BaiguzinLanguageEntities.GetContext().Client.ToList();
                    ClientUpdate();
                }
            }
            else
            {
                MessageBox.Show("Невозможно выполнить удаление, так как клиент посещал школу!");
            }
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Client;

            if (currentClient.VisitCount == 0)
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    BaiguzinLanguageEntities.GetContext().Client.Remove(currentClient);
                    BaiguzinLanguageEntities.GetContext().SaveChanges();
                    ClientListView.ItemsSource = BaiguzinLanguageEntities.GetContext().Client.ToList();
                    ClientUpdate();
                }
            }
            else
            {
                MessageBox.Show("Невозможно выполнить удаление, так как клиент посещал школу!");
            }
            CountRecordsMax = CountRecordsMax - 1;
            TBAllRecords.Text = " из " + CountRecordsMax.ToString();

        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientUpdate();
        }

        private void FiltrBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientUpdate();
        }

        private void TBoxSearch_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ClientUpdate();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var client = button.DataContext as Client;
            if (client != null)
            {
                Manager.MainFrame.Navigate(new AddEditPage(client));
                ClientUpdate();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
            ClientUpdate();
        }
    }
}
