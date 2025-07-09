using System.Windows;

namespace ProjektTimeseddel.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            cbYear.IsEnabled = false;
            cbMonth.IsEnabled = false;
            btnAddWorkDay.IsEnabled = true;
            cbDays.IsEnabled = true;
            btnStart.IsEnabled = false;
        }

        private void btnAddWorkDay_Click(object sender, RoutedEventArgs e)
        {
            btnCalc.IsEnabled = true;
            cbDays.IsEnabled = false;
            btnAddWorkDay.IsEnabled = false;
        }

        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            btnAddWorkDay.IsEnabled = true;
            cbDays.IsEnabled = true;
            btnCalc.IsEnabled = false;
        }
    }
}