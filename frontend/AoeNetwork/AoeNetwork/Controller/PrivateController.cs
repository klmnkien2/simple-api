//using System;
//using System.Net;
//using System.Collections;
//using System.Data;
//using RestSharp;
//using WinFormMVC.Model;
//using System.Windows.Forms;
//using Newtonsoft.Json;
//using System.Diagnostics;
//using System.Timers;
//using System.Collections.Generic;

//namespace WinFormMVC.Controller
//{
//    public class PrivateController
//    {
//        IChatView parentView;
//        IList _messages;
//        string read_ids;

//        public PrivateController(IChatView parentView)
//        {
//            this.parentView = parentView;
//            _messages = new ArrayList();
//            read_ids = "0";
//        }

//        public void LoadView()
//        {
//            ReceiveMessage(true);
//            ScheduleWork();
//        }

//        private void ScheduleWork()
//        {
//            System.Timers.Timer aTimer = new System.Timers.Timer();
//            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
//            aTimer.Interval = 5000;
//            aTimer.Enabled = true;
//        }

//        private void OnTimedEvent(object source, ElapsedEventArgs e)
//        {
//            ReceiveMessage(false);         
//        }

//        public void SendMessage(Friend friend, string message)
//        {
//            if (StaticValue.user_id == 0 || StaticValue.username == "") return;
//            var client = new RestClient(Properties.Resources.API_URL);

//            var request = new RestRequest("room/message/", Method.POST);
//            request.AddParameter("receive_id", friend.user_id);
//            request.AddParameter("receive_name", friend.user_name);
//            request.AddParameter("user_id", StaticValue.user_id);
//            request.AddParameter("user_name", StaticValue.username);
//            request.AddParameter("message", message);

//            // easily add HTTP Headers
//            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
//            request.Timeout = 10000;

//            var asyncHandler = client.ExecuteAsync(request, r =>
//            {
//                if (r.ResponseStatus == ResponseStatus.Completed)
//                {
//                    if (r.StatusCode == HttpStatusCode.OK)
//                    {
//                        var definition = new { message_id = new object(), status = new object() };
//                        var data = JsonConvert.DeserializeAnonymousType(r.Content, definition);
//                        lock (_messages)
//                        {
//                            var amessage = new APIMessage();
//                            amessage.message_id = int.Parse(data.message_id.ToString());
//                            amessage.receive_id = friend.user_id;
//                            amessage.receive_name = friend.user_name;
//                            amessage.user_id = StaticValue.user_id;
//                            amessage.user_name = StaticValue.username;
//                            amessage.message = message;
//                            parentView.AddPrivateMessage(amessage);

//                            read_ids += ("," + amessage.message_id);
//                        }
//                    }
//                    else
//                    {
//                        // LOG ERROR SENT
//                    }
//                }
//                else
//                {
//                    // LOG ERROR SENT REASON NETWORK CONNECTION
//                }
//            });
//        }

//        public void UpdateFriendStatus(Friend friend, int type, int ignore_type)
//        {
//            if (StaticValue.user_id == 0 || StaticValue.username == "") return;
//            var client = new RestClient(Properties.Resources.API_URL);

//            // PreUpload when ignore_type = -2 mean we need to take that processing
//            if (ignore_type == -2)
//            {
//                if (friend.type == -1)
//                {
//                    if (StaticValue.user_id == friend.from)
//                    {
//                        if (friend.ignore_type == 3)
//                        {
//                            type = -1;
//                            ignore_type = 2;
//                        }
//                        else
//                        {
//                            type = 0;
//                            ignore_type = 0;
//                        }
//                    }
//                    else {                        
//                        type = 0;
//                        ignore_type = 0;
//                    }
//                }
//                else
//                {
//                    if (StaticValue.user_id == friend.from)
//                    {
//                        type = -1;
//                        ignore_type = 1;
//                    }
//                    else
//                    {
//                        type = -1;
//                        ignore_type = 2;
//                    }
//                }
//            }
//            // END preupload

//            var request = new RestRequest("friend/update/", Method.POST);
//            request.AddParameter("user1", StaticValue.user_id);
//            request.AddParameter("user2", friend.user_id);
//            request.AddParameter("type", type);
//            request.AddParameter("ignore_type", ignore_type);

//            // easily add HTTP Headers
//            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
//            request.Timeout = 10000;

