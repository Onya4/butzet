using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml.Linq;
using butzet.volleybalDataSetTableAdapters;

namespace butzet
{
    public partial class MainWindow : Window
    {
        moneysTableAdapter moneys = new moneysTableAdapter();
        typeTableAdapter type = new typeTableAdapter(); 
        type_moneyTableAdapter type_money = new type_moneyTableAdapter();
        public MainWindow()
        {
            InitializeComponent();
            uch.ItemsSource = moneys.GetData();
            //загружаем данные оз определённого столбца и выгружаем из другого столбца таблицы type
            Tact.ItemsSource = type.GetData();
            Tact.DisplayMemberPath = "name_type";
            Tact.SelectedValuePath = "type id";
            //в datapicker выводим сегодняшнюю таблицу
            dat.SelectedDate = DateTime.Now;
            var money = moneys.GetData();
            //считаем и выводим деньги у пользователя
            int simm = 0;
            for (int i = 0; i < money.Count; i++)
            {
                if (Convert.ToInt32(money[i][4]) == 1)
                {
                    simm += Convert.ToInt32(money[i][3]);
                }
                else if (Convert.ToInt32(money[i][4]) == 2)
                {
                    simm -= Convert.ToInt32(money[i][3]);
                }
                ito.Text = Convert.ToString(simm);
            }
        }

        private void uch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (uch.SelectedItem != null)
            {
                //загружаем данные строки в переменные
                var uchot = uch.SelectedItem as DataRowView;
                //выгружаем в текстовые поля соответствующие данные
                act.Text = (string)uchot.Row[1];
                Tact.SelectedValue = (int)uchot.Row[2];
                mon.Text = Convert.ToString(uchot.Row[3]);
            }
        }

        private void newTact_Click(object sender, RoutedEventArgs e)
        {
            NewType newT = new NewType();
            newT.Show();
        }

        private void ins_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tMon;                
                if (Convert.ToInt32(mon.Text) < 0)
                {
                    int newtext = Convert.ToInt32(mon.Text) * (-1);
                    mon.Text = Convert.ToString(newtext);
                    tMon = 2;
                }
                else
                {
                    tMon = 1;
                }
                moneys.InsertQuery(act.Text, Convert.ToInt32(Tact.SelectedValue), Convert.ToDecimal(mon.Text), tMon, Convert.ToString(dat.SelectedDate));
                var money = moneys.GetData();
                int simm = 0;
                for (int i = 0; i < money.Count; i++)
                {
                    if (Convert.ToInt32(money[i][4]) == 1)
                    {
                        simm += Convert.ToInt32(money[i][3]);
                    }
                    else if (Convert.ToInt32(money[i][4]) == 2)
                    {
                        simm -= Convert.ToInt32(money[i][3]);
                    }
                    ito.Text = Convert.ToString(simm);
                }
                uch.ItemsSource = moneys.GetData();
               
            }
            catch { MessageBox.Show("Error"); }
        }

        private void upd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tMon = (int)(uch.SelectedItem as DataRowView).Row[4]; ;
                int id = (int)(uch.SelectedItem as DataRowView).Row[0];
                moneys.UpdateQuery(act.Text, Convert.ToInt32(Tact.SelectedValue), Convert.ToDecimal(mon.Text), tMon, Convert.ToString(dat.SelectedDate), id);                    
                //полсчёт суммы
                var money = moneys.GetData();
                int simm = 0;
                for (int i = 0; i < money.Count; i++)
                {
                    if (Convert.ToInt32(money[i][4]) == 1)
                    {
                        simm += Convert.ToInt32(money[i][3]);
                    }
                    else if (Convert.ToInt32(money[i][4]) == 2)
                    {
                        simm -= Convert.ToInt32(money[i][3]);
                    }
                    ito.Text = Convert.ToString(simm);
                }
                uch.ItemsSource = moneys.GetData();
            }
            catch { MessageBox.Show("Error"); }
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //удаляем строку из таблицы
                int id = (int)(uch.SelectedItem as DataRowView).Row[0];
                moneys.DeleteQuery(id);
                var money = moneys.GetData();
                //подсчёт суммы через цикл, создав матрицу таблицы money
                int simm = 0;
                for (int i = 0; i < money.Count; i++)
                {
                    if (Convert.ToInt32(money[i][4]) == 1)
                    {
                        simm += Convert.ToInt32(money[i][3]);
                    }
                    else if (Convert.ToInt32(money[i][4]) == 2)
                    {
                        simm -= Convert.ToInt32(money[i][3]);
                    }
                    ito.Text = Convert.ToString(simm);
                }
                //обновляем таблицу учёта бюджета
                uch.ItemsSource = moneys.GetData();
            }
            catch { MessageBox.Show("Error"); }
        }

        private void uch_LayoutUpdated(object sender, EventArgs e)
        {
            /*if (uch.Columns[5] == dat.SelectedDate)*/
            uch.Columns[0].Visibility = Visibility.Collapsed;
            uch.Columns[5].Visibility = Visibility.Collapsed;

        }
    }
}
