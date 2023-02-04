using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi_Library
{
    public class WorkProcess
    {
        public void Load(int num = 0)
        {
            string url = "";

            if (num > 0)
            {
                url = $"https://xkcd.com/{num}/info.0.json ";
            }
            else
            {
                url = $"https://xkcd.com/info.0.json";
            }
        }
    }
}
