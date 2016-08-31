using System;

namespace AoeNetwork
{
    public class Friend
    {

        public int user_id { get; set; }
        public string user_name { get; set; }
        public string avatar { get; set; }
        public string status { get; set; }
        public int state { get; set; }
        public int type { get; set; }
        public int ignore_type { get; set; }
        public int from { get; set; } //id of user1
    } 

}
