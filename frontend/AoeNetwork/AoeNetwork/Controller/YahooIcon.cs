using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace AoeNetwork
{
    public class YahooIcon
    {
        public static string BASE_URL = "/yahooicon/";
        // return null if no error
        public static string getIcon(string icon)
        {
            return "<img src=\"" + AoeNetwork.Properties.Resources.API_URL.Replace("/index.php", "") + BASE_URL + icon + "\" />";
        }

        public static string translateText(string text)
        {
            if (map == null)
            {
                init();
            }
            foreach (KeyValuePair<string, string> entry in map)
            {
                text = text.Replace(entry.Key, getIcon(entry.Value));
            }

            return text;
        }

        private static Dictionary<string, string> map = null;
        private static void init()
        {
            map = new Dictionary<string, string>();
            map.Add(":)", "happy.gif");
            map.Add(":(", "sad.gif");
            map.Add(";)", "winking.gif");
            map.Add(":D", "big_grin.gif");
            map.Add(">:D<", "big hug.gif");
            map.Add(":-/", "confused.gif");
            map.Add(":x", "love_struck.gif");
            map.Add(":\">", "blushing.gif");
            map.Add(":P", "tongue.gif");
            map.Add(":-*", "kiss.gif");
            map.Add("=((", "broken_heart.gif");
            map.Add(":-O", "surprise.gif");
            map.Add("X(", "angry.gif");
            map.Add(":>", "smug.gif");
            map.Add("B-)", "cool.gif");
            map.Add(":-S", "worried.gif");
            map.Add("#:-S", "whew!.gif");
            map.Add(">:)", "devil.gif");
            map.Add(":((", "crying.gif");
            map.Add(":))", "laughing.gif");
            map.Add(":|", "straight_face.gif");
            map.Add("/:)", "raised_eyebrows.gif");
            map.Add("=))", "rolling_on_floor.gif");
            map.Add("O:-)", "angel.gif");
            map.Add(":-B", "nerd.gif");
            map.Add(":-h", "wave.gif");
            map.Add("8->", "day_dreaming.gif");
            map.Add("I-)", "sleepy.gif");
            map.Add("8-|", "rolling_eyes.gif");
            map.Add("L-)", "loser.gif");
            map.Add(":-&", "sick.gif");
            map.Add("[-(", "no_talking.gif");
            map.Add(":O)", "clown.gif");
            map.Add("8-}", "silly.gif");
            map.Add("<:-P", "party.gif");
            map.Add("(:|", "yawn.gif");
            map.Add("=P~", "drooling.gif");
            map.Add(":-?", "thinking.gif");
            map.Add("=D>", "applause.gif");
            map.Add(":-SS", "nail_biting.gif");
            map.Add("@-)", "hypnotized.gif");
            map.Add(":^o", "liar.gif");
            map.Add(":-w", "waiting.gif");
            map.Add(":-<", "sigh.gif");
            map.Add(">:P", "phbbbbt.gif");
            map.Add("<):)", "cowboy.gif");
            map.Add(":-q", "thumb_down.gif");
            map.Add(":-bd", "thumb_up.gif");
        }
    }
}
