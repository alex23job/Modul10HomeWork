using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ConsultantWpfApp
{
    /// <summary>
    /// Логика взаимодействия для UserEditWindow.xaml
    /// </summary>
    public partial class UserEditWindow : Window
    {
        ObservableCollection<UserPosition> usPos = null;
        ObservableCollection<UserData> dbUsers = null;
        public ObservableCollection<UserData> GetDB()
        {
            return dbUsers;
        }

        public UserEditWindow()
        {
            InitializeComponent();
        }

        private void OnAddUserClick(object sender, RoutedEventArgs e)
        {
            if (txtNewUserLogin.Text != "")
            {
                usPos.Add(new UserPosition(txtNewUserLogin.Text, 1));
            }
        }

        public void SetUsers(ObservableCollection<UserData> db)
        {
            dbUsers = db;
            usPos = new ObservableCollection<UserPosition>();
            foreach (UserData us in db)
            {
                usPos.Add(new UserPosition(us.UserLogin, us.Rule));
            }

            listViewUsers.ItemsSource = usPos;
            listViewUsers.SelectedIndex = 0;
        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            int i;
            bool isNew = true;
            foreach(UserPosition up in usPos)
            {
                isNew = true;
                //MessageBox.Show(up.ToString());
                for (i = 0; i < dbUsers.Count; i++)
                {
                    UserData ud = dbUsers[i];
                    if (ud.UserLogin == up.UserLogin)
                    {
                        if (ud.Rule != up.Rule)
                        {
                            ud.Rule = up.Rule;
                        }
                        isNew = false;
                        break;
                    }
                }
                if (isNew)
                {
                    dbUsers.Add(new UserData(up.UserLogin, "11111", up.Rule));
                }
            }
            this.DialogResult = true;
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ////MessageBox.Show($"Rule={((UserPosition)listViewUsers.SelectedItem).Rule} selInd={((UserPosition.)sender..Source).Rule}");
            //MessageBox.Show($"selInd={((ComboBox)sender).SelectedItem}");
            ////MessageBox.Show($"Rule={((UserPosition)listViewUsers.SelectedItem).Rule}");
        }

        private void OnChangePassword(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            ListViewItem lvi = listViewUsers.ItemContainerGenerator.ContainerFromItem(menu.DataContext) as ListViewItem;
            ChangePasswordWindow cpw = new ChangePasswordWindow();
            int i;
            for (i = 0; i < dbUsers.Count; i++)
            {
                if (dbUsers[i].UserLogin == ((UserPosition)lvi.DataContext).UserLogin)
                {
                    cpw.SetUser(dbUsers[i]);
                    break;
                }
            }
            if (cpw.ShowDialog() == true)
            {
                for (i = 0; i < dbUsers.Count; i++)
                {
                    if (dbUsers[i].UserLogin == cpw.GetUser().UserLogin)
                    {
                        dbUsers[i].Password = cpw.GetUser().Password;
                        break;
                    }
                }
            }
        }

        private void OnDelUser(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            ListViewItem lvi = listViewUsers.ItemContainerGenerator.ContainerFromItem(menu.DataContext) as ListViewItem;
            //MessageBox.Show(((UserPosition)lvi.DataContext).ToString());
            string info = string.Format("\"Логин: {0}   должность: {1}\"", ((UserPosition)lvi.DataContext).UserLogin, UserPosition.GetPosition(((UserPosition)lvi.DataContext).Rule));
            if (MessageBox.Show($"Выбрана запись сотрудника : {info}\n\nУдалить запись ?", "Удаление записи сотрудника", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int i, index = -1;
                for (i = 0; i < dbUsers.Count; i++)
                {
                    if (dbUsers[i].UserLogin == ((UserPosition)lvi.DataContext).UserLogin)
                    {
                        index = i;
                        break;
                    }
                }
                usPos.Remove((UserPosition)lvi.DataContext);
                if (index != -1)
                {
                    dbUsers.RemoveAt(index);
                }
            }
        }
    }

    public class UserPosition
    {
        private static readonly List<string> usersPos;

        static UserPosition()
        {
            usersPos = new List<string>();
            usersPos.Add("Клиент");
            usersPos.Add("Консультант");
            usersPos.Add("Менеджер");
            usersPos.Add("Администратор");
        }

        public UserPosition(string log, int r)
        {
            UserLogin = log;
            Rule = r;
        }

        public string UserLogin { get; set; }
        public int Rule { get; set; }
        public IEnumerable<string> Positions => usersPos;

        public static string GetPosition(int index)
        {
            return usersPos[index];
        }

        public override string ToString()
        {
            return $"Login={UserLogin} Rule={Rule}";
        }
    }
}
