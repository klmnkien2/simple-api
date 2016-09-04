using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Media.Imaging;

namespace AoeNetwork
{
    public class SystemUtils
    {
        // return null if no error
        public static string CallVPNConnection(Room room)
        {
            //return null;
            string cmd = "vpncmd.exe localhost /CLIENT /CMD AccountGet \"aoe_vpn_connection\"";
            string result = tryExternalCmd(cmd);
            if (result.Contains("(Error code: 36)"))
            {
                cmd = "vpncmd.exe localhost /CLIENT /CMD AccountCreate \"aoe_vpn_connection\" /SERVER:103.56.157.252:443 /HUB:aoe_vpn_hub /USERNAME:client_test /NICNAME:adapter_test";
                result = tryExternalCmd(cmd);
            }

            cmd = "vpncmd.exe localhost /CLIENT /CMD AccountSet \"aoe_vpn_connection\" /SERVER:" + room.host + ":" + room.port + " /HUB:" + room.hub;
            result = tryExternalCmd(cmd);

            cmd = "vpncmd.exe localhost /CLIENT /CMD AccountUsernameSet \"aoe_vpn_connection\" /USERNAME:test1";// +StaticValue.username;
            result = tryExternalCmd(cmd);

            cmd = "vpncmd.exe localhost /CLIENT /CMD AccountPasswordSet \"aoe_vpn_connection\" /PASSWORD:123456 /TYPE:standard";
            result = tryExternalCmd(cmd);

            cmd = "vpncmd.exe localhost /CLIENT /CMD AccountDisconnect  \"aoe_vpn_connection\"";
            result = tryExternalCmd(cmd);

            setInterfaceMetric();

            cmd = "vpncmd.exe localhost /CLIENT /CMD AccountConnect  \"aoe_vpn_connection\"";
            result = tryExternalCmd(cmd);

            if (!result.Contains("--ERROR--"))
            {
                return null;
            }
            else
            {
                return "An error occured. Softether Client is not installed property or Network connection lost. ";
            }
        }

        private static void setInterfaceMetric()
        {
            string cmd = "netsh interface ipv4 show address";
            string result = tryExternalCmd(cmd);

            var interfaceArr = System.Text.RegularExpressions.Regex.Split(result, "Configuration for interface");
            foreach (var interfaceObj in interfaceArr)
            {
                if (interfaceObj.Contains("VPN"))
                {
                    int start = interfaceObj.IndexOf('"', 0);
                    int end = interfaceObj.IndexOf('"', start + 1);

                    string intefaceName = interfaceObj.Substring(start + 1, end - start - 1);
                    cmd = "netsh interface ipv4 set interface \"" + intefaceName + "\" metric=9999";
                    result = tryExternalCmd(cmd);
                    break;
                }
            }
        }

        private static string tryExternalCmd(string suffixCmd)
        {
            string cmd = "C:/Windows/System32/" + suffixCmd;
            string result = runCommand(cmd);

            if (result.Contains("is not recognized as an internal or external command"))
            {
                cmd = "C:/Windows/sysnative/" + suffixCmd;
                result = runCommand(cmd);
            }

            if (result.Contains("is not recognized as an internal or external command"))
            {
                cmd = "C:/Windows/SysWOW64/" + suffixCmd;
                result = runCommand(cmd);
            }

            if (result.Contains("is not recognized as an internal or external command"))
            {
                result = "--ERROR--" + "VPN is not installed";
            }
            Console.WriteLine(result);

            return result;
        }

        private static string runCommand(string command)
        {
            try
            {
                //* Create your Process
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/C " + command;
                //process.StartInfo.WorkingDirectory = "C:\\Windows\\System32\\";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                //* Start process and handlers
                process.Start();
                string result = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                if (result == null || result.Equals(""))
                {
                    result = "--ERROR--" + process.StandardError.ReadToEnd();
                }

                return result;
            }
            catch (Exception ex)
            {
                return "--ERROR--" + ex.Message;
            }
        }

        public static String getVPNLanIp()
        {
            try
            {
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        if (ni.Name.Contains("VPN Client"))
                        {
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    return ip.Address.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            return "";
        }

        public static void OpenGame(string pathToExe)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo(pathToExe);
                processStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(pathToExe);
                Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                //LOG can not run file
            }
        }

        public static BitmapImage getResource(string name)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Images/" + name + ".png");
            bitmap.EndInit();

            return bitmap;
        }

        public static BitmapImage getImageUrl(string url)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            return bitmap;
        }
    }
}
