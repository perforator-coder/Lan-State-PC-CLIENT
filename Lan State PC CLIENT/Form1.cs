

using Microsoft.Win32;

namespace Lan_State_PC_CLIENT
{
    public partial class Form1 : Form
    {
        private string IP_serv;
        private int PORT_serv;
        private string NICK_client;
        private LanClientacts ClientAct;
        private bool notifyEnable = true;
        private string app_name = Application.ProductName;
        private string app_start_patch = Application.ExecutablePath;
        private string appFolder = AppDomain.CurrentDomain.BaseDirectory;
        public Form1()
        {
            InitializeComponent();
            //скрываем форму
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.notifyIcon1.Icon = this.Icon;
            this.notifyIcon1.Visible = true;
            // проверяем автозагрузку
            if (appInAutoStart())
            {
                добавитьВАвтозагрузкуToolStripMenuItem.Text = "Убрать из автозагрузки";
            }
            else 
            {
                добавитьВАвтозагрузкуToolStripMenuItem.Text = "Добавить в автозагрузку";
            }
            /// Проверяем есть ли файлы конфиги
            if (File.Exists(appFolder + "notify.txt") && bool.TryParse(File.ReadAllText(appFolder + "notify.txt"), out notifyEnable))
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
            if (!File.Exists(appFolder + "IP_SERV.txt") || !File.Exists(appFolder + "PORT_serv.txt") || !File.Exists(appFolder + "NICK_client.txt"))
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
                if (!int.TryParse(File.ReadAllText(appFolder + "PORT_serv.txt"), out PORT_serv))
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
                    IP_serv = File.ReadAllText(appFolder + "IP_SERV.txt");
                    NICK_client = File.ReadAllText(appFolder + "NICK_client.txt");
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
            File.WriteAllText(appFolder + "IP_SERV.txt", IP_serv);
            File.WriteAllText(appFolder + "PORT_serv.txt", PORT_serv.ToString());
            File.WriteAllText(appFolder + "NICK_client.txt", NICK_client);
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
            File.WriteAllText(appFolder + "IP_SERV.txt", IP_serv);
            File.WriteAllText(appFolder + "PORT_serv.txt", PORT_serv.ToString());
            File.WriteAllText(appFolder + "NICK_client.txt", NICK_client);
            File.WriteAllText(appFolder + "notify.txt", notifyEnable.ToString());
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

        private void добавитьВАвтозагрузкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(RegistryKey Reg_key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run",true))
            {
                if (!appInAutoStart())
                {
                    Reg_key.SetValue(app_name, app_start_patch);
                    добавитьВАвтозагрузкуToolStripMenuItem.Text = "Убрать из автозагрузки";
                }
                else 
                {
                    Reg_key.DeleteValue(app_name,false);
                    добавитьВАвтозагрузкуToolStripMenuItem.Text = "Добавить в автозагрузку";
                }
            }

        }


        //метод для проверки есть ли автозагрузка
        private bool appInAutoStart()
        {
            using(RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false))
            {
                if (key?.GetValue(app_name) == null)
                {
                    return false;
                }
                else 
                {
                    return true;
                }
            }
        }

    }
}
