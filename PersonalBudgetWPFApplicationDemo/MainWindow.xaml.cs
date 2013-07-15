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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonalBudgetWPFApplicationDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PbDataGrid.ItemsSource = PbEntry.List();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var entry = new PbEntry();
            entry.ItemName = ItemTextBox.Text;
            entry.FromDate = FromDateBox.Text;
            entry.ToDate = ToDateBox.Text;
            entry.CurrentDate = CurrentDateBox.Text;
            entry.Amount = AmountBox.Text;
            entry.Status = Int32.Parse(StatusBox.Text);
            entry.Save();
            PbDataGrid.ItemsSource = PbEntry.List();
        }
    }
}