//            var asyncHandler = client.ExecuteAsync(request, r =>
//            {
//                if (r.ResponseStatus == ResponseStatus.Completed)
//                {
//                    if (r.StatusCode == HttpStatusCode.OK)
//                    {
//                        // REMOVE OR UPDATE ITEM in chatview Frienlist, ignoreList
//                        parentView.AfterUpdateFriend();
//                    }
//                    else
//                    {
//                        // LOG ERROR SENT - JUST TRY ANOTHER REQUEST
//                    }
//                }
//                else
//                {
//                    // LOG ERROR SENT REASON NETWORK CONNECTION
//                }
//            });
//        }

//        public void ReceiveMessage(bool first_time)
//        {
//            if (StaticValue.user_id == 0 || StaticValue.username == "") return;

//            var client = new RestClient(Properties.Resources.API_URL);

//            var request = new RestRequest("user/history/", Method.GET);
//            request.AddParameter("user_id", StaticValue.user_id);
//            request.AddParameter("read_ids", read_ids);

//            // easily add HTTP Headers
//            request.Timeout = 10000;

//            var asyncHandler = client.ExecuteAsync(request, r =>
//            {
//                if (r.ResponseStatus == ResponseStatus.Completed)
//                {
//                    if (r.StatusCode == HttpStatusCode.OK)
//                    {
//                        if (StaticValue.user_id == 0 || StaticValue.username == "") return;

//                        //MessageBox.Show(r.Content);
//                        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(r.Content);
//                        DataTable dataTable = dataSet.Tables["history"];

//                        lock (_messages)
//                        {
//                            _messages.Clear();
//                            read_ids = "0";
//                            foreach (DataRow row in dataTable.Rows)
//                            {
//                                var message = new APIMessage();
//                                message.message_id = int.Parse(row["message_id"].ToString());
//                                message.receive_id = int.Parse(row["receive_id"].ToString());
//                                message.receive_name = row["receive_name"].ToString();
//                                message.user_id = int.Parse(row["user_id"].ToString());
//                                message.user_name = row["user_name"].ToString();
//                                message.message = row["message"].ToString();
//                                message.notify = int.Parse(row["notify"].ToString());
//                                message.create_time = long.Parse(row["create_time"].ToString());

//                                read_ids = read_ids + "," + message.message_id;
//                                _messages.Add(message);
//                            }
//                            if (_messages.Count > 0)
//                            {
//                                parentView.RenderPrivateMessage(first_time, _messages);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        // LOG ERROR SENT - JUST TRY ANOTHER REQUEST
//                    }
//                }
//                else
//                {
//                    // LOG ERROR SENT REASON NETWORK CONNECTION
//                }
//            });
//        }

//        public void HistoryMessage(int receive_id, int last_view_id)
//        {
//            if (StaticValue.user_id == 0 || StaticValue.username == "") return;

//            var client = new RestClient(Properties.Resources.API_URL);

//            var request = new RestRequest("user/history/", Method.GET);
//            request.AddParameter("user_id", StaticValue.user_id);
//            request.AddParameter("receive_id", receive_id);
//            request.AddParameter("last_view_id", last_view_id);

//            // easily add HTTP Headers
//            request.Timeout = 10000;

//            var asyncHandler = client.ExecuteAsync(request, r =>
//            {
//                if (r.ResponseStatus == ResponseStatus.Completed)
//                {
//                    if (r.StatusCode == HttpStatusCode.OK)
//                    {
//                        if (StaticValue.user_id == 0 || StaticValue.username == "") return;

//                        //MessageBox.Show(r.Content);
//                        DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(r.Content);
//                        DataTable dataTable = dataSet.Tables["history"];

//                        lock (_messages)
//                        {
//                            _messages.Clear();
//                            read_ids = "0";
//                            foreach (DataRow row in dataTable.Rows)
//                            {
//                                var message = new APIMessage();
//                                message.message_id = int.Parse(row["message_id"].ToString());
//                                message.receive_id = int.Parse(row["receive_id"].ToString());
//                                message.receive_name = row["receive_name"].ToString();
//                                message.user_id = int.Parse(row["user_id"].ToString());
//                                message.user_name = row["user_name"].ToString();
//                                message.message = row["message"].ToString();
//                                message.notify = int.Parse(row["notify"].ToString());
//                                message.create_time = long.Parse(row["create_time"].ToString());

//                                read_ids = read_ids + "," + message.message_id;
//                                _messages.Add(message);
//                            }
//                            if (_messages.Count > 0)
//                            {
//                                parentView.RenderOldPrivate(receive_id, _messages);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        // LOG ERROR SENT - JUST TRY ANOTHER REQUEST
//                    }
//                }
//                else
//                {
//                    // LOG ERROR SENT REASON NETWORK CONNECTION
//                }
//            });
//        }
//    }
//}
