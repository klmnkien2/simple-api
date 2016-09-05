using System;
using System.Net;
using System.Collections;
using System.Data;
using RestSharp;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Timers;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AoeNetwork
{
    public class ChatController
    {
        ChatWindow _view;
        IList _rooms;
        IList _friends;
        IList _messages;

        public ChatController(ChatWindow view)
        {
            _view = view;
            _rooms = new ArrayList();
            _friends = new ArrayList();
            _messages = new ArrayList();
        }

        public IList rooms
        {
            get
            {
                return _rooms;
            }
        }

        public IList friends
        {
            get
            {
                return _friends;
            }
        }

        public void LoadView()
        {
            _view.SetUserInfo(StaticValue.username, StaticValue.status, StaticValue.avatar, StaticValue.level, StaticValue.diamond, StaticValue.state);

            LoadFriends();
            ScheduleWork();
        }

        private void ScheduleWork()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 30000;
            aTimer.Enabled = true;

        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            LoadFriends();
        }

        public void LoadFriends()
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "") return;

            var client = new RestClient(Properties.Resources.API_URL);

            var request = new RestRequest("friend/list/", Method.GET);
            request.AddParameter("user_id", StaticValue.user_id);

            // easily add HTTP Headers
            request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(r.Content);

                        DataTable dataTable = dataSet.Tables["friends"];

                        lock (friends)
                        {
                            friends.Clear();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                var friend = new Friend();
                                friend.user_id = int.Parse(row["user_id"].ToString());
                                friend.user_name = row["user_name"].ToString();
                                friend.avatar = row["avatar"].ToString();
                                friend.status = row["status"].ToString();
                                friend.state = int.Parse(row["state"].ToString());
                                friend.type = int.Parse(row["type"].ToString());
                                friend.ignore_type = int.Parse(row["ignore_type"].ToString());
                                friend.from = int.Parse(row["user1"].ToString());

                                if (friend.type == 0)
                                {
                                    friend.state = 0;
                                    friend.status = "Pending...";
                                }
                                else if (friend.type == -1)
                                {
                                    friend.state = 0;
                                    friend.status = "";
                                }

                                friends.Add(friend);
                            }

                            _view.SetFriendList(friends);
                        }
                    }
                    else
                    {
                        //UnSuccess(r.Content);
                    }
                }
                else
                {
                    //UnSuccess("Network error!");
                }
            });
        }

        public void AddFriend(string user_name, string message)
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "") return;

            var client = new RestClient(Properties.Resources.API_URL);

            var request = new RestRequest("friend/add/", Method.POST);
            request.AddParameter("user_id", StaticValue.user_id);
            request.AddParameter("user_name", StaticValue.username);
            request.AddParameter("friend_name", user_name);
            request.AddParameter("message", message);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        LoadFriends();
                        _view.AfterAddFriend(null, null);// CLose form add friend
                    }
                    else
                    {
                        // LOG ERROR SENT - JUST TRY ANOTHER REQUEST
                        _view.AfterAddFriend(null, "User is not exist or already friend");
                    }
                }
                else
                {
                    // LOG ERROR SENT REASON NETWORK CONNECTION
                    _view.AfterAddFriend(null, "Network error!");
                }
            });
        }

        public void UpdateStatus(int state, string status)
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "") return;
            if (state == null) state = StaticValue.state;
            if (status == null) status = StaticValue.status;

            var client = new RestClient(Properties.Resources.API_URL);

            var request = new RestRequest("user/status/", Method.POST);
            request.AddParameter("user_id", StaticValue.user_id);
            request.AddParameter("status", status);
            request.AddParameter("state", state);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        // DO NOTHING
                    }
                }
                else
                {
                    // LOG ERROR SENT REASON NETWORK CONNECTION
                }
            });
        }
    }
}
