using System;
using System.Net;
using System.Collections;
using System.Data;
using RestSharp;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Timers;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows;

namespace AoeNetwork
{
    public class RoomController
    {
        RoomWindow _view;
        IList _users;
        IList _messages;
        Room _room = null;
        Object flag_conn = new Object();
        string read_ids;

        public RoomController(RoomWindow view)
        {
            view.SetRoomController(this);
            _view = view;
            _users = new ArrayList();
            _messages = new ArrayList();
            read_ids = "0";
        }

        public IList Users
        {
            get
            {
                return _users;
            }
        }

        public Room CurrentRoom
        {
            get
            {
                return this._room;
            }
        }

        public void LoadView()
        {
            //LoadAds();
            LoadChannel(null);
            //LoadTreeRoom();
            _view.SetUserInfo(StaticValue.username, StaticValue.status, StaticValue.avatar,
                StaticValue.level, StaticValue.diamond, StaticValue.state);
        }

        public void ScheduleWork()
        {
            OnTimedEvent(null, null);

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;

        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ReceiveMessage();
            LoadUsers();
            _view.SetUserInfo(StaticValue.username, StaticValue.status, StaticValue.avatar,
                StaticValue.level, StaticValue.diamond, StaticValue.state);
        }

        public void LoadAds()
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "") return;

            var client = new RestClient(Properties.Resources.API_URL);
            var request = new RestRequest("room/ads/", Method.GET);

            // easily add HTTP Headers
            request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        // LOAD Ads to area
                        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(r.Content);

                        DataTable dataTable = dataSet.Tables["ads"];

                        lock (flag_conn)
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string url = row["url"].ToString();
                                string image = row["image"].ToString();
                                int type = int.Parse(row["type"].ToString());

