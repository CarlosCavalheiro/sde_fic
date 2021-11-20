using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDE_FIC.Util
{
    public class ValueUtil
    {

        public static string DBNullToString(object  obj)
        {
            if (obj == DBNull.Value )
                return "";
            else
                return (string)obj;
        }

    }

    
}
