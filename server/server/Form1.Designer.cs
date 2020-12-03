namespace server
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
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_listen = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.choose_db = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 94);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            // 
            // textBox_port
            // 
            this.textBox_port.Enabled = false;
            this.textBox_port.Location = new System.Drawing.Point(180, 90);
            this.textBox_port.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(132, 22);
            this.textBox_port.TabIndex = 1;
            // 
            // button_listen
            // 
            this.button_listen.Enabled = false;
            this.button_listen.Location = new System.Drawing.Point(456, 87);
            this.button_listen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_listen.Name = "button_listen";
            this.button_listen.Size = new System.Drawing.Size(100, 28);
            this.button_listen.TabIndex = 2;
            this.button_listen.Text = "Listen";
            this.button_listen.UseVisualStyleBackColor = true;
            this.button_listen.Click += new System.EventHandler(this.button_listen_Click);
            // 
            // logs
            // 
            this.logs.EnableAutoDragDrop = true;
            this.logs.Location = new System.Drawing.Point(108, 139);
            this.logs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.logs.Name = "logs";
            this.logs.ReadOnly = true;
            this.logs.Size = new System.Drawing.Size(782, 206);
            this.logs.TabIndex = 3;
            this.logs.Text = "";
            // 
            // choose_db
            // 
            this.choose_db.Location = new System.Drawing.Point(180, 37);
            this.choose_db.Name = "choose_db";
            this.choose_db.Size = new System.Drawing.Size(376, 28);
            this.choose_db.TabIndex = 4;
            this.choose_db.Text = "Choose Database Path";
            this.choose_db.UseVisualStyleBackColor = true;
            this.choose_db.Click += new System.EventHandler(this.choose_db_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 382);
            this.Controls.Add(this.choose_db);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_listen);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Button button_listen;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Button choose_db;
    }
}

