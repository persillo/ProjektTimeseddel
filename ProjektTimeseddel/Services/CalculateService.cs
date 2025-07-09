using ProjektTimeseddel.ViewModels;
using PublicHoliday;

namespace ProjektTimeseddel.Services
{
    public class CalculateService
    {
        public void CalculateWorkHrs(WorkDayViewModel workDayVM)
        {
            TimeSpan timeSpan = workDayVM.WorkEnd - workDayVM.WorkStart;

            // If WorkStart and WorkEnd are both 00:00 set WorkHrs to 24
            if (workDayVM.WorkStart.Hour == 0 &&
                workDayVM.WorkStart.Minute == 0 &&
                workDayVM.WorkEnd.Hour == 0 &&
                workDayVM.WorkEnd.Minute == 0)
            {
                timeSpan += TimeSpan.FromHours(24);
            }

            workDayVM.WorkHrs = timeSpan.TotalHours;
        }
        private double CalculateHoursInInterval(TimeOnly workStart, TimeOnly workEnd, TimeOnly intervalStart, TimeOnly intervalEnd)
        {
            DateTime startDate = DateTime.Today.Add(workStart.ToTimeSpan());
            DateTime endDate = workEnd > workStart
                ? DateTime.Today.Add(workEnd.ToTimeSpan())
                : DateTime.Today.AddDays(1).Add(workEnd.ToTimeSpan());

            DateTime intervalStartDt = DateTime.Today.Add(intervalStart.ToTimeSpan());
            DateTime intervalEndDt = intervalEnd > intervalStart
                ? DateTime.Today.Add(intervalEnd.ToTimeSpan())
                : DateTime.Today.AddDays(1).Add(intervalEnd.ToTimeSpan());

            DateTime overlapStart = startDate > intervalStartDt ? startDate : intervalStartDt;
            DateTime overlapEnd = endDate < intervalEndDt ? endDate : intervalEndDt;

            double overlap = (overlapEnd - overlapStart).TotalHours;
            return Math.Max(0, overlap);
        }
        public void CalculateManFre(WorkDayViewModel workDayVM)
        {
            if (workDayVM.Date.DayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Friday)
            {
                double manFreHours = CalculateHoursInInterval(
                    workDayVM.WorkStart,
                    workDayVM.WorkEnd,
                    new TimeOnly(17, 0),
                    new TimeOnly(6, 0));

                workDayVM.ManFreHrs = manFreHours;
            }
        }
        public void CalculateLør1117(WorkDayViewModel workDayVM)
        {
            if (workDayVM.Date.DayOfWeek is DayOfWeek.Saturday)
            {
                double lør1117Hrs = CalculateHoursInInterval(
                    workDayVM.WorkStart,
                    workDayVM.WorkEnd,
                    new TimeOnly(11, 0),
                    new TimeOnly(17, 0));

                workDayVM.Lør1117Hrs = lør1117Hrs;
            }
        }
        public void CalculateLør1724(WorkDayViewModel workDayVM)
        {
            if (workDayVM.Date.DayOfWeek is DayOfWeek.Saturday)
            {
                double lør1724Hrs = CalculateHoursInInterval(
                    workDayVM.WorkStart,
                    workDayVM.WorkEnd,
                    new TimeOnly(17, 0),
                    new TimeOnly(0, 0));

                workDayVM.Lør1724Hrs = lør1724Hrs;
            }
        }
        public void CalculateSønHel(WorkDayViewModel workDayVM)
        {
            int year = workDayVM.Date.Year;

            IList<DateTime> holidays = new DenmarkPublicHoliday(false, false).PublicHolidays(year);
            bool isHoliday = false;

            foreach (DateTime holiday in holidays)
            {
                if (holiday == workDayVM.Date)
                {
                    isHoliday = true;
                    break;
                }
            }

            if (workDayVM.Date.DayOfWeek is DayOfWeek.Sunday || isHoliday == true)
            {
                double sønHelHrs = CalculateHoursInInterval(
                    workDayVM.WorkStart,
                    workDayVM.WorkEnd,
                    new TimeOnly(0, 0),
                    new TimeOnly(0, 0));

                workDayVM.SønHelHrs = sønHelHrs;
            }
        }
        public void CalculateMan0006(WorkDayViewModel workDayVM)
        {
            if (workDayVM.Date.DayOfWeek is DayOfWeek.Monday)
            {
                double man0006Hrs = CalculateHoursInInterval(
                    workDayVM.WorkStart,
                    workDayVM.WorkEnd,
                    new TimeOnly(0, 0),
                    new TimeOnly(6, 0));

                workDayVM.Man0006Hrs = man0006Hrs;
            }
        }
        public double CalculateTotalHrs(IEnumerable<WorkDayViewModel> workDayVMs)
        {
            double totalHrs = 0;
            foreach (WorkDayViewModel workDayVM in workDayVMs)
            {
                totalHrs += workDayVM.WorkHrs;
            }
            return totalHrs;
        }
        public List<double> CalculateTotalIntervalHrs(IEnumerable<WorkDayViewModel> workDayVMs)
        {
            List<double> intervalHrs = [];

            double totalManFreHrs = 0;
            foreach (WorkDayViewModel workDayVM in workDayVMs)
            {
                totalManFreHrs += workDayVM.ManFreHrs;
            }
            intervalHrs.Add(totalManFreHrs);

            double totalLør1117Hrs = 0;
            foreach (WorkDayViewModel workDayVM in workDayVMs)
            {
                totalLør1117Hrs += workDayVM.Lør1117Hrs;
            }
            intervalHrs.Add(totalLør1117Hrs);

            double totalLør1724Hrs = 0;
            foreach (WorkDayViewModel workDayVM in workDayVMs)
            {
                totalLør1724Hrs += workDayVM.Lør1724Hrs;
            }
            intervalHrs.Add(totalLør1724Hrs);

            double totalSønHelHrs = 0;
            foreach (WorkDayViewModel workDayVM in workDayVMs)
            {
                totalSønHelHrs += workDayVM.SønHelHrs;
            }
            intervalHrs.Add(totalSønHelHrs);

            double totalMan0006Hrs = 0;
            foreach (WorkDayViewModel workDayVM in workDayVMs)
            {
                totalMan0006Hrs += workDayVM.Man0006Hrs;
            }
            intervalHrs.Add(totalMan0006Hrs);

            return intervalHrs;
        }
        public void PerformAllWorkDayCalculations(WorkDayViewModel workDayVM)
        {
            CalculateWorkHrs(workDayVM);
            CalculateManFre(workDayVM);
            CalculateLør1117(workDayVM);
            CalculateLør1724(workDayVM);
            CalculateSønHel(workDayVM);
            CalculateMan0006(workDayVM);
        }
    }
}
