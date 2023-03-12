using System.Linq;
using System;
using GemBox;
using GemBox.Spreadsheet;
using ConsoleApplication1;

class Program
{
    static void Main(string[] args)
    {
        new Program().ExcelCreator(DateTime.Now, true);
    }
    UAM_TABLE_DBEntities db = new UAM_TABLE_DBEntities();
    public void ExcelCreator(DateTime date, bool all)
    {


        string name = NameOfMonth(date.Month) + ", " + date.Year;
        string filename = "Табель выходов УАМ ТЭЦ-ПВС за " + name;
        int countDay = DateTime.DaysInMonth(date.Year, date.Month);
        Employe[] employes = db.Employes.ToArray();
        WorkDay[] workDays;
        if (all) workDays = db.WorkDays.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month).ToArray();
        else workDays = db.WorkDays.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Month && w.Date <= date).ToArray();
        var noexistsLit = workDays.Where(x => x.ShedulesId == 6).Select(x => x.Literal).Distinct().ToArray();
        Shedule[] shedules = db.Shedules.ToArray();



        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
        ExcelFile file = ExcelFile.Load(countDay + ".xls");
        
        ExcelWorksheet sheet = file.Worksheets.ActiveWorksheet;
        sheet.Name = name;

        var range = sheet.Cells[0, 0];
        range.Value = "Табель выходов УАМ ТЭЦ-ПВС за " + name;
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
            ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
            ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
            col++;
        }





        row = 4;
        col = 0;

        foreach (var employe in employes)
        {
            #region employes
            var shedsem = workDays.Where(w => w.EmployeId == employe.Id && w.ShedulesId != 6).Select(s => s.Shedule).Distinct().ToArray();
            var countsh = shedsem.Length;
            countsh = countsh < 1 ? 1 : countsh;


            ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
            if (countsh > 1) ranges.Merged = true;
            ranges.Value = employe.PersonnelId;
            ranges.Style.Font.Weight = 900;
            ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
            ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
            col++;

            ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
            if (countsh > 1) ranges.Merged = true;
            ranges.Value = employe.Name;
            ranges.Style.Font.Weight = 900;
            ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
            ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
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
                else literal =  wd.Literal;
                ranges.Value = literal;
                if (!GetExit(literal)) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Red);
                else if(wd.ShedulesId == 1) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Blue);
                else if (wd.ShedulesId == 9) ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.DarkBlue);
                else ranges.Style.Font.Color = SpreadsheetColor.FromName(ColorName.Black);
                if (!all && d.Year == DateTime.Now.Year && d.Month == DateTime.Now.Month && d.Day == DateTime.Now.Day)
                    ranges.Style.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Yellow));
                    ranges.Style.Font.Weight = 900;
                ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
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
                range.Value = shed.Brig + " Гр." + shed.Sheduler;
                range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                col++;

                range = sheet.Cells[row, col];
                var employerexists = itemdays.Where(s => s.ShedulesId == shed.Id).Count();
                allExist += employerexists;
                range.Value = employerexists == 0 ? "" : employerexists.ToString();
                range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);

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
                range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                col++;

                var ehour = itemdays.Where(s => s.ShedulesId == shed.Id).Select((s) =>
                {
                    if (s.Literal == "Н") return 5;
                    else return 0;
                }).Sum();
                range = sheet.Cells[row, col];
                range.Value = ehour == 0 ? "" : ehour.ToString();
                range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                col++;

                var nhour = itemdays.Where(s => s.ShedulesId == shed.Id).Select((s) =>
                {
                    if (s.Literal == "Н") return 7;
                    else return 0;
                }).Sum();
                range = sheet.Cells[row, col];
                range.Value = nhour == 0 ? "" : nhour.ToString();
                range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                col++;

                var employesHours = nhour + ehour + dhour;
                allhour += employesHours;
                range = sheet.Cells[row, col];
                range.Value = employesHours == 0 ? "" : employesHours.ToString();
                range.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                range.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                range.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
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
            ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
            ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
            col++;

            ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
            if (countsh > 1) ranges.Merged = true;
            ranges.Value = allhour == 0 ? "" : allhour.ToString();
            ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
            ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
            col++;


            #endregion

                for (int i = 0; i < noexistsLit.Count(); i++)
                {
                    ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, countsh);
                    if (countsh > 1) ranges.Merged = true;
                    var c = workDays.Where(w => w.EmployeId == employe.Id).Where(w => w.Literal == noexistsLit[i]).Count();
                    ranges.Value = c == 0 ? "" : c.ToString();
                    ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
                    ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Thin);
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
        ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
        ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
        ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
        ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
        col +=2;

        for (int i = 1; i <= countDay; i++)
        {
            DateTime d = new DateTime(date.Year, date.Month, i);
            ranges = sheet.Cells.GetSubrangeRelative(row, col, 1, 1);
            int exdays = workDays.Where(w =>  w.Date == d &&  w.ShedulesId != 6).Count();
            ranges.Value = exdays;
            if (!all && d.Year == DateTime.Now.Year && d.Month == DateTime.Now.Month && d.Day == DateTime.Now.Day)
                ranges.Style.FillPattern.SetSolid(SpreadsheetColor.FromName(ColorName.Yellow));
            ranges.Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ranges.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ranges.Style.Borders.SetBorders(MultipleBorders.Vertical, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
            ranges.Style.Borders.SetBorders(MultipleBorders.Horizontal, SpreadsheetColor.FromArgb(0, 0, 0), LineStyle.Medium);
            col++;
        }

        col = 0;
        row+=2;



        file.Save(@"C:\Users\DonCondor\Desktop\" + filename + ".xls");


    }

    public bool GetExit(string str)
    {
        double d;
        try
        {
            return double.TryParse(str, out d) || double.TryParse(str.Replace(".", ","), out d) || double.TryParse(str.Replace(",", "."), out d) || str == "Д" || str == "Н";

        }
        catch
        {

            return false;
        }
    }

    public string NameOfMonth(int month)
    {
        switch (month)
        {
            case 1: return "Январь";
            case 2: return "Февраль";
            case 3: return "Март";
            case 4: return "Апрель";
            case 5: return "Май";
            case 6: return "Июнь";
            case 7: return "Июль";
            case 8: return "Август";
            case 9: return "Сентябрь";
            case 10: return "Октябрь";
            case 11: return "Ноябрь";
            case 12: return "Декабрь";
        }
        return "";
    }
}