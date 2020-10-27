using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace poker.exe._2._0
{
    static class Aux
    {
        public static int ToInt(string str)
        {
            int tempInt = 0;
            char[] CharStr = new char[str.Length];
            CharStr = str.ToArray();
            for (int i = 0; i < str.Length; i++)
            {
                if (CharStr[i] - 48 > 9 || CharStr[i] - 48 < 0)
                    return -999;
                tempInt += CharStr[i] - 48;
                tempInt *= 10;
            }
            tempInt /= 10;
            return tempInt;            
        }                          
    }
}
