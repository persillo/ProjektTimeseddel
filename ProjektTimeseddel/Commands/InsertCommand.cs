using ProjektTimeseddel.Services;
using ProjektTimeseddel.ViewModels;
using System.Windows.Input;

namespace ProjektTimeseddel.Commands
{
    public class InsertCommand : ICommand
    {
        private WordService wordService;

        public event EventHandler? CanExecuteChanged;
        public InsertCommand(MainViewModel mainVM)
        {
            this.wordService = new WordService(mainVM);
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            wordService.CreateFile();
            wordService.InsertDataIntoFile();
        }
    }
}
