using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoeNetwork
{
    public class StaticValue
    {
        public static int user_id = 0;
        public static string username = "";
        public static string avatar = "";
        public static string level = "";
        public static string diamond = "";
        public static int state = 0;
        public static string status = "";
        public static long last_active = 0;

        public static void Clear() {
            user_id = 0;
            username = "";
            avatar = "";
            level = "";
            diamond = "";
            state = 0;
            status = "";
            last_active = 0;
        }
    }
}
