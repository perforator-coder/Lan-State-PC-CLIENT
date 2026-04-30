

namespace Lan_State_PC_CLIENT
{
    public partial class Form1 : Form
    {
        private string IP_serv;
        private int PORT_serv;
        private string NICK_client;
        private LanClientacts ClientAct;
        private bool notifyEnable = true;
        public Form1()
        {
            InitializeComponent();
            //скрываем форму
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.notifyIcon1.Icon = this.Icon;
            this.notifyIcon1.Visible = true;
            /// Проверяем есть ли файлы конфиги
            if (File.Exists("notify.txt") && bool.TryParse(File.ReadAllText("notify.txt"), out notifyEnable))
            {
                if (notifyEnable)
                {

                    this.отключитьУведомленияToolStripMenuItem.Text = "Включить уведомления";
                }
                else
                {

                    this.отключитьУведомленияToolStripMenuItem.Text = "Отключить уведомления";
                }
            }
            if (!File.Exists("IP_SERV.txt") || !File.Exists("PORT_serv.txt") || !File.Exists("NICK_client.txt"))
            {
                //вызываем форму для изменения данных
                this.WindowState = FormWindowState.Normal;
                this.notifyIcon1.Visible = false;
                this.ShowInTaskbar = true;
                this.Show();

            }
            else
            {
                //Иначе мы передаем данные из файлов в переменые
                if (!int.TryParse(File.ReadAllText("PORT_serv.txt"), out PORT_serv))
                {
                    //вызываем форму для изменения
                    this.WindowState = FormWindowState.Normal;
                    this.notifyIcon1.Visible = false;
                    this.ShowInTaskbar = true;
                    this.Show();

                }
                else
                {
                    // если есть все то создаем подключение
                    IP_serv = File.ReadAllText("IP_SERV.txt");
                    NICK_client = File.ReadAllText("NICK_client.txt");
                    IP_SERVER_BOX.Text = IP_serv;
                    PORT_SERVER_BOX.Text = PORT_serv.ToString();
                    NICK_CLIENT_BOX.Text = NICK_client;
                    ClientAct = new LanClientacts(IP_serv, PORT_serv, NICK_client);
                    ClientAct.StartClient();
                    if (notifyEnable)
                    {
                        notifyIcon1.ShowBalloonTip(10, "Lan State PC CLIENT", "Клиент запущен в фоне", ToolTipIcon.Info);
                        //notifyEnable = false;
                    }
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
            ClientAct = new LanClientacts(IP_serv, PORT_serv, NICK_client);
            ClientAct.StartClient();
            if (notifyEnable)
            {
                notifyIcon1.ShowBalloonTip(10, "Lan State PC CLIENT", "Клиент запущен в фоне", ToolTipIcon.Info);
                //notifyEnable = false;
            }


        }
        private void SaveClientData(object sender, EventArgs e)
        {
            // Записываем данные из txt
            File.WriteAllText("IP_SERV.txt", IP_serv);
            File.WriteAllText("PORT_serv.txt", PORT_serv.ToString());
            File.WriteAllText("NICK_client.txt", NICK_client);
            File.WriteAllText("notify.txt", notifyEnable.ToString());
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // при нажатии на приложение в трее открывается меню
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            // и останавливается клиенская часть
            ClientAct.StopClient();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_app form_about = new About_app();
            form_about.ShowDialog();
        }

        private void IP_SERVER_BOX_TextChanged(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientAct.StopClient();
            this.Close();
        }

        private void отключитьУведомленияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (notifyEnable)
            {
                notifyEnable = false;
                this.отключитьУведомленияToolStripMenuItem.Text = "Включить уведомления";
            }
            else
            {
                notifyEnable = true;
                this.отключитьУведомленияToolStripMenuItem.Text = "Отключить уведомления";
            }
        }

        
        //метод для проверки есть ли автозагрузка
        private bool GetAutoRun()
        {
            string AutoRun_patch = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string name_link = Path.Combine(AutoRun_patch,Application.ProductName + ".lnk");
            return false;
        }
    }
}
