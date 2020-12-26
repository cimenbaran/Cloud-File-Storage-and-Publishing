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
            this.button_copy = new System.Windows.Forms.Button();
            this.textBox_copy = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 111);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Port:";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(149, 58);
            this.textBox_ip.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(132, 22);
            this.textBox_ip.TabIndex = 1;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(149, 102);
            this.textBox_port.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(132, 22);
            this.textBox_port.TabIndex = 2;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(149, 188);
            this.button_connect.Margin = new System.Windows.Forms.Padding(4);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(100, 28);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(407, 58);
            this.logs.Margin = new System.Windows.Forms.Padding(4);
            this.logs.Name = "logs";
            this.logs.ReadOnly = true;
            this.logs.Size = new System.Drawing.Size(599, 403);
            this.logs.TabIndex = 6;
            this.logs.Text = "";
            // 
            // textBox_userName
            // 
            this.textBox_userName.Location = new System.Drawing.Point(149, 145);
            this.textBox_userName.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.Size = new System.Drawing.Size(132, 22);
            this.textBox_userName.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 149);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Username:";
            // 
            // uploadFile
            // 
            this.uploadFile.Enabled = false;
            this.uploadFile.Location = new System.Drawing.Point(145, 282);
            this.uploadFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uploadFile.Name = "uploadFile";
            this.uploadFile.Size = new System.Drawing.Size(147, 28);
            this.uploadFile.TabIndex = 5;
            this.uploadFile.Text = "Upload File";
            this.uploadFile.UseVisualStyleBackColor = true;
            this.uploadFile.Click += new System.EventHandler(this.uploadFile_Click);
            // 
            // button_disconnect
            // 
            this.button_disconnect.Enabled = false;
            this.button_disconnect.Location = new System.Drawing.Point(152, 231);
            this.button_disconnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(96, 27);
            this.button_disconnect.TabIndex = 9;
            this.button_disconnect.Text = "Disconnect";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.button_disconnect_Click);
            // 
            // button_download
            // 
            this.button_download.Enabled = false;
            this.button_download.Location = new System.Drawing.Point(281, 433);
            this.button_download.Margin = new System.Windows.Forms.Padding(4);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(100, 28);
            this.button_download.TabIndex = 10;
            this.button_download.Text = "Download";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_download_Click);
            // 
            // textBox_download
            // 
            this.textBox_download.Enabled = false;
            this.textBox_download.Location = new System.Drawing.Point(281, 401);
            this.textBox_download.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_download.Name = "textBox_download";
            this.textBox_download.Size = new System.Drawing.Size(99, 22);
            this.textBox_download.TabIndex = 11;
            // 
            // button_list
            // 
            this.button_list.Enabled = false;
            this.button_list.Location = new System.Drawing.Point(13, 366);
            this.button_list.Margin = new System.Windows.Forms.Padding(4);
            this.button_list.Name = "button_list";
            this.button_list.Size = new System.Drawing.Size(171, 28);
            this.button_list.TabIndex = 12;
            this.button_list.Text = "Get the Filelist You Own";
            this.button_list.UseVisualStyleBackColor = true;
            this.button_list.Click += new System.EventHandler(this.button_list_Click);
            // 
            // button_makepublic
            // 
            this.button_makepublic.Enabled = false;
            this.button_makepublic.Location = new System.Drawing.Point(13, 433);
            this.button_makepublic.Margin = new System.Windows.Forms.Padding(4);
            this.button_makepublic.Name = "button_makepublic";
            this.button_makepublic.Size = new System.Drawing.Size(100, 28);
            this.button_makepublic.TabIndex = 13;
            this.button_makepublic.Text = "Make Public";
            this.button_makepublic.UseVisualStyleBackColor = true;
            this.button_makepublic.Click += new System.EventHandler(this.button_makepublic_Click);
            // 
            // textBox_toPublic
            // 
            this.textBox_toPublic.Enabled = false;
            this.textBox_toPublic.Location = new System.Drawing.Point(13, 401);
            this.textBox_toPublic.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_toPublic.Name = "textBox_toPublic";
            this.textBox_toPublic.Size = new System.Drawing.Size(99, 22);
            this.textBox_toPublic.TabIndex = 14;
            // 
            // button_publiclist
            // 
            this.button_publiclist.Enabled = false;
            this.button_publiclist.Location = new System.Drawing.Point(227, 366);
            this.button_publiclist.Margin = new System.Windows.Forms.Padding(4);
            this.button_publiclist.Name = "button_publiclist";
            this.button_publiclist.Size = new System.Drawing.Size(155, 28);
            this.button_publiclist.TabIndex = 15;
            this.button_publiclist.Text = "Get the Public Filelist";
            this.button_publiclist.UseVisualStyleBackColor = true;
            this.button_publiclist.Click += new System.EventHandler(this.button_publiclist_Click);
            // 
            // button_copy
            // 
            this.button_copy.Enabled = false;
            this.button_copy.Location = new System.Drawing.Point(120, 434);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(86, 27);
            this.button_copy.TabIndex = 16;
            this.button_copy.Text = "Copy a file";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.Click += new System.EventHandler(this.button_copy_Click);
            // 
            // textBox_copy
            // 
            this.textBox_copy.Enabled = false;
            this.textBox_copy.Location = new System.Drawing.Point(120, 401);
            this.textBox_copy.Name = "textBox_copy";
            this.textBox_copy.Size = new System.Drawing.Size(87, 22);
            this.textBox_copy.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 481);
            this.Controls.Add(this.textBox_copy);
            this.Controls.Add(this.button_copy);
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
            this.Margin = new System.Windows.Forms.Padding(4);
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
        private System.Windows.Forms.Button button_copy;
        private System.Windows.Forms.TextBox textBox_copy;
    }
}

