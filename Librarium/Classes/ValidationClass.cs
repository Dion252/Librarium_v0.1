using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarium.Classes
{
    class ValidationClass
    {
        public static bool ValidationDateCredit(DateTime startdate, double price)
        {
            if (startdate > DateTime.Now || price <0)
            {
                return false;
            }

            return true;
        }
    }
}
