namespace EmployesWork.WpfUI
{
    public class HelpClass
    {
        public enum monthes : int
        {
            Январь = 1,
            Февраль = 2,
            Март = 3,
            Апрель = 4,
            Май = 5,
            Июнь = 6,
            Июль = 7,
            Август = 8,
            Сентябрь = 9,
            Октябрь = 10,
            Ноябрь = 11,
            Декабрь = 12
        };


        public string NameOfMonth(int month) => ((monthes)month).ToString();
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

    }
}