using System;
using System.Linq;
using GemBox.Spreadsheet;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EmployesWork.WpfUI
{
    internal class CreatorRaport
    {
        private UAM_TABLE_DBEntities db = new UAM_TABLE_DBEntities();

        private bool isFull;
        private DateTime date;
        private string CurrentPath { get; set; }
        


        public CreatorRaport( bool isFull, DateTime date, string path)
        {
            this.date = date;
            this.isFull = isFull;
            this.date = date;
            CurrentPath = path;
        }

        public async Task Start(Button btn)
        {
            AsyncFactory fac = new AsyncFactory() { TaskLink = Creator };
            await fac.StartTask();
            btn.IsEnabled = true;
        }

        private void Creator()
        {
            try
            {
                string name = new HelpClass().NameOfMonth(date.Month) + ", " + date.Year;
                string filename = CN.CREATE_TITLE_OF_TABLE + name;
                int countDay = DateTime.DaysInMonth(date.Year, date.Month);
                ExcelFile file = Creating(name, countDay);
                var resultPath = SavingFile(filename, file);
                MessageBox.Show($"{CN.CREATE_SAVE_SUCCES}\n{resultPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{CN.CREATE_ERROR}\n{ex.Message}");                
            }
        }

        private string  SavingFile(string filename, ExcelFile file)
        {
            string strpath = "";
            strpath = CurrentPath;// + (new HelpClass().NameOfMonth(date.Month)) + "\\";
            if (!Directory.Exists(strpath))
                Directory.CreateDirectory(strpath);
            if (isFull)
            {               
                strpath += filename + CN.CREATE_FULL;
            }
            strpath += filename + CN.CREATE_FILE_EXTENSION;               
            file.Save(strpath);
            return strpath;
        }

        private ExcelFile Creating(string name, int countDay)
        {
            Employe[] employes = db.Employes.Where(em => em.Show == true).OrderBy(o => o.Description).ToArray();
            WorkDay[] workDays;
            if (isFull) workDays = db.WorkDays.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month).ToArray();
            else workDays = db.WorkDays.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month && w.Date <= DateTime.Now).ToArray();
            var noexistsLit = workDays.Where(x => x.ShedulesId == 6).Select(x => x.Literal).Distinct().ToArray();
            Shedule[] shedules = db.Shedules.ToArray();
            #region creating
            SpreadsheetInfo.SetLicense(CN.CREATE_LICENSE_KEY);
            ExcelFile file = ExcelFile.Load(countDay + CN.CREATE_FILE_EXTENSION);
            ExcelWorksheet sheet = file.Worksheets.ActiveWorksheet;
            sheet.Name = name;
            var range = sheet.Cells[0, 0];
            range.Value = $"{CN.CREATE_TITLE_OF_TABLE} {name}";
            var ranges = sheet.Cells.GetSubrangeAbsolute(2, 2, 2, 2 + countDay);
            int count = 1;
            foreach (var item in ranges)
            {
                if (count > countDay) break;
                DateTime d = new DateTime(date.Year, date.Month, count);
                if (d.DayOfWeek == DayOfWeek.Sunday || d.DayOfWeek == DayOfWeek.Saturday)
                {
                    item.Style.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Red));
                    item.Style.Font.Color = SpreadsheetColor.FromName(ColorName.White);
                }
                item.Style.Font.Weight = 900;
                count++;
            }
            int row = 1;
            int col = 10 + countDay;
            foreach (var item in noexistsLit)
            {
                ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, 2);
                ranges.Merged = true;
                ranges.Value = item;
                SetStyle_Medium_Center_Center(ranges);
                col++;
            }

            row = 4;
            col = 0;

            foreach (var employe in employes)
            {
                if (employe.Show != true) continue;
                #region employes
                var shedsem = workDays.Where(w => w.EmployeId == employe.Id && w.ShedulesId != 6).Select(s => s.Shedule).Distinct().ToArray();
                var countsh = shedsem.Length;
                countsh = countsh < 1 ? 1 : countsh;
                ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                if (countsh > 1) ranges.Merged = true;
                ranges.Value = employe.PersonnelId;
                ranges.Style.Font.Weight = 900;
                SetStyle_Thin_Center_Left(ranges);
                col++;
                ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                if (countsh > 1) ranges.Merged = true;
                ranges.Value = employe.Name;
                ranges.Style.Font.Weight = 900;
                SetStyle_Thin_Center_Left(ranges);
                if (employe.ING.GetValueOrDefault()) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Red);
                col++;
                #endregion
                #region literals
                for (int i = 1; i <= countDay; i++)
                {
                    DateTime d = new DateTime(date.Year, date.Month, i);
                    ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                    if (countsh > 1) ranges.Merged = true;
                    WorkDay wd = workDays.Where(w => w.EmployeId == employe.Id && w.Date == d).FirstOrDefault();
                    string literal;
                    if (wd == null) literal = "";
                    else literal = wd.Literal;
                    ranges.Value = literal;
                    if (!new HelpClass().GetExit(literal)) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Red);
                    else if (wd.ShedulesId == 1) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Blue);
                    else if (wd.ShedulesId == 9) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.DarkBlue);
                    else ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Black);
                    if (!isFull && d.Year == DateTime.Now.Year && d.Month == DateTime.Now.Month && d.Day == DateTime.Now.Day)
                        ranges.Style.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Yellow));
                    ranges.Style.Font.Weight = 900;
                    SetStyle_Thin_Center_Center(ranges);
                    col++;
                }
                #endregion
                #region itogs in shedules
                var itemdays = workDays.Where(w => w.EmployeId == employe.Id).Where(s => s.Date.Month == date.Date.Month && s.Date.Year == date.Year && s.ShedulesId != 6);
                var allhour = 0d;
                var allExist = 0d;

                var column = col;
                foreach (var shed in itemdays.Select(d => d.Shedule).Distinct())
                {
                    range = sheet.Cells[row, col];
                    range.Value = shed.Brig + " " + shed.Sheduler;
                    SetStyle_Thin_Center_Center(range);
                    col++;

                    range = sheet.Cells[row, col];
                    var employerexists = itemdays.Where(s => s.ShedulesId == shed.Id).Count();
                    allExist += employerexists;
                    range.Value = employerexists == 0 ? "" : employerexists.ToString();
                    SetStyle_Thin_Center_Center(range);
                    col++;

                    var dhour = itemdays.Where(s => s.ShedulesId == shed.Id).Select((s) =>
                    {
                        double hour;
                        if (double.TryParse(s.Literal, out hour) || double.TryParse(s.Literal.Replace(".", ","), out hour) || double.TryParse(s.Literal.Replace(",", "."), out hour)) return hour;
                        else if (s.Literal == "Д") return 11;
                        else if (s.Literal == "Н") return 1;
                        else return 0;
                    }).Sum();
                    range = sheet.Cells[row, col];
                    range.Value = dhour == 0 ? "" : dhour.ToString();
                    SetStyle_Thin_Center_Center(range);
                    col++;

                    var ehour = itemdays.Where(s => s.ShedulesId == shed.Id).Select((s) =>
                    {
                        if (s.Literal == "Н") return 5;
                        else return 0;
                    }).Sum();
                    range = sheet.Cells[row, col];
                    range.Value = ehour == 0 ? "" : ehour.ToString();
                    SetStyle_Thin_Center_Center(range);
                    col++;

                    var nhour = itemdays.Where(s => s.ShedulesId == shed.Id).Select((s) =>
                    {
                        if (s.Literal == "Н") return 7;
                        else return 0;
                    }).Sum();
                    range = sheet.Cells[row, col];
                    range.Value = "";//nhour == 0 ? "" : nhour.ToString();
                    SetStyle_Thin_Center_Center(range);
                    col++;

                    var employesHours = nhour + ehour + dhour;
                    allhour += employesHours;
                    range = sheet.Cells[row, col];
                    range.Value = employesHours == 0 ? "" : employesHours.ToString();
                    SetStyle_Thin_Center_Center(range);
                    col++;


                    col = column;
                    if (countsh > 1) row++;
                }
                #endregion
                #region itogslast
                if (countsh > 1)
                    row -= countsh;
                col += 6;
                ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                if (countsh > 1) ranges.Merged = true;
                ranges.Value = allExist == 0 ? "" : allExist.ToString();
                SetStyle_Thin_Center_Center(ranges);
                col++;

                ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                if (countsh > 1) ranges.Merged = true;
                ranges.Value = allhour == 0 ? "" : allhour.ToString();
                SetStyle_Thin_Center_Center(ranges);
                col++;


                #endregion

                for (int i = 0; i < noexistsLit.Count(); i++)
                {
                    ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                    if (countsh > 1) ranges.Merged = true;
                    var c = workDays.Where(w => w.EmployeId == employe.Id).Where(w => w.Literal == noexistsLit[i]).Count();
                    ranges.Value = c == 0 ? "" : c.ToString();
                    SetStyle_Thin_Center_Center(ranges);
                    col++;


                }
                row += countsh == 1 ? 1 : countsh;
                ExcelRow r = sheet.Rows[row];
                r.Height = 100;
                row++;
                col = 0;

            }

            ranges = sheet.Cells.GetSubrangeRelative(row, col, 2, 1);
            ranges.Merged = true;
            ranges.Value = "Итого за сутки отработало";
            SetStyle_Medium_Center_Left(ranges);
            col += 2;

            for (int i = 1; i <= countDay; i++)
            {
                DateTime d = new DateTime(date.Year, date.Month, i);
                ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, 1);
                int exdays = workDays.Where(w => w.Date == d && w.ShedulesId != 6 && w.ShedulesId != 13).Count();
                ranges.Value = exdays;
                if (!isFull && d.Year == DateTime.Now.Year && d.Month == DateTime.Now.Month && d.Day == DateTime.Now.Day)
                    ranges.Style.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Yellow));
                SetStyle_Medium_Center_Center(ranges);
                col++;
            }

            col = 0;
            row += 2;

            #endregion
            return file;
        }

        private static void SetStyle_Medium_Center_Left(CellRange ranges)
        {
            ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
            ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
        }

        private static void SetStyle_Medium_Center_Center(CellRange ranges)
        {
            ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
            ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
        }

        private void SetStyle_Thin_Center_Center(CellRange range)
        {
            range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
            range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
        }
        private void SetStyle_Thin_Center_Center(ExcelCell range)
        {
            range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
            range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
        }
        private void SetStyle_Thin_Center_Left(CellRange range)
        {
            range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
            range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
        }

        public class AsyncFactory
        {
            public Action TaskLink { get; set; }

            async public Task StartTask()
            {
                await Task.Run(() => TaskLink());
            }
        }
    }
}