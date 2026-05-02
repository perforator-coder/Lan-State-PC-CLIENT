using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace Lan_State_PC_CLIENT
{
    internal class LanClientacts
    {
        private string nickname;
        private string IP_serv;
        private int port;
        private bool IsActive = false;
        private CancellationTokenSource ctl = new CancellationTokenSource();

        public LanClientacts(string IP_serv, int PORT_serv, string Nickname)
        {
            this.nickname = Nickname;
            this.IP_serv = IP_serv;
            this.port = PORT_serv;

        }

        public async Task<bool> StartClient()
        {
            TcpClient tcpClient = new TcpClient(this.IP_serv, this.port);
            IsActive = true;
            if (tcpClient.Connected)
            {
                //что будет если соединение сеть
                NetworkStream stream = tcpClient.GetStream();
                StreamReader ReadMS = new StreamReader(stream, Encoding.UTF8);
                StreamWriter SendMS = new StreamWriter(stream, Encoding.UTF8);
                SendMS.AutoFlush = true; // установка автоматической отправки
                while (tcpClient.Connected && IsActive)
                {
                    string ServerMS_tmp = await ReadMS.ReadLineAsync(ctl.Token);
                    
                    string ServerMS = Regex.Replace(ServerMS_tmp, @"[^a-zA-Z0-9:А-Яа-я ]", "");
                    if (ServerMS.Contains(':'))
                    {
                        string[] split_ServerMS = ServerMS.Split(':');
                        if (split_ServerMS[0] == "MS")
                        {
                            MessageBox.Show(split_ServerMS[1],"Server MS",MessageBoxButtons.OK);
                            await SendMS.WriteLineAsync("OK");
                            continue;
                        }
                    }
                    switch (ServerMS)
                    {
                        case "PINGID":
                            // отправляем ID если есть запрос
                            await SendMS.WriteLineAsync(nickname);
                            break;
                        case "STATUS":
                            await SendMS.WriteLineAsync("CLIENT:OK");
                            break;
                        case "GETINFO":
                            await SendMS.WriteLineAsync(SendInfo());
                            break;
                        case "SHUTDOWN":
                            Process.Start("shutdown","/s /t 0 /f");
                            StopClient();
                            break;
                        case "RESTART":
                            Process.Start("shutdown", "/r /t 0 /f");
                            StopClient();
                            break;
                        
                        default:
                            return true;

                    }
                }
                return true;
            }
            else
            {
                MessageBox.Show("Соединения с сервером нет", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        public void StopClient()
        {
            IsActive = false;
            ctl.Cancel();
            
        }
        // метод для получения статистики
        public string SendInfo()
        {
            // создаем стринг билдер и добовляем через : данные
            StringBuilder Sendinfo_txt = new StringBuilder();
            //добовляем локальный ip 0
            IPAddress[] IPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in IPs)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Sendinfo_txt.Append(ip.ToString());
                    Sendinfo_txt.Append(',');
                    break;
                }
            }
            // Проверяем есть ли доступ к интернету 1
            bool haveConect = false;
            try
            {
                Ping Pingsb = new Ping();
                Byte[] bufer = new byte[32];
                PingOptions Options = new PingOptions();
                PingReply Reply = Pingsb.Send("77.88.8.8", 5000, bufer, Options);
                if (Reply.Status == IPStatus.Success)
                {
                    haveConect = true;
                }
            }
            catch (Exception)
            {
                haveConect=false;
            }
            if (haveConect)
            {
                Sendinfo_txt.Append("Да");
            }
            else
            {
                Sendinfo_txt.Append("Нет");
            }
            Sendinfo_txt.Append(',');
            //получаем имя системы (Windows n)
            //получение данных 2
            string os_info = "";
            string Gpu_info = "";
            string mac = "";
            try 
            {
                using (var INFO_OS = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem"))
                {
                    
                    foreach (var INFO in INFO_OS.Get())
                    {
                        os_info = INFO["Caption"]?.ToString();
                        //MessageBox.Show(os_info);
                        break;
                    }
                }
                // получение gpu 4
                using (ManagementObjectSearcher Ser = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
                {
                    foreach (var info in Ser.Get())
                    {
                        Gpu_info = info["Name"].ToString();
                        break;
                    }
                }
                using (ManagementObjectSearcher Ser = new ManagementObjectSearcher("SELECT MacAddress FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True"))
                {
                    foreach (ManagementObject info in Ser.Get())
                    {
                        if (info["MacAddress"] != null )
                        {
                            mac = info["MacAddress"].ToString();
                        }
                    }    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "er,er,er,er,er,er";
            }
            //MessageBox.Show(os_info);
            Sendinfo_txt.Append(os_info);
            Sendinfo_txt.Append(',');
           
            //получаем тип процесора 3
            Sendinfo_txt.Append(Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0", "ProcessorNameString","Неизвестно").ToString());
            Sendinfo_txt.Append(',');
            // добовляем gpu 4
            Sendinfo_txt.Append(Gpu_info);
            Sendinfo_txt.Append(',');
            // Добовляем mac адрес 5
           
            
            Sendinfo_txt.Append(mac);
            Sendinfo_txt.Append(',');
            // отдаем строку
            //MessageBox.Show(Sendinfo_txt.ToString());
            return Sendinfo_txt.ToString();
            

        }
    }
}
