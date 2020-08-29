namespace FiveInLineServer
{
    partial class IP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextBoxIPAddress = new System.Windows.Forms.TextBox();
            this.Port = new System.Windows.Forms.TextBox();
            this.StartServer = new System.Windows.Forms.Button();
            this.Users = new System.Windows.Forms.ComboBox();
            this.ChatWindow = new System.Windows.Forms.TextBox();
            this.SendMessageBox = new System.Windows.Forms.TextBox();
            this.Send = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxIPAddress
            // 
            this.TextBoxIPAddress.Location = new System.Drawing.Point(4, 10);
            this.TextBoxIPAddress.Name = "TextBoxIPAddress";
            this.TextBoxIPAddress.Size = new System.Drawing.Size(127, 22);
            this.TextBoxIPAddress.TabIndex = 0;
            this.TextBoxIPAddress.Text = "127.0.0.1";
            this.TextBoxIPAddress.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Port
            // 
            this.Port.Location = new System.Drawing.Point(137, 10);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(64, 22);
            this.Port.TabIndex = 1;
            this.Port.Text = "5000";
            this.Port.TextChanged += new System.EventHandler(this.Port_TextChanged);
            // 
            // StartServer
            // 
            this.StartServer.Location = new System.Drawing.Point(207, 10);
            this.StartServer.Name = "StartServer";
            this.StartServer.Size = new System.Drawing.Size(116, 23);
            this.StartServer.TabIndex = 2;
            this.StartServer.Text = "Start Server";
            this.StartServer.UseVisualStyleBackColor = true;
            this.StartServer.Click += new System.EventHandler(this.StartServer_Click_1);
            // 
            // Users
            // 
            this.Users.FormattingEnabled = true;
            this.Users.Location = new System.Drawing.Point(329, 10);
            this.Users.Name = "Users";
            this.Users.Size = new System.Drawing.Size(165, 24);
            this.Users.TabIndex = 3;
            // 
            // ChatWindow
            // 
            this.ChatWindow.Location = new System.Drawing.Point(4, 59);
            this.ChatWindow.Multiline = true;
            this.ChatWindow.Name = "ChatWindow";
            this.ChatWindow.Size = new System.Drawing.Size(490, 215);
            this.ChatWindow.TabIndex = 4;
            this.ChatWindow.TextChanged += new System.EventHandler(this.ChatWindow_TextChanged);
            // 
            // SendMessageBox
            // 
            this.SendMessageBox.Location = new System.Drawing.Point(4, 280);
            this.SendMessageBox.Multiline = true;
            this.SendMessageBox.Name = "SendMessageBox";
            this.SendMessageBox.Size = new System.Drawing.Size(490, 101);
            this.SendMessageBox.TabIndex = 5;
            // 
            // Send
            // 
            this.Send.Location = new System.Drawing.Point(419, 387);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(75, 23);
            this.Send.TabIndex = 6;
            this.Send.Text = "Send";
            this.Send.UseVisualStyleBackColor = true;
            this.Send.Click += new System.EventHandler(this.Send_Click_1);
            // 
            // IP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 418);
            this.Controls.Add(this.Send);
            this.Controls.Add(this.SendMessageBox);
            this.Controls.Add(this.ChatWindow);
            this.Controls.Add(this.Users);
            this.Controls.Add(this.StartServer);
            this.Controls.Add(this.Port);
            this.Controls.Add(this.TextBoxIPAddress);
            this.Name = "IP";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.FormServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxIPAddress;
        private System.Windows.Forms.TextBox Port;
        private System.Windows.Forms.Button StartServer;
        private System.Windows.Forms.ComboBox Users;
        private System.Windows.Forms.TextBox ChatWindow;
        private System.Windows.Forms.TextBox SendMessageBox;
        private System.Windows.Forms.Button Send;
    }
}

