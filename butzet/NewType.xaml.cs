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
using System.Windows.Shapes;
using butzet.volleybalDataSetTableAdapters;

namespace butzet
{
    public partial class NewType : Window
    {
        typeTableAdapter type = new typeTableAdapter();

        public NewType()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                type.InsertQuery(N_nT.Text);
                (Application.Current.MainWindow as MainWindow).Tact.ItemsSource = type.GetData();
                Close();
            }
            catch { MessageBox.Show("Error"); }
        }
    }
}
