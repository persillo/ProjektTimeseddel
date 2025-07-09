using ProjektTimeseddel.Services;
using ProjektTimeseddel.ViewModels;
using System.Windows.Input;

namespace ProjektTimeseddel.Commands
{
    public class CalculateCommand : ICommand
    {
        private CalculateService calcService;
        private MainViewModel mainVM;
        public event EventHandler? CanExecuteChanged;

        public CalculateCommand(MainViewModel mainVM)
        {
            calcService = new CalculateService();
            this.mainVM = mainVM;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            calcService.PerformAllWorkDayCalculations(mainVM.WorkDayVMs.Last());

            mainVM.TotalWorkHrs = calcService.CalculateTotalHrs(mainVM.WorkDayVMs);
            List<double> totalIntervalHrs = calcService.CalculateTotalIntervalHrs(mainVM.WorkDayVMs);
            mainVM.TotalManFreHrs = totalIntervalHrs[0];
            mainVM.TotalLør1117Hrs = totalIntervalHrs[1];
            mainVM.TotalLør1724Hrs = totalIntervalHrs[2];
            mainVM.TotalSønHelHrs = totalIntervalHrs[3];
            mainVM.TotalMan0006Hrs = totalIntervalHrs[4];
        }
    }
}
