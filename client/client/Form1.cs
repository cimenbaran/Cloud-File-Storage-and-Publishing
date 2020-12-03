using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;


        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            // Connecting a Server

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;
            int portNum;
            string userName = textBox_userName.Text;


            if (Int32.TryParse(textBox_port.Text, out portNum))
            {
                try
                {
                    if (userName != "" && userName.Length <= 64)
                    {
                        clientSocket.Connect(IP, portNum);
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(userName);
                        clientSocket.Send(buffer);
                        Byte[] buffer2 = new Byte[64];
                        clientSocket.Receive(buffer2);
                        string incomingMessage = Encoding.Default.GetString(buffer2);
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                        if (incomingMessage == "error_username")
                        {
                            logs.AppendText("This username is already taken! Cannot connect to the server.\n");
                            clientSocket.Close();
                        }
                        else
                        {
                            button_connect.Enabled = false;
                            textBox_ip.Enabled = false;
                            textBox_port.Enabled = false;
                            textBox_userName.Enabled = false;

                            connected = true;
                            logs.AppendText("Connected to the server!\n");
                            uploadFile.Enabled = true;
                            Thread receiveThread = new Thread(Receive);
                            receiveThread.Start();
                        }

                    }
                    else
                    {
                        if (userName == "")
                        {
                            logs.AppendText("Username cannot be empty.\n");
                        }
                        else
                        {
                            logs.AppendText("Username cannot be larger than 64 characters.\n");
                        }
                    }


                }
                catch
                {
                    logs.AppendText("Cannot connect to the server...\n");
                }
            }
            else
            {
                logs.AppendText("Check the port number.\n");
            }
        }

        private void Receive()
        {
            // Receiving a message from the Server
            while (connected)
            {
                try
                {
                    
                    Byte[] buffer = new Byte[128];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    logs.AppendText("Server: " + incomingMessage + "\n");

                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected.\n");
                        button_connect.Enabled = true;
                        textBox_port.Enabled = true;
                        textBox_ip.Enabled = true;
                        uploadFile.Enabled = false;
                        textBox_userName.Enabled = true;

                    }

                    clientSocket.Close();
                    connected = false;

                }
            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }


        private void uploadFile_Click(object sender, EventArgs e)
        {
            // Uploading a file to the Server
            try
            {
                // Select the file
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"; // Taken directly from docs

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // If the file is selected
                    int fileProperties = 256; // FileName + The Data's Length
                    int fileNameLength = 128; // FileName
                    string fileLength = File.ReadAllBytes(dialog.FileName).Length.ToString(); // The Data's Length is turned into string 
                                                                                              // to put into a Byte Array with the FileName

                    Byte[] filePropertiesBuffer = new Byte[fileProperties]; // Allocate space for FileName and The Data's Length

                    // Copy the FileName and The Data's Length into the filePropertiesBuffer
                    Array.Copy(Encoding.Default.GetBytes(dialog.SafeFileName), filePropertiesBuffer, dialog.SafeFileName.Length);
                    Array.Copy( Encoding.ASCII.GetBytes(fileLength),0, filePropertiesBuffer, fileNameLength,fileLength.Length);

                    // Send the filePropertiesBuffer to the Server
                    clientSocket.Send(filePropertiesBuffer);

                    // Copy the data into generalBuffer
                    Byte[] generalBuffer = new Byte[File.ReadAllBytes(dialog.FileName).Length];
                    generalBuffer = File.ReadAllBytes(dialog.FileName);

                    // Send the data to the surver via generalBuffer
                    clientSocket.Send(generalBuffer);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
