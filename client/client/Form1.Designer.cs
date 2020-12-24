namespace client
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.uploadFile = new System.Windows.Forms.Button();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.button_download = new System.Windows.Forms.Button();
            this.textBox_download = new System.Windows.Forms.TextBox();
            this.button_list = new System.Windows.Forms.Button();
            this.button_makepublic = new System.Windows.Forms.Button();
            this.textBox_toPublic = new System.Windows.Forms.TextBox();
            this.button_publiclist = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Port:";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(112, 47);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(100, 20);
            this.textBox_ip.TabIndex = 1;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(112, 83);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(100, 20);
            this.textBox_port.TabIndex = 2;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(112, 153);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 23);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(305, 47);
            this.logs.Name = "logs";
            this.logs.ReadOnly = true;
            this.logs.Size = new System.Drawing.Size(450, 328);
            this.logs.TabIndex = 6;
            this.logs.Text = "";
            // 
            // textBox_userName
            // 
            this.textBox_userName.Location = new System.Drawing.Point(112, 118);
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.Size = new System.Drawing.Size(100, 20);
            this.textBox_userName.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Username:";
            // 
            // uploadFile
            // 
            this.uploadFile.Enabled = false;
            this.uploadFile.Location = new System.Drawing.Point(109, 229);
            this.uploadFile.Margin = new System.Windows.Forms.Padding(2);
            this.uploadFile.Name = "uploadFile";
            this.uploadFile.Size = new System.Drawing.Size(110, 23);
            this.uploadFile.TabIndex = 5;
            this.uploadFile.Text = "Upload File";
            this.uploadFile.UseVisualStyleBackColor = true;
            this.uploadFile.Click += new System.EventHandler(this.uploadFile_Click);
            // 
            // button_disconnect
            // 
            this.button_disconnect.Enabled = false;
            this.button_disconnect.Location = new System.Drawing.Point(114, 188);
            this.button_disconnect.Margin = new System.Windows.Forms.Padding(2);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(72, 22);
            this.button_disconnect.TabIndex = 9;
            this.button_disconnect.Text = "Disconnect";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.button_disconnect_Click);
            // 
            // button_download
            // 
            this.button_download.Enabled = false;
            this.button_download.Location = new System.Drawing.Point(144, 352);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(75, 23);
            this.button_download.TabIndex = 10;
            this.button_download.Text = "Download";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_download_Click);
            // 
            // textBox_download
            // 
            this.textBox_download.Enabled = false;
            this.textBox_download.Location = new System.Drawing.Point(144, 326);
            this.textBox_download.Name = "textBox_download";
            this.textBox_download.Size = new System.Drawing.Size(75, 20);
            this.textBox_download.TabIndex = 11;
            // 
            // button_list
            // 
            this.button_list.Enabled = false;
            this.button_list.Location = new System.Drawing.Point(49, 297);
            this.button_list.Name = "button_list";
            this.button_list.Size = new System.Drawing.Size(75, 23);
            this.button_list.TabIndex = 12;
            this.button_list.Text = "Get List";
            this.button_list.UseVisualStyleBackColor = true;
            this.button_list.Click += new System.EventHandler(this.button_list_Click);
            // 
            // button_makepublic
            // 
            this.button_makepublic.Enabled = false;
            this.button_makepublic.Location = new System.Drawing.Point(49, 352);
            this.button_makepublic.Name = "button_makepublic";
            this.button_makepublic.Size = new System.Drawing.Size(75, 23);
            this.button_makepublic.TabIndex = 13;
            this.button_makepublic.Text = "Make Public";
            this.button_makepublic.UseVisualStyleBackColor = true;
            this.button_makepublic.Click += new System.EventHandler(this.button_makepublic_Click);
            // 
            // textBox_toPublic
            // 
            this.textBox_toPublic.Enabled = false;
            this.textBox_toPublic.Location = new System.Drawing.Point(49, 326);
            this.textBox_toPublic.Name = "textBox_toPublic";
            this.textBox_toPublic.Size = new System.Drawing.Size(75, 20);
            this.textBox_toPublic.TabIndex = 14;
            // 
            // button_publiclist
            // 
            this.button_publiclist.Enabled = false;
            this.button_publiclist.Location = new System.Drawing.Point(144, 297);
            this.button_publiclist.Name = "button_publiclist";
            this.button_publiclist.Size = new System.Drawing.Size(99, 23);
            this.button_publiclist.TabIndex = 15;
            this.button_publiclist.Text = "Get Public List";
            this.button_publiclist.UseVisualStyleBackColor = true;
            this.button_publiclist.Click += new System.EventHandler(this.button_publiclist_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 391);
            this.Controls.Add(this.button_publiclist);
            this.Controls.Add(this.textBox_toPublic);
            this.Controls.Add(this.button_makepublic);
            this.Controls.Add(this.button_list);
            this.Controls.Add(this.textBox_download);
            this.Controls.Add(this.button_download);
            this.Controls.Add(this.button_disconnect);
            this.Controls.Add(this.uploadFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_userName);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button uploadFile;
        private System.Windows.Forms.Button button_disconnect;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.TextBox textBox_download;
        private System.Windows.Forms.Button button_list;
        private System.Windows.Forms.Button button_makepublic;
        private System.Windows.Forms.TextBox textBox_toPublic;
        private System.Windows.Forms.Button button_publiclist;
    }
}

