using ProjektTimeseddel.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ProjektTimeseddel.ViewModels
{
    /* TO DO:
     *  - Automatisk kolon i tidspunkter + begrænsninger for input (validation)
     *  - Bedre navigation med tastatur
     * Bonus features:
     *  - Slet-knap til arbejdsdag
     *  - Rediger-knap ved hver arbejdsdag (låser alt på nær dagens tidspunkter og "Udregn dag")
     *  - Clear-knap til at starte forfra
     *  - "Sygetimer"-kolonne
     *  - Synlig scrollbar?
     */

    /* SPØRGSMÅL:
     * Skal der stå 0 eller være tomt, når der er 0 timer i et interval?
     */

    public class MainViewModel : INotifyPropertyChanged
    {
        private int year;
        private int monthIndex;
        private int date;
        private double totalWorkHrs;
        private double totalManFreHrs;
        private double totalLør1117Hrs;
        private double totalLør1724Hrs;
        private double totalSønHelHrs;
        private double totalMan0006Hrs;
        private ObservableCollection<int> days = [];

        public ObservableCollection<WorkDayViewModel> WorkDayVMs { get; set; } = [];
        public List<double> TotalHrList { get; set; } = [];

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Year
        {
            get { return year; }
            set { year = value; OnPropertyChanged(nameof(Year)); }
        }
        public int MonthIndex
        {
            get => monthIndex;
            set
            {
                monthIndex = value;
                Month = value + 1;
                Days.Clear();
                for (int i = 1; i <= DateTime.DaysInMonth(year, Month); i++)
                {
                    Days.Add(i);
                }
                Date = 1;
                OnPropertyChanged(nameof(MonthIndex));
            }
        }
        public int Month { get; set; }
        public int Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged(nameof(Date)); }
        }
        public ObservableCollection<int> Years { get; set; }
        public ObservableCollection<string> Months { get; set; }
        public ObservableCollection<int> Days
        {
            get { return days; }
            set { days = value; OnPropertyChanged(nameof(Days)); }
        }
        public double TotalWorkHrs
        {
            get { return totalWorkHrs; }
            set
            {
                totalWorkHrs = value;
                if (TotalHrList.Count < 6) TotalHrList.Add(value);
                else TotalHrList[0] = value;
                OnPropertyChanged(nameof(TotalWorkHrs));
            }
        }
        public double TotalManFreHrs
        {
            get { return totalManFreHrs; }
            set
            {
                totalManFreHrs = value;
                if (TotalHrList.Count < 6) TotalHrList.Add(value);
                else TotalHrList[1] = value;
                OnPropertyChanged(nameof(TotalManFreHrs));
            }
        }
        public double TotalLør1117Hrs
        {
            get { return totalLør1117Hrs; }
            set
            {
                totalLør1117Hrs = value;
                if (TotalHrList.Count < 6) TotalHrList.Add(value);
                else TotalHrList[2] = value;
                OnPropertyChanged(nameof(TotalLør1117Hrs));
            }
        }
        public double TotalLør1724Hrs
        {
            get { return totalLør1724Hrs; }
            set
            {
                totalLør1724Hrs = value;
                if (TotalHrList.Count < 6) TotalHrList.Add(value);
                else TotalHrList[3] = value;
                OnPropertyChanged(nameof(TotalLør1724Hrs));
            }
        }
        public double TotalSønHelHrs
        {
            get { return totalSønHelHrs; }
            set
            {
                totalSønHelHrs = value;
                if (TotalHrList.Count < 6) TotalHrList.Add(value);
                else TotalHrList[4] = value;
                OnPropertyChanged(nameof(TotalSønHelHrs));
            }
        }
        public double TotalMan0006Hrs
        {
            get { return totalMan0006Hrs; }
            set
            {
                totalMan0006Hrs = value;
                if (TotalHrList.Count < 6) TotalHrList.Add(value);
                else TotalHrList[5] = value;
                OnPropertyChanged(nameof(TotalMan0006Hrs));
            }
        }
        public ICommand AddCmd { get; set; }
        public ICommand CalcCmd { get; set; }
        public ICommand InsertCmd { get; set; }

        public MainViewModel()
        {
            AddCmd = new AddCommand(this);
            CalcCmd = new CalculateCommand(this);
            InsertCmd = new InsertCommand(this);

            int lastYear = DateTime.Now.AddYears(-1).Year;
            int currentYear = DateTime.Now.Year;
            int nextYear = DateTime.Now.AddYears(1).Year;

            Years = new ObservableCollection<int> { lastYear, currentYear, nextYear };

            Months = new ObservableCollection<string>
            {
                "Januar",
                "Februar",
                "Marts",
                "April",
                "Maj",
                "Juni",
                "Juli",
                "August",
                "September",
                "Oktober",
                "November",
                "December"
            };

            int yearMinusOneMonth = DateTime.Now.AddMonths(-1).Year;
            Year = yearMinusOneMonth;

            int lastMonth = DateTime.Now.Month - 1;
            MonthIndex = lastMonth - 1;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
