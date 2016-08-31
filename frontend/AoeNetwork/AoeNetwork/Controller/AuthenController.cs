using System;
using System.Net;
using System.Collections;
using RestSharp;
using System.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;

namespace AoeNetwork
{
    public class AuthenController
    {
        LoginWindow _view;

        public AuthenController(LoginWindow view)
        {
            _view = view;
        }

        public void LoadView()
        {
            _view.ClearForm();
        }

        public void Login()
        {
            _view.LoginEnable(false);
            var client = new RestClient(AoeNetwork.Properties.Resources.API_URL);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("user/login/", Method.POST);
            request.AddParameter("username", _view.UserName);
            request.AddParameter("password", _view.Password);
            request.AddParameter("state", _view.State);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //request.Timeout = 10000;

            var asyncHandler = client.ExecuteAsync(request, r =>
            {
                if (r.ResponseStatus == ResponseStatus.Completed)
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        Success(r.Content);
                    }
                    else
                    {
                        UnSuccess("API connect Error");
                    }
                }
                else
                {
                    UnSuccess("Network error");
                }

            });

        }

        private void Success(string content)
        {
            try
            {
                Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(content.ToString());
                if (data["needpay"] != "0")
                {
                    _view.NeedPay();
                    return;
                }
                StaticValue.user_id = int.Parse(data["user_id"]);
                StaticValue.username = data["username"];
                StaticValue.avatar = data["avatar"];
                StaticValue.level = data["level"];
                StaticValue.diamond = data["diamond"];
                StaticValue.state = int.Parse(data["state"]);
                StaticValue.status = data["status"];
                if (data["last_active"] != null)
                {
                    StaticValue.last_active = long.Parse(data["last_active"]);
                }

                _view.GoChat();
            }
            catch (Exception ex)
            {
                UnSuccess("Du lieu API tra ve chua khop." + ex.Message);
            }
            finally
            {
                _view.LoginEnable(true);
            }
        }

        private void UnSuccess(string content)
        {
            _view.LoginEnable(true);
            try
            {
                
                Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                MessageBox.Show(data["error"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Du lieu API tra ve chua khop.");
            }  
        }
    }
}
