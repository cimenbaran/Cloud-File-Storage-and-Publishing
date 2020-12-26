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
        string downloadpath = "";
        List<String> list = new List<String>();
        List<String> publiclist = new List<String>();
        string username = "";
        string copypath = "";

        // The headers explanation
        // -----------------------
        //      Client --> Server
        //      0 -> Sending a file to Server
        //      1 -> Make file public
        //      2 ->
        //      3 ->
        //      4 -> Downloading file from the Server (Download)
        //      5 -> Getting private list from Server (Get Private List)
        //      6 -> Getting public list from Server (Get Public List)
        //      7 ->
        // -----------------------
        //      Server --> Client
        //      0 -> Message to Client
        //      1 ->
        //      2 ->
        //      3 ->
        //      4 -> Sending file to Client (Download)
        //      5 -> Sending private files list to Client(Get Private List)
        //      6 -> Sending public files list to Client(Get Public List)
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
            username = userName;

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
                                button_list.Enabled = true;
                                button_publiclist.Enabled = true;

                                connected = true;
                                logs.AppendText("Connected to the server!\n\n");
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

                        logs.AppendText("Server: " + incomingMessage + "\n\n");
                    }

                    if (receivedInfoHeader[0] == 1) { }

                    if (receivedInfoHeader[0] == 2) { }

                    if (receivedInfoHeader[0] == 3)
                    {
                        Byte[] buffer = new Byte[64];
                        clientSocket.Receive(buffer);

                        string incomingMessage = Encoding.Default.GetString(buffer);
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                        logs.AppendText("Server: " + incomingMessage + "\n\n");
                    }

                    if (receivedInfoHeader[0] == 4)
                    {                     

                        // Receive the incoming File's name and size
                        Byte[] fileProperties = new byte[256]; // First 128 Bytes are for Name, Last 128 for Size
                        clientSocket.Receive(fileProperties); // Receive the Buffer

                        // Take the file name from the buffer
                        string fileName = Encoding.Default.GetString(fileProperties.Take(128).ToArray());
                        fileName = fileName.Substring(0, fileName.IndexOf("\0"));

                        // Take the file size from buffer
                        int fileSize = Int32.Parse(Encoding.Default.GetString(fileProperties.Skip(128).Take(128).ToArray()));
                        string filename_pre = fileName;
                        // Get the file data
                        Byte[] buffer2 = new Byte[fileSize]; // The buffer size is allocated by the file size
                        clientSocket.Receive(buffer2);
                        int count = 1;
                        if (File.Exists(downloadpath + "/" + fileName))
                        {
                            fileName = filename_pre.Split('.')[filename_pre.Split('.').Length - 2] + "(" + count + ")." + filename_pre.Split('.').Last();
                            while (File.Exists(downloadpath + "/" + fileName))
                            {
                                count += 1;
                                fileName = filename_pre.Split('.')[filename_pre.Split('.').Length - 2] + "(" + count + ")." + filename_pre.Split('.').Last();
                            }
                            
                        }
                        BinaryWriter bWrite = new BinaryWriter(File.Open // using system.I/O
                                        (downloadpath + "/" + fileName, FileMode.Append));
                        bWrite.Write(buffer2);
                        bWrite.Close();
                        buffer2 = null; // In order to prevent creating files over and over again

                        // Print the logs and send the confirmation message to the Client
                        logs.AppendText("Downloaded file: \"" + fileName + "\" from: the server." + "\n\n"); // Log message
                    }

                    if (receivedInfoHeader[0] == 5)
                    {
                        Byte[] buffer_size = new Byte[64];
                        clientSocket.Receive(buffer_size);
                        string list_size = Encoding.Default.GetString(buffer_size);
                        list_size = list_size.Substring(0, list_size.IndexOf("\0"));
                        int list_size_int = Int32.Parse(list_size);
                        Byte[] buffer = new Byte[128];
                        for (int i = 0; i <list_size_int; i++)
                        {
                            clientSocket.Receive(buffer);
                            string incomingList = Encoding.Default.GetString(buffer);
                            incomingList = incomingList.Substring(0, incomingList.IndexOf("\0"));
                            string deneme = incomingList;
                            incomingList = deneme.Split('\t')[0];
                            incomingList += '\t' + deneme.Split('\t')[1];
                            incomingList += '\t' + deneme.Split('\t')[2].Substring(0, 16);

                            list.Add(incomingList);
                        }
                        logs.AppendText("You can download, delete, copy or make public the following files.\n");
                        for (int i = 0; i < list.Count ; i++)
                        {
                            logs.AppendText((i + 1).ToString() + ". " + list[i] + "\n");
                        }

                        
                    }

                    if (receivedInfoHeader[0] == 6)
                    {
                        Byte[] buffer_size = new Byte[64];
                        clientSocket.Receive(buffer_size);
                        string list_size = Encoding.Default.GetString(buffer_size);
                        list_size = list_size.Substring(0, list_size.IndexOf("\0"));
                        int list_size_int = Int32.Parse(list_size);
                        Byte[] buffer = new Byte[128];
                        for (int i = 0; i < list_size_int; i++)
                        {
                            clientSocket.Receive(buffer);
                            string incomingList = Encoding.Default.GetString(buffer);
                            incomingList = incomingList.Substring(0, incomingList.IndexOf("\0"));
                            string deneme = incomingList;
                            incomingList = deneme.Split('\t')[0];
                            incomingList += '\t' + deneme.Split('\t')[1];
                            incomingList += '\t' + deneme.Split('\t')[2];
                            incomingList += '\t' + deneme.Split('\t')[3].Substring(0, 16);

                            publiclist.Add(incomingList);
                        }
                        logs.AppendText("You can only download the following files that belong to someone else.\n");
                        for (int i = 0; i < publiclist.Count; i++)
                        {
                            logs.AppendText((i+1).ToString()+ ". "+publiclist[i] + "\n");
                        }
                    }

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
                        button_download.Enabled = false;
                        textBox_download.Enabled = false;
                        button_copy.Enabled = false;
                        textBox_copy.Enabled = false;

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
                    Array.Copy(Encoding.ASCII.GetBytes(fileLength), 0, filePropertiesBuffer, fileNameLength, fileLength.Length);

                    // Send the filePropertiesBuffer to the Server
                    clientSocket.Send(filePropertiesBuffer);

                    // Copy the data into generalBuffer
                    Byte[] generalBuffer = new Byte[File.ReadAllBytes(dialog.FileName).Length];
                    generalBuffer = File.ReadAllBytes(dialog.FileName);

                    // Send the data to the surver via generalBuffer
                    clientSocket.Send(generalBuffer);
                    logs.AppendText("Sent file: \"" + dialog.SafeFileName + "\" \n\n");

                    button_list.Enabled = true;
                    button_download.Enabled = false;
                    button_copy.Enabled = false;
                    textBox_download.Enabled = false;
                    button_makepublic.Enabled = false;
                    textBox_toPublic.Enabled = false;
                    textBox_copy.Enabled = false;
                    textBox_download.Text = String.Empty;
                    textBox_toPublic.Text = String.Empty;
                    textBox_copy.Text = String.Empty;
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
            logs.AppendText(user_Name + "  has disconnected.\n");
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

            textBox_download.Enabled = false;
            textBox_download.Text = String.Empty;
            textBox_toPublic.Enabled = false;
            textBox_toPublic.Text = String.Empty;

            uploadFile.Enabled = false;
            button_download.Enabled = false;
            button_list.Enabled = false;
            button_makepublic.Enabled = false;
            button_publiclist.Enabled = false;

            button_copy.Enabled = false;
            textBox_copy.Text = String.Empty;
            textBox_copy.Enabled = false;


        }

        private void button_download_Click(object sender, EventArgs e)
        {
            string filename = textBox_download.Text;
            ///////////////////////
            // if in icine && den sonra list in icinde var mi yok mu kontrolu olcak 
            // string list = "";
            // list icinde bosluklarla ayrilabilir txt isimleri 
            // bi global list tutulur burda
            bool flag = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (filename == list[i].Split('\t')[0])
                {
                    flag = true;
                    filename = username + "_" + filename;
                    break;
                }
            }
            if (!flag)
            {
                for (int i = 0; i < publiclist.Count; i++)
                {
                    if (filename == publiclist[i].Split('\t')[1])
                    {
                        flag = true;
                        filename = publiclist[i].Split('\t')[0] + "_" + filename;
                        break;
                    }
                }
            }
            if (filename == "" || !flag)
            {
                logs.AppendText("Wrong input for downloading... You must enter a filename from the given lists.\n");
            }
            else
            {
                Byte[] infoHeader = new Byte[1];
                infoHeader[0] = 4;
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string folderPath = fbd.SelectedPath;
                        downloadpath = folderPath;
                        downloadpath = folderPath.Replace(@"\", "/"); // DO NOT CHANGE PATH CORRECTION
                    }
                }
                clientSocket.Send(infoHeader);
                Byte[] buffer = new Byte[128];
                buffer = Encoding.Default.GetBytes(filename);
                clientSocket.Send(buffer);
                textBox_download.Clear();

            }
            
        }

        private void button_list_Click(object sender, EventArgs e)
        {
            list.Clear();
            button_makepublic.Enabled = true;
            textBox_toPublic.Enabled = true;
            button_download.Enabled = true;
            textBox_download.Enabled = true;
            button_copy.Enabled = true;
            textBox_copy.Enabled = true;

            Byte[] infoHeader = new Byte[1];
            infoHeader[0] = 5;
            clientSocket.Send(infoHeader);
        }

        private void button_makepublic_Click(object sender, EventArgs e)
        {
            try
            {
                string fileNameToPublic = textBox_toPublic.Text;
                Byte[] bufferToPublic = new Byte[128];
                if (fileNameToPublic == "" || fileNameToPublic.Length >= 128)
                {
                    logs.AppendText("Enter a file name less than 64 character");
                    return;
                }

                // Send the 1 byte to inform the server that the client is sending a file
                Byte[] infoHeader = new Byte[1];
                infoHeader[0] = 1;
                clientSocket.Send(infoHeader);


                bufferToPublic = Encoding.Default.GetBytes(fileNameToPublic);
                clientSocket.Send(bufferToPublic);
                textBox_toPublic.Clear();
                
            }
            catch
            {

            }
        }

        private void button_publiclist_Click(object sender, EventArgs e)
        {
            publiclist.Clear();
            button_download.Enabled = true;
            textBox_download.Enabled = true;


            Byte[] infoHeader = new Byte[1];
            infoHeader[0] = 6;
            clientSocket.Send(infoHeader);
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            string filename = textBox_copy.Text;

            bool flag = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (filename == list[i].Split('\t')[0])
                {
                    flag = true;
                }
            }

            // Check if the file is public - if public make the copy public too
            bool isPublic = false;
            for (int i = 0; i < publiclist.Count; i++)
            {
                if (filename == publiclist[i].Split('\t')[1])
                {
                    isPublic = true;
                }
            }

            if (filename == "" || !flag )
            {
                logs.AppendText("Wrong input for copying... You can only copy a file from the list.\n");
                textBox_copy.Clear();
            }
            else
            {
                Byte[] infoHeader = new Byte[1];
                infoHeader[0] = 3;
                clientSocket.Send(infoHeader);

                // Send 1 if the file is public otherwise send 0
                Byte[] accesRightHeader = new Byte[1];
                if (isPublic)
                    accesRightHeader[0] = 1;
                else
                    accesRightHeader[0] = 0;

                clientSocket.Send(accesRightHeader);


                Byte[] buffer = new Byte[128];
                buffer = Encoding.Default.GetBytes(filename);
                clientSocket.Send(buffer);
                textBox_copy.Clear();

                try
                { 
                    Byte[] buffer2 = new Byte[128];
                    clientSocket.Receive(buffer2);

                    string incomingMessage = Encoding.Default.GetString(buffer2);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    logs.AppendText("Server: " + incomingMessage + "\n\n");

                    button_copy.Enabled = false;
                    textBox_copy.Enabled = false;
                    button_download.Enabled = false;
                    textBox_download.Enabled = false;
                    button_makepublic.Enabled = false;
                    textBox_toPublic.Enabled = false;


                }
                catch
                {
                    logs.AppendText("Server: coppy error \n\n");
                }
            }

            

        }
    }
}