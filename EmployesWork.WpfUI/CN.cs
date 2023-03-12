using System.Windows.Controls;

namespace EmployesWork.WpfUI
{
    public static class CN
    {
        static public string CREATE_TITLE_OF_TABLE => "Табель выходов УАМ ТЭЦ-ПВС за ";
        static public string CREATE_SAVE_SUCCES => "Табель успешно сохранен по пути: ";
        static public string CREATE_ERROR => "Произошла ошибка:";
        static public string CREATE_FILE_EXTENSION => ".xls";
        static public string CREATE_FULL => "(ПОЛНЫЙ)";
        static public string CREATE_LICENSE_KEY => "FREE-LIMITED-KEY";


        static public string PATH_FILENAME => "pathes.inf";
        static public string PATH_PATHWORD => "Путь ";
        static public string PATH_ADDED => "успешно добавлен.";
        static public string PATH_NOT_ADDED => "не был добавлен, проверьте корректность задаваемого пути.";
        static public string PATH_REMOVED => "успешно удален.";
        static public string PATH_NOT_REMOVED => "не был удален.";
        static public string PATH_DEFAULT_PATH_DESKTOP => @"C:\Users\КИП\Desktop\";
        static public string PATH_DEFAULT_PATH_LOCAL_NETWORK => @"\\192.168.2.222\work\Отделы\УАМ\Входящие\Для нормировщика\";

        public static string MAIN_TITLE => "Конвертер табеля в Excel файл";
        public static string MAIN_BTN_START => "Экспортировать табель в EXCEL";

        public static string MAIN_IS_FULL => "Полный табель";
        public static string MAIN_IS_NOT_FULL => "Текущий табель";
        public static string MAIN_PATH_LABEL => "Пути:";
        public static string MAIN_BTN_REMOVE => "Удалить";
        public static string MAIN_BTN_ADD => "Добавить";
    }
}
