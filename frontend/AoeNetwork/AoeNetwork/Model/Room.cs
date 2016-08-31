using System;
using System.Collections.Generic;

namespace AoeNetwork
{
    public class Room
    {

        public int room_id { get; set; }
        public string room_name { get; set; }
        public int parent_id { get; set; }
        public string server_id { get; set; }
        public int members { get; set; }
        public int maximum { get; set; }
        public string host { get; set; }
        public string port { get; set; }
        public string hub { get; set; }
        public int number_connected { get; set; }

    } 

}
