using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Helpers
{
    public class ClientHelpers
    {
        public static string GetClientIp()
        {
            return HttpContext.Current?.Request?.UserHostAddress;
        }
    }
}