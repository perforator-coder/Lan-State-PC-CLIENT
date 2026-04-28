using System;
using System.Collections.Generic;
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

        //public 
    }
}
