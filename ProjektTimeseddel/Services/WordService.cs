using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProjektTimeseddel.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace ProjektTimeseddel.Services
{
    public class WordService
    {
        private string filePath;
        private MainViewModel mainVM;
        private ObservableCollection<WorkDayViewModel> workDayVMs;
        public WordService(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
            workDayVMs = mainVM.WorkDayVMs;
        }
        public void InsertDataIntoFile()
        {
            InsertWorkTimes();
            InsertWorkHrs();
            InsertTotalHrs();
            InsertDateOfSignature();
        }
        public void CreateFile()
        {
            string templatePath;

            switch (DateTime.DaysInMonth(mainVM.Year, mainVM.Month))
            {
                case 31:
                    templatePath = @"C:\Users\perni\source\repos\ProjektTimeseddel\ProjektTimeseddel\Resources\ProjektTimeseddelSkabelon31dage.docx";
                    break;
                case 30:
                    templatePath = @"C:\Users\perni\source\repos\ProjektTimeseddel\ProjektTimeseddel\Resources\ProjektTimeseddelSkabelon30dage.docx";
                    break;
                case 28:
                    templatePath = @"C:\Users\perni\source\repos\ProjektTimeseddel\ProjektTimeseddel\Resources\ProjektTimeseddelSkabelon28dage.docx";
                    break;
                case 29:
                    templatePath = @"C:\Users\perni\source\repos\ProjektTimeseddel\ProjektTimeseddel\Resources\ProjektTimeseddelSkabelon29dage.docx";
                    break;
                default:
                    templatePath = @"C:\Users\perni\source\repos\ProjektTimeseddel\ProjektTimeseddel\Resources\ProjektTimeseddelSkabelon31dage.docx";
                    break;
            }

            DateTime workMonthAndYear = new DateTime(mainVM.Year, mainVM.Month, 1);
            string monthString = workMonthAndYear.ToString("MMMM", CultureInfo.CreateSpecificCulture("da-DK"));
            filePath = $@"C:\Users\perni\Documents\Mathias\{monthString}{workMonthAndYear.ToString("yyyy")}Timeseddel§84.docx";

            File.Copy(templatePath, filePath, overwrite: true);

            using (WordprocessingDocument.CreateFromTemplate(filePath)) { }

            // Insert month and year
            string tableTitle = workMonthAndYear.ToString("Y", CultureInfo.CreateSpecificCulture("da-DK"));
            AddTextToCell($"{tableTitle[0].ToString().ToUpper()}{tableTitle.Substring(1)}", 0, 1);
        }
        private void InsertWorkTimes()
        {
            foreach (WorkDayViewModel workDayVM in workDayVMs)
            {
                AddTextToCell(workDayVM.StringWorkStart, workDayVM.Date.Day + 1, 1);
                AddTextToCell(workDayVM.StringWorkEnd, workDayVM.Date.Day + 1, 2);
            }
        }
        private void InsertWorkHrs()
        {
            foreach (WorkDayViewModel workDayVM in workDayVMs)
            {
                if (workDayVM.WorkHrs > 0) AddTextToCell(workDayVM.WorkHrs.ToString(CultureInfo.CreateSpecificCulture("en-US")), workDayVM.Date.Day + 1, 3);
                if (workDayVM.ManFreHrs > 0) AddTextToCell(workDayVM.ManFreHrs.ToString(CultureInfo.CreateSpecificCulture("en-US")), workDayVM.Date.Day + 1, 4);
                if (workDayVM.Lør1117Hrs > 0) AddTextToCell(workDayVM.Lør1117Hrs.ToString(CultureInfo.CreateSpecificCulture("en-US")), workDayVM.Date.Day + 1, 5);
                if (workDayVM.Lør1724Hrs > 0) AddTextToCell(workDayVM.Lør1724Hrs.ToString(CultureInfo.CreateSpecificCulture("en-US")), workDayVM.Date.Day + 1, 6);
                if (workDayVM.SønHelHrs > 0) AddTextToCell(workDayVM.SønHelHrs.ToString(CultureInfo.CreateSpecificCulture("en-US")), workDayVM.Date.Day + 1, 7);
                if (workDayVM.Man0006Hrs > 0) AddTextToCell(workDayVM.Man0006Hrs.ToString(CultureInfo.CreateSpecificCulture("en-US")), workDayVM.Date.Day + 1, 8);
            }
        }
        private void InsertTotalHrs()
        {
            int daysInMonth = DateTime.DaysInMonth(mainVM.Year, mainVM.Month);

            for (int i = 0; i < mainVM.TotalHrList.Count; i++)
            {
                if (mainVM.TotalHrList[i] > 0) AddTextToCell(mainVM.TotalHrList[i].ToString(CultureInfo.CreateSpecificCulture("en-US")), daysInMonth + 3, i + 1);
            }
        }
        private void InsertDateOfSignature()
        {
            AddTextToCell($"{DateTime.Now.Day.ToString()}/{DateTime.Now.Month.ToString()}/{DateTime.Now.Year.ToString()}", 1, 1, 0, 0);
        }
        private void AddTextToCell(string txt, int rowIndex, int cellIndex)
        {
            // Use the file name and path passed in as an argument to 
            // open an existing document.
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                if (doc.MainDocumentPart is null || doc.MainDocumentPart.Document.Body is null)
                {
                    throw new ArgumentNullException("MainDocumentPart and/or Body is null.");
                }

                // Find the first table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().First();

                // Find the specified row in the table.
                TableRow row = table.Elements<TableRow>().ElementAt(rowIndex);

                // Find the specified cell in the row.
                TableCell cell = row.Elements<TableCell>().ElementAt(cellIndex);
                // Find the first paragraph in the table cell.
                Paragraph p = cell.Elements<Paragraph>().First();

                // Add run to paragraph
                Run r = new Run();
                p.Append(r);

                // Set font size for run
                FontSize fontSize = new FontSize() { Val = "20" };
                RunProperties rPr = new RunProperties()
                {
                    FontSize = fontSize,
                };
                r.AppendChild(rPr);

                // Add text to run
                Text t = new Text(txt);
                r.AppendChild(t);
            }
        }
        private void AddTextToCell(string txt, int tableIndex, int paragraphIndex, int rowIndex, int cellIndex)
        {
            // Use the file name and path passed in as an argument to 
            // open an existing document.
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                if (doc.MainDocumentPart is null || doc.MainDocumentPart.Document.Body is null)
                {
                    throw new ArgumentNullException("MainDocumentPart and/or Body is null.");
                }

                // Find the specified table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableIndex);

                // Find the specified row in the table.
                TableRow row = table.Elements<TableRow>().ElementAt(rowIndex);

                // Find the specified cell in the row.
                TableCell cell = row.Elements<TableCell>().ElementAt(cellIndex);
                // Find the specified paragraph in the table cell.
                Paragraph p = cell.Elements<Paragraph>().ElementAt(paragraphIndex);

                // Add run to paragraph
                Run r = new Run();
                p.Append(r);

                // Set font size for run
                FontSize fontSize = new FontSize() { Val = "20" };
                RunProperties rPr = new RunProperties()
                {
                    FontSize = fontSize,
                };
                r.AppendChild(rPr);

                // Add text to run
                Text t = new Text(txt);
                r.AppendChild(t);
            }
        }
    }
}
