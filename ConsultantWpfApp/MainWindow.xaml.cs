using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace ConsultantWpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<UserData> dbUsers = new ObservableCollection<UserData>();
        string pathDbUsers = "User.xml";

        ObservableCollection<Person> dbClients = new ObservableCollection<Person>();
        string pathDbClients = "Person.xml";

        UserData currentUser = null;

        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(pathDbClients))
            {
                dbClients = DeserializePersonList(pathDbClients);
            }
            if (File.Exists(pathDbUsers))
            {
                dbUsers = DeserializeUserList(pathDbUsers);
            }
            if (dbUsers.Count == 0)
            {
                dbUsers.Add(new UserData("admin", "12345", 3));
                dbUsers.Add(new UserData("Petrov", "11111", 1));
                dbUsers.Add(new UserData("Popov", "34343", 2));
            }
            if (dbClients.Count == 0)
            {
                dbClients.Add(new Person("Иванов", "Олег", "Сергеевич", "0120 123456", "+79091113223"));
                dbClients.Add(new Person("Павлов", "Дмитрий", "Васильевич", "0132 234567", "+79091125342"));
                dbClients.Add(new Person("Зорина", "Анастасия", "Евгеньевна", "0155 345689", "+79091357658"));
            }
            listDbView.ItemsSource = dbClients;
            listDbView.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow();
            if (lw.ShowDialog() == true)
            {
                for (int i = 0; i < dbUsers.Count; i++)
                {
                    if ((lw.userLogin == dbUsers[i].UserLogin) && (lw.userPassword == dbUsers[i].Password))
                    {
                        switch(dbUsers[i].Rule)
                        {
                            case 1:
                                currentUser = new Consultant(dbUsers[i].UserLogin, dbUsers[i].Password);
                                break;
                            case 2:
                                currentUser = new Manager(dbUsers[i].UserLogin, dbUsers[i].Password);
                                break;
                            case 3:
                                currentUser = new Administrator(dbUsers[i].UserLogin, dbUsers[i].Password);
                                break;
                            default:
                                currentUser = dbUsers[i];
                                break;
                        }
                        userName.Content = currentUser.UserLogin;
                        IUserRights userRights = currentUser as IUserRights;
                        if (userRights != null)
                        {
                            btnAdd.IsEnabled = userRights.IsAddingPerson();
                            btnDel.IsEnabled = userRights.IsAddingPerson();
                            btnEdit.IsEnabled = true;
                            btnEditUsers.Visibility = userRights.IsEditingUser() ? Visibility.Visible : Visibility.Hidden;
                        }
                        else
                        {
                            btnAdd.IsEnabled = false;
                            btnDel.IsEnabled = false;
                            btnEdit.IsEnabled = false;
                            btnEditUsers.Visibility = Visibility.Hidden;
                        }
                        return;
                    }
                }
                MessageBox.Show("Ошибка ввода логина и/или пароля !!!");
            }
        }

        /// <summary>
        /// Метод сериализации ObservableCollection<Person>
        /// </summary>
        /// <param name="myList">Коллекция для сериализации</param>
        /// <param name="Path">Путь к файлу</param>
        private void SerializePersonList(ObservableCollection<Person> myList, string Path)
        {
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Person>));

            // Создаем поток для сохранения данных
            Stream fStream = new FileStream(Path, FileMode.Create, FileAccess.Write);

            // Запускаем процесс сериализации
            xmlSerializer.Serialize(fStream, myList);

            // Закрываем поток
            fStream.Close();
        }

        /// <summary>
        /// Метод десериализации PersonList
        /// </summary>
        /// <param name="Path">Путь к файлу</param>
        private ObservableCollection<Person> DeserializePersonList(string Path)
        {
            ObservableCollection<Person> tempPersonCol = new ObservableCollection<Person>();
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Person>));

            // Создаем поток для чтения данных
            Stream fStream = new FileStream(Path, FileMode.Open, FileAccess.Read);

            // Запускаем процесс десериализации
            tempPersonCol = xmlSerializer.Deserialize(fStream) as ObservableCollection<Person>;

            // Закрываем поток
            fStream.Close();

            // Возвращаем результат
            return tempPersonCol;
        }
        /// <summary>
        /// Метод сериализации ObservableCollection<Person>
        /// </summary>
        /// <param name="myList">Коллекция для сериализации</param>
        /// <param name="Path">Путь к файлу</param>
        private void SerializeUserList(ObservableCollection<UserData> myList, string Path)
        {
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<UserData>));

            // Создаем поток для сохранения данных
            Stream fStream = new FileStream(Path, FileMode.Create, FileAccess.Write);

            // Запускаем процесс сериализации
            xmlSerializer.Serialize(fStream, myList);

            // Закрываем поток
            fStream.Close();
        }

        /// <summary>
        /// Метод десериализации PersonList
        /// </summary>
        /// <param name="Path">Путь к файлу</param>
        private ObservableCollection<UserData> DeserializeUserList(string Path)
        {
            ObservableCollection<UserData> tempPersonCol = new ObservableCollection<UserData>();
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<UserData>));

            // Создаем поток для чтения данных
            Stream fStream = new FileStream(Path, FileMode.Open, FileAccess.Read);

            // Запускаем процесс десериализации
            tempPersonCol = xmlSerializer.Deserialize(fStream) as ObservableCollection<UserData>;

            // Закрываем поток
            fStream.Close();

            // Возвращаем результат
            return tempPersonCol;
        }

        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SerializePersonList(dbClients, pathDbClients);
            SerializeUserList(dbUsers, pathDbUsers);
        }

        private void ListDbView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //+userName.Content = listDbView.SelectedIndex.ToString();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            PersonWindow pw = new PersonWindow();
            pw.SetPerson(new Person("", "", "", "", ""));
            if (pw.ShowDialog() == true)
            {
                dbClients.Add(new Person(pw.per.Name, pw.per.LastName, pw.per.SecondName, pw.per.Pasport, pw.per.Tlf));
            }
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            //PersonWindow pw = new PersonWindow();
            PersonEditWindow pw = new PersonEditWindow();
            if (!pw.SetParam((Person)listDbView.SelectedItem, currentUser)) return;
            if (pw.ShowDialog() == true)
            {
                dbClients[listDbView.SelectedIndex] = pw.GetPerson();
                listDbView.Items.Refresh();
            }
        }

        private void DelClick(object sender, RoutedEventArgs e)
        {
            string info = string.Format("\"Фамилия: {0}   тлф: {1}\"", ((Person)listDbView.SelectedItem).Name, ((Person)listDbView.SelectedItem).Tlf);
            if (MessageBox.Show($"Выбрана запись клиента : {info}\n\nУдалить запись ?", "Удаление записи клиента", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                dbClients.RemoveAt(listDbView.SelectedIndex);
            }
        }

        private void btnEditUsersClick(object sender, RoutedEventArgs e)
        {
            UserEditWindow uw = new UserEditWindow();
            uw.SetUsers(dbUsers);
            if (uw.ShowDialog() == true)
            {

            }
        }
    }
}