                                _view.SetAdsInfo(url, image, type);
                            }

                        }
                    }
                    else
                    {
                        // LOG ERROR SENT
                    }
                }
                else
                {
                    // LOG ERROR SENT REASON NETWORK CONNECTION
                }
            });
        }

        public void LoadChannel(string channel_id)
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "") return;
            
            var client = new RestClient(Properties.Resources.API_URL);
            var request = new RestRequest("room/channel/" + (channel_id != null ? ("?channel_id=" + channel_id) : ""), Method.GET);

            // easily add HTTP Headers
            request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        // LOAD Ads to area
                        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(r.Content);

                        lock (flag_conn)
                        {
                            if (channel_id == null)
                            {
                                DataTable dataTable = dataSet.Tables["channel"];

                                _view.SetChannel(dataTable);
                                
                            }
                            else
                            {
                                DataTable dataTable = dataSet.Tables["room"];

                                _view.SetRoomOfChannel(dataTable);
                            }

                        }
                    }
                    else
                    {
                        // LOG ERROR SENT
                    }
                }
                else
                {
                    // LOG ERROR SENT REASON NETWORK CONNECTION
                }
            });
        }

        public void LoadUsers()
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "" || _room == null) return;

            var client = new RestClient(Properties.Resources.API_URL);

            var request = new RestRequest("user/room/", Method.GET);
            request.AddParameter("room_id", _room.room_id);
            request.AddParameter("user_id", StaticValue.user_id);
            request.AddParameter("ip", SystemUtils.getVPNLanIp());
            request.AddParameter("ping", SystemUtils.getVPNPing(_room.host));


            // easily add HTTP Headers
            request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(r.Content);

                        DataTable dataTable = dataSet.Tables["users"];
                        lock (Users)
                        {
                            Users.Clear();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                try
                                {
                                    var user = new UserCache();
                                    user.user_id = int.Parse(row["user_id"].ToString());
                                    user.user_name = row["user_name"].ToString();
                                    user.avatar = row["avatar"].ToString();
                                    user.ip = row["ip"].ToString();
                                    user.ping = row["ping"].ToString();
                                    user.level = row["level"].ToString();
                                    user.state = int.Parse(row["state"].ToString());

                                    Users.Add(user);
                                }
                                catch (Exception nocatch) { }
                            }

                            _view.SetUserList(Users);
                        }
                    }
                    else
                    {
                        // LOG ERROR API
                    }
                }
                else
                {
                    // LOG ERROR NETWORK
                }
            });
        }

        private void LoadTreeRoom()
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "") return;
            var client = new RestClient(Properties.Resources.API_URL);

            var request = new RestRequest("room/list/", Method.GET);

            // easily add HTTP Headers
            request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(r.Content);
                        DataTable dataTable = dataSet.Tables["rooms"];

                        //_view.SetTreeRoom(dataTable);
                    }
                    else
                    {
                        // LOG ERROR LOAD USERS API
                    }
                }
                else
                {
                    // LOG ERROR NETWORK
                }
            });

        }

        public void JoinRoom(Room room)
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "") return;

            var client = new RestClient(Properties.Resources.API_URL);

            var request = new RestRequest("user/join/", Method.POST);
            request.AddParameter("user_id", StaticValue.user_id);
            request.AddParameter("user_name", StaticValue.username);
            request.AddParameter("room_id", room.room_id);
            request.AddParameter("room_name", room.room_name);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        _room = room;
                        _users = new ArrayList();
                        _messages = new ArrayList();
                        read_ids = "0";
                        _view.SuccessJoinRoom(room);
                        
                    }
                    else
                    {
                        // LOG ERROR SENT
                        MessageBox.Show("Yêu cầu tham gia phòng không thành công. Vui lòng thử lại!");
                    }
                }
                else
                {
                    // LOG ERROR SENT REASON NETWORK CONNECTION
                    MessageBox.Show("Network error!");
                }
            });
        }

        public void SendMessage(string message)
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "" || _room == null) return;
            var client = new RestClient(Properties.Resources.API_URL);

            var request = new RestRequest("room/message/", Method.POST);
            request.AddParameter("room_id", _room.room_id);
            request.AddParameter("user_id", StaticValue.user_id);
            request.AddParameter("user_name", StaticValue.username);
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
                        var definition = new { message_id = new object(), status = new object() };
                        var data = JsonConvert.DeserializeAnonymousType(r.Content, definition);
                        lock (_messages)
                        {
                            var amessage = new APIMessage();
                            amessage.message_id = int.Parse(data.message_id.ToString());
                            amessage.room_id = _room.room_id;
                            amessage.user_id = StaticValue.user_id;
                            amessage.user_name = StaticValue.username;
                            amessage.message = message;
                            _view.AddPrivateMessage(amessage);

                            read_ids += ("," + amessage.message_id);
                        }

                    }
                    else
                    {
                        // LOG ERROR SENT
                    }
                }
                else
                {
                    // LOG ERROR SENT REASON NETWORK CONNECTION
                }
            });
        }

        public void ReceiveMessage()
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "" || _room == null) return;

            var client = new RestClient(Properties.Resources.API_URL);

            var request = new RestRequest("room/history/", Method.GET);
            request.AddParameter("room_id", _room.room_id);
            request.AddParameter("read_ids", read_ids);

            // easily add HTTP Headers
            request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        if (StaticValue.user_id == 0 || StaticValue.username == "") return;

                        //MessageBox.Show(r.Content);
                        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(r.Content);
                        DataTable dataTable = dataSet.Tables["history"];

                        lock (_messages)
                        {
                            _messages.Clear();
                            read_ids = "0";
                            foreach (DataRow row in dataTable.Rows)
                            {
                                var message = new APIMessage();
                                message.message_id = int.Parse(row["message_id"].ToString());
                                message.user_id = int.Parse(row["user_id"].ToString());
                                message.user_name = row["user_name"].ToString();
                                message.message = row["message"].ToString();
                                message.notify = int.Parse(row["notify"].ToString());
                                message.create_time = long.Parse(row["create_time"].ToString());

                                read_ids = read_ids + "," + message.message_id;
                                _messages.Add(message);
                            }
                            if (_messages.Count > 0)
                            {
                                _view.RenderMessage(_messages);
                            }
                        }
                    }
                    else
                    {
                        // LOG ERROR SENT - JUST TRY ANOTHER REQUEST
                    }
                }
                else
                {
                    // LOG ERROR SENT REASON NETWORK CONNECTION
                }
            });
        }

        public void HistoryMessage(int last_view_id)
        {
            if (StaticValue.user_id == 0 || StaticValue.username == "" || _room == null) return;

            var client = new RestClient(Properties.Resources.API_URL);

            var request = new RestRequest("room/history/", Method.GET);
            request.AddParameter("room_id", _room.room_id);
            request.AddParameter("old", 1);
            request.AddParameter("last_view_id", last_view_id);

            // easily add HTTP Headers
            request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(r.Content);
                        DataTable dataTable = dataSet.Tables["history"];

                        lock (_messages)
                        {
                            _messages.Clear();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                var message = new APIMessage();
                                message.message_id = int.Parse(row["message_id"].ToString());
                                message.user_id = int.Parse(row["user_id"].ToString());
                                message.user_name = row["user_name"].ToString();
                                message.message = row["message"].ToString();
                                message.notify = int.Parse(row["notify"].ToString());
                                message.create_time = long.Parse(row["create_time"].ToString());

                                read_ids = read_ids + "," + message.message_id;
                                _messages.Add(message);
                            }
                            if (_messages.Count > 0)
                            {
                                _view.RenderOldMessage(_messages);
                            }
                        }
                    }
                    else
                    {
                        // LOG ERROR SENT - JUST TRY ANOTHER REQUEST
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
