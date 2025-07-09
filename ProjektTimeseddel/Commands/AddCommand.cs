using ProjektTimeseddel.ViewModels;
using System.Windows.Input;

namespace ProjektTimeseddel.Commands
{
    public class AddCommand : ICommand
    {
        private MainViewModel mainVM;
        public event EventHandler? CanExecuteChanged;

        public AddCommand(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            // Create new WorkDayViewModel with selected date and add to WorkDayVMs in mainVM
            DateTime date = new DateTime(mainVM.Year, mainVM.Month, mainVM.Date);
            mainVM.WorkDayVMs.Add(new WorkDayViewModel(date));

            // Update Days so it starts from the day after the previously selected date
            mainVM.Days = [.. mainVM.Days.TakeLast(DateTime.DaysInMonth(mainVM.Year, mainVM.Month) - Int32.Parse(mainVM.Date.ToString("d")))];

            // Check if there are more days left in the month
            // If yes, set to next day, if no, set to zero
            mainVM.Date = mainVM.Days.Count > 0 ? mainVM.Days.First() : 0;
        }
    }
}
