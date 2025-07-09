using System.ComponentModel;
using System.Globalization;

namespace ProjektTimeseddel.ViewModels
{
    public class WorkDayViewModel : INotifyPropertyChanged
    {
        private DateTime date;
        private string stringWorkStart = "00:00";
        private string stringWorkEnd = "00:00";
        private double workHrs;
        private double manFreHrs;
        private double lør1117Hrs;
        private double lør1724Hrs;
        private double sønHelHrs;
        private double man0006Hrs;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTime Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged(nameof(date)); }
        }
        public string StringDate
        {
            get { return $"{Date.Day.ToString()}/{Date.Month.ToString()}"; }
        }
        public string StringWorkStart
        {
            get { return stringWorkStart; }
            set { stringWorkStart = value; OnPropertyChanged(nameof(StringWorkStart)); }
        }
        public string StringWorkEnd
        {
            get { return stringWorkEnd; }
            set { stringWorkEnd = value; OnPropertyChanged(nameof(StringWorkEnd)); }
        }
        public TimeOnly WorkStart
        {
            get { return TimeOnly.ParseExact(stringWorkStart, "t", CultureInfo.CreateSpecificCulture("da-DK")); }
        }
        public TimeOnly WorkEnd
        {
            get { return TimeOnly.ParseExact(stringWorkEnd, "t", CultureInfo.CreateSpecificCulture("da-DK")); }
        }
        public double WorkHrs
        {
            get { return workHrs; }
            set { workHrs = value; OnPropertyChanged(nameof(WorkHrs)); }
        }
        public double ManFreHrs
        {
            get { return manFreHrs; }
            set { manFreHrs = value; OnPropertyChanged(nameof(ManFreHrs)); }
        }
        public double Lør1117Hrs
        {
            get { return lør1117Hrs; }
            set { lør1117Hrs = value; OnPropertyChanged(nameof(lør1117Hrs)); }
        }
        public double Lør1724Hrs
        {
            get { return lør1724Hrs; }
            set { lør1724Hrs = value; OnPropertyChanged(nameof(lør1724Hrs)); }
        }
        public double SønHelHrs
        {
            get { return sønHelHrs; }
            set { sønHelHrs = value; OnPropertyChanged(nameof(sønHelHrs)); }
        }
        public double Man0006Hrs
        {
            get { return man0006Hrs; }
            set { man0006Hrs = value; OnPropertyChanged(nameof(man0006Hrs)); }
        }
        public WorkDayViewModel(DateTime date)
        {
            this.date = date;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
