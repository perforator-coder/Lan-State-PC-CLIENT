using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Lan_State_PC_CLIENT
{
    internal class LanClientacts
    {
        private string nickname;
        private string IP_serv;
        private int port;
        private bool IsActive = false;
        
        public LanClientacts(string IP_serv,int PORT_serv,string Nickname)
        {
            this.nickname = Nickname;
            this.IP_serv = IP_serv;
            this.port = PORT_serv;
            
        }

        public async Task<bool> StartClient()
        {
            TcpClient tcpClient = new TcpClient(this.IP_serv, this.port);
            
            if (tcpClient.Connected)
            {
                //что будет если соединение сеть
                NetworkStream stream = tcpClient.GetStream();
                StreamReader ReadMS = new StreamReader(stream, Encoding.UTF8);
                StreamWriter SendMS = new StreamWriter(stream, Encoding.UTF8);
                SendMS.AutoFlush = true; // установка автоматической отправки
                while (tcpClient.Connected)
                {
                    string ServerMS = await ReadMS.ReadLineAsync();
                    switch (ServerMS)
                    {
                        case "PING_ID":
                            // отправляем ID если есть запрос
                            await SendMS.WriteLineAsync(nickname);
                            break;
                        case "STATUS":
                            await SendMS.WriteLineAsync("CLIENT:OK");
                            break;

                    }
                }
                return true;
            }
            else
            {
                MessageBox.Show("Соединения с сервером нет","Connection error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
           
        }
    }
}
