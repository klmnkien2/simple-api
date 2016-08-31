using System;

namespace AoeNetwork
{
    public class APIMessage
    {

        public int message_id { get; set; }
        public int room_id { get; set; }
        public int user_id { get; set; }
        public int receive_id { get; set; }
        public string receive_name { get; set; }
        public int notify { get; set; }
        public string message_format { get; set; }
        public string user_name { get; set; }
        public string message { get; set; }
        public long create_time { get; set; }
    } 

}
