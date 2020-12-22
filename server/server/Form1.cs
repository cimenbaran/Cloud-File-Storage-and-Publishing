using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Form1 : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Storing the Clients and Usernames
        List<Socket> clientSockets = new List<Socket>();
        List<String> clientUsernames = new List<String>();


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
        // Client'tan Server'a Recieve işlemi yapacaksanız
        // kodunuzu Receive() fonksiyonun içindeki if'lerden birine yazın.
        // Yukarıdaki tabloya hangi kodu kullandığınızı yazın
        // Aynısını Client Projesinde de doldurun.
        // -----------------------
        //
        // Server'dan Client'a Send işlemi yapmadan önce
        // aşağıdaki kodu kopyalayın ve ilk olarak onu yollayın.
        // xxxxx olan yere, yukardaki boş işlemlerden sayılardan birini yazın
        // Yukarıdaki tabloya hangi kodu kullandığınızı yazın
        // Aynısını Client Projesinde de doldurun.

        // -----------------------
        // Send the 1 byte to inform the server that the client is sending a file
        // Byte[] infoHeader = new Byte[1];
        // infoHeader[0] = xxxxx;
        // thisClient.Send(infoHeader);
        // -----------------------
        //
        //
        //

        string DB_Path = "";
        string LOGS_Path = "";

        bool terminating = false;
        bool listening = false;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }



        private void button_listen_Click(object sender, EventArgs e)
        {
            // Listening to a port

            int serverPort;
            if (Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(200);
                listening = true;
                textBox_port.Enabled = false;
                button_listen.Enabled = false;

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();
                logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                logs.AppendText("Please enter a valid port number.\n");
            }
        }

        private void Accept()
        {   // Accepting a Client
            while (listening)
            {
                try
                {
                    
                    Socket newClient = serverSocket.Accept();
                    Byte[] buffer = new Byte[64];
                    newClient.Receive(buffer);
                    string incomingUsername = Encoding.Default.GetString(buffer);
                    incomingUsername = incomingUsername.Substring(0, incomingUsername.IndexOf("\0"));
                    if (clientUsernames.Exists(element => element == incomingUsername))
                    {

                        // Telling the Client, server is sending a message
                        Byte[] infoHeader = new Byte[1];
                        infoHeader[0] = 0;
                        newClient.Send(infoHeader);


                        logs.AppendText("The username \"" + incomingUsername + "\" is already taken! Cannot connect to the server.\n");
                        string errorUsername = "error_username";
                        Byte[] buffer2 = Encoding.Default.GetBytes(errorUsername);
                        
                        

                        newClient.Send(buffer2);
                        newClient.Close();
                    }
                    else
                    {

                        // Telling the Client, server is sending a message
                        Byte[] infoHeader = new Byte[1];
                        infoHeader[0] = 0;
                        newClient.Send(infoHeader);


                        clientSockets.Add(newClient);
                        string noError = "All OK.";
                        Byte[] buffer2 = Encoding.Default.GetBytes(noError);

                        

                        newClient.Send(buffer2);
                        clientUsernames.Add(incomingUsername);
                        logs.AppendText("\"" + incomingUsername + "\" is connected.\n");
                        Thread receiveThread = new Thread(() => Receive(newClient, incomingUsername));
                        receiveThread.Start();
                    }

                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }
                }
            }
        }

        private void Receive(Socket thisClient, string clientUsername)
        {   
            
            bool connected = true;
            while (connected && !terminating)
            {

                    

                try
                {
                    // Receive the operation information
                    Byte[] receivedInfoHeader = new Byte[1];
                    thisClient.Receive(receivedInfoHeader);

                    if (receivedInfoHeader[0] == 0)
                    {
                        // Receiving a file from a particular Client


                        // Receive the incoming File's name and size
                        Byte[] fileProperties = new byte[256]; // First 128 Bytes are for Name, Last 128 for Size
                        thisClient.Receive(fileProperties); // Receive the Buffer

                        // Take the file name from the buffer
                        string fileName = Encoding.Default.GetString(fileProperties.Take(128).ToArray());

                        // Take the file size from buffer
                        int fileSize = Int32.Parse(Encoding.Default.GetString(fileProperties.Skip(128).Take(128).ToArray()));

                        // Get the file data
                        Byte[] buffer = new Byte[fileSize]; // The buffer size is allocated by the file size
                        thisClient.Receive(buffer);

                        // Format the file name
                        fileName = fileName.Substring(0, fileName.IndexOf("\0"));
                        fileName = clientUsername + "_" + fileName;
                        string fileName_basic = fileName;




                        // Read the LOGS.txt file to determine the final file name
                        StreamReader logReader = new StreamReader(LOGS_Path);
                        string line = "";
                        bool flag = false;
                        bool flag2 = false;
                        int count = 0;
                        while ((line = logReader.ReadLine()) != null)
                        {
                            if (!(line.Split('\t')[0] == clientUsername))
                                continue;

                            if (count != 0)
                            {
                                fileName = fileName_basic.Split('.')[fileName_basic.Split('.').Length - 2] + "(" + count.ToString() + ")." + fileName_basic.Split('.').Last();
                                flag = true;
                            }

                            if (line.Split('\t')[1].Split('_')[0] + "_" + line.Split('\t')[1].Split('_')[1] == fileName)
                            {

                                count = count + 1;
                                flag2 = true;
                            }
                        }


                        if (!flag && flag2)
                        {
                            fileName_basic = fileName_basic.Split('.')[fileName_basic.Split('.').Length - 2] + "(" + count.ToString() + ")." + fileName_basic.Split('.').Last();
                        }
                        if (flag)
                        {
                            fileName_basic = fileName_basic.Split('.')[fileName_basic.Split('.').Length - 2] + "(" + count.ToString() + ")." + fileName_basic.Split('.').Last();
                        }
                        logReader.Close();

                        // Create the file and write into it
                        BinaryWriter bWrite = new BinaryWriter(File.Open // using system.I/O
                                (DB_Path + "/" + fileName_basic, FileMode.Append));
                        bWrite.Write(buffer);
                        bWrite.Close();

                        // Write into LOGS.txt
                        BinaryWriter bWriteLog = new BinaryWriter(File.Open(LOGS_Path, FileMode.Append));
                        Byte[] logBuffer = Encoding.Default.GetBytes(clientUsername + "\t" + fileName_basic + "\n");
                        bWriteLog.Write(logBuffer.ToArray());
                        bWriteLog.Close();

                        buffer = null; // In order to prevent creating files over and over again

                        // Print the logs and send the confirmation message to the Client
                        logs.AppendText("Received file: \"" + fileName_basic + "\" from: \"" + clientUsername + "\"\n"); // Log message
                        string receivedFile = "Uploaded file: \"" + fileName_basic + "\" from: \"" + clientUsername + "\" successfully.." + "\n";
                        Byte[] buffer2 = new Byte[64];
                        buffer2 = Encoding.Default.GetBytes(receivedFile);
                        thisClient.Send(buffer2);
                    }

                    if (receivedInfoHeader[0] == 1) { }

                    if (receivedInfoHeader[0] == 2) { }

                    if (receivedInfoHeader[0] == 3) { }

                    if (receivedInfoHeader[0] == 4) { }

                    if (receivedInfoHeader[0] == 5) { }

                    if (receivedInfoHeader[0] == 6) { }

                    if (receivedInfoHeader[0] == 7) { }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    if (!terminating)
                    {
                        logs.AppendText("\"" + clientUsername + "\" has disconnected.\n");
                    }

                    clientSockets.Remove(thisClient);
                    clientUsernames.Remove(clientUsername);
                    thisClient.Close();
                    connected = false;
                }
                
            }
        }



        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }


        private void choose_db_Click(object sender, EventArgs e)
        {
            // Choose the Database folder
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string folderPath = fbd.SelectedPath;
                    textBox_port.Enabled = true;
                    button_listen.Enabled = true;
                    DB_Path = folderPath;
                    string lgp = folderPath.Substring(0, folderPath.LastIndexOf('\\')) + "\\LOGS.txt";


                    StreamWriter w = File.AppendText(lgp); // "CREATE IF LOGS TXT DOES NOT EXIST
                    w.Close();

                    DB_Path = folderPath.Replace(@"\", "/"); // DO NOT CHANGE PATH CORRECTION
                    LOGS_Path = lgp.Replace(@"\", "/");      // DO NOT CHANGE PATH CORRECTION

                    logs.AppendText("You choosed: " + DB_Path + " as db path.\n");
                    logs.AppendText("LOGS path is: " + LOGS_Path + "\n");
                }
            }

        }
    }
}
