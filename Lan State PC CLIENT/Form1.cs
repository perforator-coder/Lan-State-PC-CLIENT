namespace Lan_State_PC_CLIENT
{
    public partial class Form1 : Form
    {
        private string IP_serv;
        private int PORT_serv;
        private string NICK_client;
        private LanClientacts ClientAct;
        public Form1()
        {
            InitializeComponent();
            //скрываем форму
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.notifyIcon1.Icon = this.Icon;
            
            this.notifyIcon1.Visible = true;
            
            if (!File.Exists("IP_SERV.txt") || !File.Exists("PORT_serv.txt") || !File.Exists("NICK_client.txt"))
            {
                //вызываем форму для изменения данных
                this.Show();
                this.notifyIcon1.Visible = false;
                this.ShowInTaskbar = true;

            }
            else
            {
                //Иначе мы передаем данные из файлов в переменые
                if (!int.TryParse(File.ReadAllText("PORT_serv.txt"), out PORT_serv))
                {
                    //вызываем форму для изменения
                    this.Show();
                    this.notifyIcon1.Visible = false;
                    this.ShowInTaskbar = true;
                }
                else
                {
                    // если есть все то создаем подключение
                    IP_serv = File.ReadAllText("IP_SERV.txt");
                    NICK_client = File.ReadAllText("NICK_client.txt");
                    IP_SERVER_BOX.Text = IP_serv;
                    PORT_SERVER_BOX.Text = PORT_serv.ToString();
                    NICK_CLIENT_BOX.Text = NICK_client;
                    ClientAct =  new LanClientacts(IP_serv, PORT_serv, NICK_client);
                    ClientAct.StartClient();
                }
            }
            // проверяем данные если они коректны то создаем TCP соединения
            this.FormClosed += new FormClosedEventHandler(SaveClientData);
        }

        private void SAVE_PAM_Click(object sender, EventArgs e)
        {
            // проверяем введенные данные
            if (string.IsNullOrWhiteSpace(IP_SERVER_BOX.Text) || !int.TryParse(PORT_SERVER_BOX.Text, out PORT_serv) || string.IsNullOrWhiteSpace(NICK_CLIENT_BOX.Text))
            {
                MessageBox.Show("Ошибка: Данные не коректны", "CLIENT DATA ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IP_serv = IP_SERVER_BOX.Text;
            NICK_client = NICK_CLIENT_BOX.Text;
            File.WriteAllText("IP_SERV.txt", IP_serv);
            File.WriteAllText("PORT_serv.txt", PORT_serv.ToString());
            File.WriteAllText("NICK_client.txt", NICK_client);
            // скрываем в трей программу
            this.Hide();
            this.notifyIcon1.Visible = true;
            //и создаем соединение
            ClientAct = new LanClientacts(IP_serv,PORT_serv,NICK_client);
            ClientAct.StartClient();


        }
        private void SaveClientData(object sender, EventArgs e)
        {
            // Записываем данные из txt
            File.WriteAllText("IP_SERV.txt", IP_serv);
            File.WriteAllText("PORT_serv.txt", PORT_serv.ToString());
            File.WriteAllText("NICK_client.txt", NICK_client);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // при нажатии на приложение в трее открывается меню
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            // и останавливается клиенская часть
            //ClientAct.StopClient();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_app form_about = new About_app();
            form_about.ShowDialog();
        }
    }
}
