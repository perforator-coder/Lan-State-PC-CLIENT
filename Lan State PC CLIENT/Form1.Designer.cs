namespace Lan_State_PC_CLIENT
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            оПрограммеToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            IP_SERVER_BOX = new TextBox();
            PORT_SERVER_BOX = new TextBox();
            NICK_CLIENT_BOX = new TextBox();
            SAVE_PAM = new Button();
            notifyIcon1 = new NotifyIcon(components);
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { оПрограммеToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(268, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // оПрограммеToolStripMenuItem
            // 
            оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            оПрограммеToolStripMenuItem.Size = new Size(94, 20);
            оПрограммеToolStripMenuItem.Text = "О программе";
            оПрограммеToolStripMenuItem.Click += оПрограммеToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 124);
            label1.Name = "label1";
            label1.Size = new Size(81, 15);
            label1.TabIndex = 1;
            label1.Text = "Имя клиента:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 94);
            label2.Name = "label2";
            label2.Size = new Size(87, 15);
            label2.TabIndex = 2;
            label2.Text = "Порт Сервера:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 63);
            label3.Name = "label3";
            label3.Size = new Size(69, 15);
            label3.TabIndex = 3;
            label3.Text = "IP Сервера:";
            // 
            // IP_SERVER_BOX
            // 
            IP_SERVER_BOX.Location = new Point(99, 60);
            IP_SERVER_BOX.Name = "IP_SERVER_BOX";
            IP_SERVER_BOX.Size = new Size(100, 23);
            IP_SERVER_BOX.TabIndex = 4;
            // 
            // PORT_SERVER_BOX
            // 
            PORT_SERVER_BOX.Location = new Point(99, 91);
            PORT_SERVER_BOX.Name = "PORT_SERVER_BOX";
            PORT_SERVER_BOX.Size = new Size(100, 23);
            PORT_SERVER_BOX.TabIndex = 5;
            // 
            // NICK_CLIENT_BOX
            // 
            NICK_CLIENT_BOX.Location = new Point(99, 121);
            NICK_CLIENT_BOX.Name = "NICK_CLIENT_BOX";
            NICK_CLIENT_BOX.Size = new Size(100, 23);
            NICK_CLIENT_BOX.TabIndex = 6;
            // 
            // SAVE_PAM
            // 
            SAVE_PAM.Location = new Point(42, 157);
            SAVE_PAM.Name = "SAVE_PAM";
            SAVE_PAM.Size = new Size(179, 23);
            SAVE_PAM.TabIndex = 7;
            SAVE_PAM.Text = "Сохранить и запустить ";
            SAVE_PAM.UseVisualStyleBackColor = true;
            SAVE_PAM.Click += SAVE_PAM_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipText = "Программа работает в трее.\r\nКлиент запущен...";
            notifyIcon1.BalloonTipTitle = "Lan State PC CLIENT";
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Lan State PC CLIENT";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(268, 192);
            Controls.Add(SAVE_PAM);
            Controls.Add(NICK_CLIENT_BOX);
            Controls.Add(PORT_SERVER_BOX);
            Controls.Add(IP_SERVER_BOX);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "Lan State PC CLIENT";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem оПрограммеToolStripMenuItem;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox IP_SERVER_BOX;
        private TextBox PORT_SERVER_BOX;
        private TextBox NICK_CLIENT_BOX;
        private Button SAVE_PAM;
        private NotifyIcon notifyIcon1;
    }
}
