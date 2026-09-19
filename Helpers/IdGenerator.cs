using System;
using System.Text;

namespace DarkContinent.Helpers
{
    public static class IdGenerator
    {
        public static int GenerateRandomId()
        {
            string str = "0123456789";
            Random ran = new Random();
            StringBuilder temp = new StringBuilder();

            temp.Append(str[ran.Next(1, str.Length)]);

            for (int i = 0; i < 8; i++)
            {
                temp.Append(str[ran.Next(0, str.Length)]);
            }

            return int.Parse(temp.ToString());
        }
    }
}
