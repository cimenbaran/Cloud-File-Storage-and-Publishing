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


        // The headers explanation
        // -----------------------
        //      Client --> Server
        //      0 -> Sending a file to Server
        //      1 ->
        //      2 ->
        //      3 ->
        //      4 ->
        //      5 ->
        //      6 ->
        //      7 ->
        // -----------------------
        //      Server --> Client
        //      0 -> Message to Client
        //      1 ->
        //      2 ->
        //      3 ->
        //      4 ->
        //      5 ->
        //      6 ->
        //      7 ->
        // -----------------------
        //
        // Server'dan Client'a Recieve işlemi yapacaksanız
        // kodunuzu Receive() fonksiyonun içindeki if'lerden birine yazın.
        // Yukarıdaki tabloya hangi kodu kullandığınızı yazın
        // Aynısını Server Projesinde de doldurun.
        // -----------------------
        //
        // Client'tan Server'a Send işlemi yapmadan önce
        // aşağıdaki kodu kopyalayın ve ilk olarak onu yollayın.
        // xxxxx olan yere, yukardaki boş işlemlerden sayılardan birini yazın
        // Yukarıdaki tabloya hangi kodu kullandığınızı yazın
        // Aynısını Server Projesinde de doldurun.

        // -----------------------
        // Send the 1 byte to inform the server that the client is sending a file
        // Byte[] infoHeader = new Byte[1];
        // infoHeader[0] = xxxxx;
        // clientSocket.Send(infoHeader);
        // -----------------------
        //
        //
        //




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
                        // Receive the operation information
                        Byte[] receivedInfoHeader = new Byte[1];
                        clientSocket.Receive(receivedInfoHeader);

                        if (receivedInfoHeader[0] == 0)
                        {
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
                                button_disconnect.Enabled = true;
                                Thread receiveThread = new Thread(Receive);
                                receiveThread.Start();
                            }
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
                    // Receive the operation information
                    Byte[] receivedInfoHeader = new Byte[1];
                    clientSocket.Receive(receivedInfoHeader);

                    if (receivedInfoHeader[0] == 0)
                    {
                        Byte[] buffer = new Byte[128];
                        clientSocket.Receive(buffer);

                        string incomingMessage = Encoding.Default.GetString(buffer);
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                        logs.AppendText("Server: " + incomingMessage + "\n");
                    }

                    if (receivedInfoHeader[0] == 1) { }

                    if (receivedInfoHeader[0] == 2) { }

                    if (receivedInfoHeader[0] == 3) { }

                    if (receivedInfoHeader[0] == 4) { }

                    if (receivedInfoHeader[0] == 5) { }

                    if (receivedInfoHeader[0] == 6) { }

                    if (receivedInfoHeader[0] == 7) { }
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

                    // Send the 1 byte to inform the server that the client is sending a file
                    Byte[] infoHeader = new Byte[1];
                    infoHeader[0] = 0;
                    clientSocket.Send(infoHeader);

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
                    logs.AppendText("Sent file: \"" + dialog.SafeFileName + "\" \n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            string user_Name = textBox_userName.Text;
            logs.AppendText( user_Name + "  has disconnected.\n");
            clientSocket.Close();
            connected = false;
            terminating = true;
            
            button_disconnect.Enabled = false;
            button_connect.Enabled = true;

            textBox_port.Enabled = true;
            textBox_port.Text = String.Empty;

            textBox_ip.Enabled = true;
            textBox_ip.Text = String.Empty;

            textBox_userName.Enabled = true;
            textBox_userName.Text = String.Empty;

            uploadFile.Enabled = false;
            textBox_userName.Enabled = true;


        }
    }
}
