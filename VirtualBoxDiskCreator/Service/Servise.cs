namespace VirtualBoxDiskCreator.Utility
{
    static class Service
    {
        public static bool IsDigit(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        public static int GetNumberFromString(string number)
        {
            int index = number.Length;
            while (index > 0 && IsDigit(number[index - 1]))
            {
                index--;
            }

            int.TryParse(number.Substring(index), out int result);
            return result;
        }

        public static string GetVirtualBoxDirectory()
        {
            string path1 = @"C:\Program Files\Oracle\VirtualBox\VBoxManage.exe";
            string path2 = @"C:\Program Files (x86)\Oracle\VirtualBox\VBoxManage.exe";

            if (System.IO.File.Exists(path1))
            {
                return path1;
            }
            else if (System.IO.File.Exists(path2))
            {
                return path2;
            }
            else
            {
                return null;
            }
        }
    }
}