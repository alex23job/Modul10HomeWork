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

namespace ConsultantWpfApp
{
    /// <summary>
    /// Логика взаимодействия для PersonWindow.xaml
    /// </summary>
    public partial class PersonWindow : Window
    {
        public Person per { get; set; }
        public class MyParam
        {
            public string paramName {get; set;}
            public string paramValue { get; set; }

            public MyParam(string name, string value)
            {
                paramName = name;
                paramValue = value;
            }
        }

        public PersonWindow()
        {
            InitializeComponent();
        }

        public void SetPerson(Person p)
        {
            per = p;
            listView.Items.Add(new MyParam("Фамилия", per.Name));
            listView.Items.Add(new MyParam("Имя", per.LastName));
            listView.Items.Add(new MyParam("Отчество", per.SecondName));
            listView.Items.Add(new MyParam("Паспорт", per.Pasport));
            listView.Items.Add(new MyParam("Телефон", per.Tlf));
            //((TextBox)((GridView)listView.View).Columns[1].CellTemplate.LoadContent()).Width = 150;
            //MessageBox.Show(((GridView)listView.View).Columns[1].CellTemplate.LoadContent().ToString());
            //MessageBox.Show(((GridView)listView.View).Columns.Count.ToString());
            //MessageBox.Show(listView.View.GetType().ToString());
            //MessageBox.Show(((ListViewItem)listView.Items[0]).ContentTemplate.ToString());
        }

        private void ClickOK(object sender, RoutedEventArgs e)
        {
            per.Name = ((MyParam)listView.Items[0]).paramValue;
            per.LastName = ((MyParam)listView.Items[1]).paramValue;
            per.SecondName = ((MyParam)listView.Items[2]).paramValue;
            per.Pasport = ((MyParam)listView.Items[3]).paramValue;
            per.Tlf = ((MyParam)listView.Items[4]).paramValue;
            this.DialogResult = true;
        }

        private void ClickCansel(object sender, RoutedEventArgs e)
        {

        }
    }
}
