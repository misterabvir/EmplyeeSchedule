using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace EmployesWork.WpfUI
{
    public class PathesKeeper : IEnumerable
    {
        List<string> pathes = new List<string>();
        public int Count => pathes.Count;

        public bool ReadPathes()
        {
            pathes.Clear();
            try
            {
                using (FileStream fs = new FileStream(CN.PATH_FILENAME, FileMode.OpenOrCreate))
                {
                    StreamReader sr = new StreamReader(fs);
                    while (!sr.EndOfStream)
                    {
                        pathes.Add(sr.ReadLine());
                    }
                    sr.Close();
                }
                if (pathes.Count == 0)
                    SetDefault();                 
                return true;
            }
            catch
            {              
                SetDefault();
                return false;
            }
        }

        private void SetDefault()
        {
            pathes.Clear();
            //if (Directory.Exists(CN.PATH_DEFAULT_PATH_LOCAL_NETWORK))
                pathes.Add(CN.PATH_DEFAULT_PATH_LOCAL_NETWORK);
            //if (Directory.Exists(CN.PATH_DEFAULT_PATH_DESKTOP))
                pathes.Add(CN.PATH_DEFAULT_PATH_DESKTOP);
            pathes.Add(@"C:\");
        }

        public bool AddString(string newPath)
        {
            if (Directory.Exists(newPath))
            {
                pathes.Add(newPath);
                return true;
            }
            return false;
        }

        public bool RemoveString(string removePath)
        {
            if (pathes.Count > 1)
            {
                bool isRemoved = pathes.Remove(removePath);
                return isRemoved;
            }
            return false;
        }

        public bool SavePathes()
        {
            try
            {
                using (FileStream fs = new FileStream(CN.PATH_FILENAME, FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    foreach (string path in pathes)
                    {
                        sw.WriteLine(path);
                    }
                    sw.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return pathes.GetEnumerator();
        }
    }
}
