using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Lan_State_PC_CLIENT
{
    internal class LanClientacts
    {
        private string nickname;
        private string IP_serv;
        private int port;
        private bool IsActive = false;

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
                    string ServerMS_tmp = await ReadMS.ReadLineAsync();
                    // как разешить 1 спец символ смотреть надо
                    string ServerMS = Regex.Replace(ServerMS_tmp, @"[^a-zA-Z0-9:]", "");
                    //строка больше чем ожидается
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
        }
        // метод для получения статистики
        public string SendInfo()
        {
            // создаем стринг билдер и добовляем через : данные
            StringBuilder Sendinfo_txt = new StringBuilder();
            //добовляем локальный ip
            IPAddress[] IPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in IPs)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Sendinfo_txt.Append(ip.ToString());
                    Sendinfo_txt.Append(':');
                    break;
                }
            }
            // Проверяем есть ли доступ к интернету
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
            Sendinfo_txt.Append(':');
            //получаем имя системы (Windows n)
            //получение данных 

            Sendinfo_txt.Append(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName",""));
           
            //получаем тип процесора
            Sendinfo_txt.Append(':');
            Sendinfo_txt.Append(Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0", "ProcessorNameString",""));
            // отдаем строку
            return Sendinfo_txt.ToString();
            

        }
    }
}
