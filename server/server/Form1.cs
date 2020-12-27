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
        //      2 -> Delete file
        //      3 ->
        //      4 -> Downloading file from the Server (Download)
        //      5 -> Getting list from Server (Get List)
        //      6 ->
        //      7 ->
        // -----------------------
        //      Server --> Client
        //      0 -> Message to Client
        //      1 ->
        //      2 ->
        //      3 -> Create a on the server (Copy)
        //      4 -> Sending file to Client (Download)
        //      5 -> Sending list as a string (Get List)
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
                        //StreamReader logReader = new StreamReader(LOGS_Path);
                        //string line = "";
                        int denemecount = 0;
                        bool deneme = false;
                        while (!deneme)
                        {
                            if (File.Exists(DB_Path + "/" + fileName_basic))
                            {
                                denemecount++;
                                fileName_basic = fileName.Split('_')[0] + "_" + fileName.Split('_')[1].Split('.')[0] + "(" + denemecount.ToString() + ")" + ".txt";
                            }
                            else
                            {
                                deneme = true;
                            }
                        }
                        //if (!File.Exists(DB_Path + "/" + fileName_basic))
                        //{

                        //}
                        //else
                        //{
                        //    bool flag = false;
                        //    bool flag2 = false;
                        //    int count = 0;
                        //    while ((line = logReader.ReadLine()) != null)
                        //    {
                        //        if (!(line.Split('\t')[0] == clientUsername))
                        //            continue;
                        //        if (!(line.Split('\t')[2] == "2"))
                        //            continue;

                        //        if (count != 0)
                        //        {
                        //            fileName = fileName_basic.Split('.')[fileName_basic.Split('.').Length - 2] + "(" + count.ToString() + ")." + fileName_basic.Split('.').Last();
                        //            flag = true;
                        //        }

                        //        if (line.Split('\t')[1].Split('_')[0] + "_" + line.Split('\t')[1].Split('_')[1] == fileName)
                        //        {

                        //            count = count + 1;
                        //            flag2 = true;
                        //        }
                        //    }


                        //    if (!flag && flag2)
                        //    {
                        //        fileName_basic = fileName_basic.Split('.')[fileName_basic.Split('.').Length - 2] + "(" + count.ToString() + ")." + fileName_basic.Split('.').Last();
                        //    }
                        //    if (flag)
                        //    {
                        //        fileName_basic = fileName_basic.Split('.')[fileName_basic.Split('.').Length - 2] + "(" + count.ToString() + ")." + fileName_basic.Split('.').Last();
                        //    }
                        //}
                        //logReader.Close();

                        // Create the file and write into it
                        BinaryWriter bWrite = new BinaryWriter(File.Open // using system.I/O
                                (DB_Path + "/" + fileName_basic, FileMode.Append));
                        bWrite.Write(buffer);
                        bWrite.Close();

                        // Write into LOGS.txt
                        string time = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                        BinaryWriter bWriteLog = new BinaryWriter(File.Open(LOGS_Path, FileMode.Append));
                        Byte[] logBuffer = Encoding.Default.GetBytes(clientUsername + "\t" + fileName_basic + "\t" + "0" + "\t"+ fileSize + "bytes" +"\t" + time + "\n");
                        bWriteLog.Write(logBuffer.ToArray());
                        bWriteLog.Close();

                        buffer = null; // In order to prevent creating files over and over again

                        // Print the logs and send the confirmation message to the Client
                        logs.AppendText("Received file: \"" + fileName_basic + "\" from: \"" + clientUsername + "\"\n"); // Log message
                        string receivedFile = "Uploaded file: \"" + fileName_basic + "\" from: \"" + clientUsername + "\" successfully.." + "\n";

                        //Byte[] infoHeader = new Byte[1];
                        //infoHeader[0] = 0;
                        //thisClient.Send(infoHeader);

                        Byte[] buffer2 = new Byte[64];
                        buffer2 = Encoding.Default.GetBytes(receivedFile);
                        thisClient.Send(buffer2);
                    }

                    if (receivedInfoHeader[0] == 1)
                    {
                        try
                        {

                            Byte[] buffer_fileNameToPublic = new Byte[128];
                            thisClient.Receive(buffer_fileNameToPublic);
                            string fileNameToPublic = Encoding.Default.GetString(buffer_fileNameToPublic);
                            fileNameToPublic = fileNameToPublic.Substring(0, fileNameToPublic.IndexOf("\0"));

                            string toClient = makePublic(fileNameToPublic, clientUsername);

                            Byte[] infoHeader = new Byte[1];
                            infoHeader[0] = 0;
                            thisClient.Send(infoHeader);

                            Byte[] buffer_toClient = new Byte[128];
                            buffer_toClient = Encoding.Default.GetBytes(toClient);
                            thisClient.Send(buffer_toClient);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    if (receivedInfoHeader[0] == 2)//message is to delete file
                    {
                        //receive the name of file to be deleted from client
                        Byte[] buffer_delete = new byte[64];
                        thisClient.Receive(buffer_delete);

                        // Take the file name from the buffer
                        string filename = Encoding.Default.GetString(buffer_delete);

                        filename = filename.Substring(0, filename.IndexOf("\0"));
                        string rawfilename = filename;
                        filename = clientUsername + "_" + filename;

                        File.Delete(Path.Combine(DB_Path, filename));//deleting the file

                        StreamReader logReader = new StreamReader(LOGS_Path);
                        string line = "";
                        bool flag = false;
                        int lineNo = 0;
                        //bool flag2 = false;
                        while ((line = logReader.ReadLine()) != null)
                        {
                            if (!(line.Split('\t')[0] == clientUsername))
                            {
                                lineNo++;
                                continue;
                            }
                            string lineFileName = "";
                            lineFileName = line.Split('\t')[1];
                            lineFileName = lineFileName.Substring(lineFileName.IndexOf("_") + 1);
                            //logs.AppendText("@" + lineFileName + "@" + fileName + "@\n");
                            string linedaki = lineFileName.Trim();

                            if (lineFileName.Trim() == rawfilename)
                            {
                                //logs.AppendText(lineNo + "file bulundu\n");
                                flag = true;

                                if (line.Split('\t')[2] == "2")
                                {

                                    logReader.Close();
                                    // message sent to client
                                    Byte[] infoHeader = new Byte[1];
                                    infoHeader[0] = 0;
                                    thisClient.Send(infoHeader);

                                    string deleteMessage = "The file \"" + rawfilename + "\" is already deleted!";
                                    Byte[] buffer_toClient = new Byte[128];
                                    buffer_toClient = Encoding.Default.GetBytes(deleteMessage);
                                    thisClient.Send(buffer_toClient);

                                    break;
                                }
                                else
                                {
                                    logReader.Close();
                                    string newLine = line.Split('\t')[0] + '\t' + line.Split('\t')[1] + '\t' + "2\t" + line.Split('\t')[3] + '\t' + line.Split('\t')[4];
                                    lineChanger(newLine, LOGS_Path, lineNo);
                                    logs.AppendText(rawfilename + " is deleted by "+ clientUsername + '\n');

                                    // message sent to client
                                    Byte[] infoHeader = new Byte[1];
                                    infoHeader[0] = 0;
                                    thisClient.Send(infoHeader);

                                    string deleteMessage = "The file \"" + rawfilename + "\" is deleted succesfully!";
                                    Byte[] buffer_toClient = new Byte[128];
                                    buffer_toClient = Encoding.Default.GetBytes(deleteMessage);
                                    thisClient.Send(buffer_toClient);
                                    break;
                                }
                            }
                            lineNo++;
                            if (!flag)
                            {
                                // message sent to client
                                Byte[] infoHeader = new Byte[1];
                                infoHeader[0] = 0;
                                thisClient.Send(infoHeader);

                                string deleteMessage = "There is no such a file " + rawfilename + " belongs to you.\n";
                                Byte[] buffer_toClient = new Byte[128];
                                buffer_toClient = Encoding.Default.GetBytes(deleteMessage);
                                thisClient.Send(buffer_toClient);
                                
                            }
                        }
                    }


                    if (receivedInfoHeader[0] == 3) 
                    {

                        // Receive the privacy information
                        Byte[] receivedAccesRightHeader = new Byte[1];
                        thisClient.Receive(receivedAccesRightHeader);


                        // Get the file name from client
                        Byte[] buffer_filename = new Byte[128];
                        thisClient.Receive(buffer_filename);
                        string fileName = Encoding.Default.GetString(buffer_filename);

                        // Format the file name
                        fileName = fileName.Substring(0, fileName.IndexOf("\0"));
                        fileName = clientUsername + "_" + fileName;
                        string fileName_basic = fileName;
                        string noncopyname = fileName;

                        // yeni update file isim belirleme kismi fileName_basic yeni isim oluyor
                        int denemecount = 0;
                        bool deneme = false;
                        while (!deneme)
                        {
                            if (File.Exists(DB_Path + "/" + fileName_basic))
                            {
                                denemecount++;
                                fileName_basic = fileName.Split('_')[0] + "_" + fileName.Split('_')[1].Split('.')[0] + "(" + denemecount.ToString() + ")" + ".txt";
                            }
                            else
                            {
                                deneme = true;
                            }
                        }

                        //// Read the LOGS.txt to update new file name 
                        //StreamReader logReader = new StreamReader(LOGS_Path);
                        //string line = "";
                        //bool flag = false;
                        //bool flag2 = false;
                        //int count = 0;
                        //while ((line = logReader.ReadLine()) != null)
                        //{
                        //    if (!(line.Split('\t')[0] == clientUsername))
                        //        continue;
                        //    if (count != 0)
                        //    {
                        //        fileName = fileName_basic.Split('.')[fileName_basic.Split('.').Length - 2] + "(" + count.ToString() + ")." + fileName_basic.Split('.').Last();
                        //        flag = true;
                        //    }
                        //    if (line.Split('\t')[1].Split('_')[0] + "_" + line.Split('\t')[1].Split('_')[1] == fileName)
                        //    {
                        //        count = count + 1;
                        //        flag2 = true;
                        //    }
                        //}
                        //if (!flag && flag2)
                        //{
                        //    fileName_basic = fileName_basic.Split('.')[fileName_basic.Split('.').Length - 2] + "(" + count.ToString() + ")." + fileName_basic.Split('.').Last();
                        //}
                        //if (flag)
                        //{
                        //    fileName_basic = fileName_basic.Split('.')[fileName_basic.Split('.').Length - 2] + "(" + count.ToString() + ")." + fileName_basic.Split('.').Last();
                        //}
                        //logReader.Close();

                        // fileName_basic is name of the new file 
                        // noncopyname is the name of the file to be copied

                        try
                        {
                            // Will not overwrite if the destination file already exists.
                            File.Copy(Path.Combine(DB_Path, noncopyname), Path.Combine(DB_Path, fileName_basic));

                            // Access right (public/private) of the original file should also be passed to the new copy 
                            if (receivedAccesRightHeader[0] == 1)
                               makePublic(fileName_basic, clientUsername);
                        

                            // Print the logs and send the confirmation message to the Client
                            logs.AppendText("Copied file: \"" + noncopyname + "\" from \"" + clientUsername + "\"" + " as " +"\"" + fileName_basic + "\"" + "\n"); // Log message
                            string receivedFile = " Copied file: \"" + noncopyname + "\" from \"" + clientUsername + "\"" + " as " + "\"" + fileName_basic +  "\" successfully.." + "\n";


                            // Write into LOGS.txt
                            string fileLength = File.ReadAllBytes(DB_Path + "/" + noncopyname).Length.ToString();
                            string time = DateTime.Now.ToString("MM/dd/yyyy HH:mm");

                            BinaryWriter bWriteLog = new BinaryWriter(File.Open(LOGS_Path, FileMode.Append));
                            Byte[] logBuffer = Encoding.Default.GetBytes(clientUsername + "\t" + fileName_basic + "\t" + "0" + "\t" + fileLength + "bytes" + "\t" + time + "\n");
                            bWriteLog.Write(logBuffer.ToArray());
                            bWriteLog.Close();

                            // Sending the confirmation message to the Client
                            Byte[] buffer2 = new Byte[64];
                            buffer2 = Encoding.Default.GetBytes(receivedFile);
                            thisClient.Send(buffer2);
                        }

                        // Catch exception if the file was already copied.
                        catch (IOException copyError)
                        {
                            Console.WriteLine(copyError.Message);
                        }

                    }

                    if (receivedInfoHeader[0] == 4)
                    {
                        // Send the 1 byte to inform the server that the client is sending a file
                        Byte[] infoHeader = new Byte[1];
                        infoHeader[0] = 4;
                        thisClient.Send(infoHeader);


                        Byte[] buffer_filename = new Byte[128];
                        thisClient.Receive(buffer_filename);
                        string filename = Encoding.Default.GetString(buffer_filename);
                        filename = filename.Substring(0, filename.IndexOf("\0"));
                        
                        if (!File.Exists(DB_Path + "/" + filename))
                        {
                            StreamReader logReader = new StreamReader(LOGS_Path);
                            string line = "";
                            while ((line = logReader.ReadLine()) != null)
                            {
                                
                                if (line.Split('\t')[2] == "0" || line.Split('\t')[2] == "2")
                                {
                                    continue;
                                }
                                if (line.Split('\t')[1].Split('_')[1] != filename.Split('_')[1])
                                {
                                    continue;
                                }
                                filename = line.Split('\t')[1];
                            }
                            logReader.Close();

                        }
                        string filepathname = DB_Path + "/" + filename;

                        int fileProperties = 256; // FileName + The Data's Length
                        int fileNameLength = 128; // FileName
                        string fileLength = File.ReadAllBytes(filepathname).Length.ToString(); // The Data's Length is turned into string 
                                                                                                  // to put into a Byte Array with the FileName

                        Byte[] filePropertiesBuffer = new Byte[fileProperties]; // Allocate space for FileName and The Data's Length

                        // Copy the FileName and The Data's Length into the filePropertiesBuffer
                        Array.Copy(Encoding.Default.GetBytes(filename), filePropertiesBuffer, filename.Length);
                        Array.Copy(Encoding.ASCII.GetBytes(fileLength), 0, filePropertiesBuffer, fileNameLength, fileLength.Length);

                        // Send the filePropertiesBuffer to the Server
                        thisClient.Send(filePropertiesBuffer);

                        // Copy the data into generalBuffer
                        Byte[] generalBuffer = new Byte[File.ReadAllBytes(filepathname).Length];
                        generalBuffer = File.ReadAllBytes(filepathname);

                        // Send the data to the surver via generalBuffer
                        thisClient.Send(generalBuffer);

                        logs.AppendText("Client " + clientUsername + " downloaded the file "+filename.Split('_')[1]+".\n");
                    }

                    if (receivedInfoHeader[0] == 5)
                    {
                        Byte[] infoHeader = new Byte[1];
                        infoHeader[0] = 5;
                        thisClient.Send(infoHeader);
                        // Read the LOGS.txt file to determine the list
                        StreamReader logReader = new StreamReader(LOGS_Path);
                        string line = "";
                        List<String> fileList = new List<String>();
                        while ((line = logReader.ReadLine()) != null)
                        {
                            if (!(line.Split('\t')[0] == clientUsername) || line.Split('\t')[2] == "2")
                            {
                                continue;
                            }
                            fileList.Add(line.Split('\t')[1].Split('_')[1]+ "\t"+line.Split('\t')[3] + "\t" + line.Split('\t')[4]);
                        }
                        logReader.Close();
                        string count = fileList.Count.ToString();
                        Byte[] buffer_count = new Byte[64];
                        buffer_count = Encoding.Default.GetBytes(count);
                        thisClient.Send(buffer_count);
                        buffer_count = null;
                        Thread.Sleep(100);
                        for (int i = 0; i < fileList.Count; i++)
                        {
                            Byte[] buffer = new Byte[128];
                            buffer = Encoding.Default.GetBytes(fileList[i]);
                            thisClient.Send(buffer);
                            Thread.Sleep(100);
                        }
                        // Send the 1 byte to inform the server that the client is sending a file
                        infoHeader[0] = 0;
                        thisClient.Send(infoHeader);

                        Byte[] buffer_toClient = new Byte[128];
                        logs.AppendText("List of files that the client owns successfully sent to "+clientUsername+".\n");
                        buffer_toClient = Encoding.Default.GetBytes("List of files that you own successfully sent to the you.");
                        thisClient.Send(buffer_toClient);
                    }

                    if (receivedInfoHeader[0] == 6)
                    {
                        Byte[] infoHeader = new Byte[1];
                        infoHeader[0] = 6;
                        thisClient.Send(infoHeader);
                        // Read the LOGS.txt file to determine the list
                        StreamReader logReader = new StreamReader(LOGS_Path);
                        string line = "";
                        List<String> fileList = new List<String>();
                        while ((line = logReader.ReadLine()) != null)
                        {
                            if (line.Split('\t')[2] == "0" || line.Split('\t')[2] == "2")
                            {
                                continue;
                            }
                            fileList.Add(line.Split('\t')[0] + "\t" + line.Split('\t')[1].Split('_')[1] + "\t" + line.Split('\t')[3] + "\t" + line.Split('\t')[4]);
                        }
                        logReader.Close();
                        string count = fileList.Count.ToString();
                        Byte[] buffer_count = new Byte[64];
                        buffer_count = Encoding.Default.GetBytes(count);
                        thisClient.Send(buffer_count);
                        buffer_count = null;
                        Thread.Sleep(100);
                        for (int i = 0; i < fileList.Count; i++)
                        {
                            Byte[] buffer = new Byte[128];
                            buffer = Encoding.Default.GetBytes(fileList[i]);
                            thisClient.Send(buffer);
                            Thread.Sleep(100);
                        }
                        // Send the 1 byte to inform the server that the client is sending a file
                        infoHeader[0] = 0;
                        thisClient.Send(infoHeader);

                        Byte[] buffer_toClient = new Byte[128];
                        logs.AppendText("List of public files successfully sent to " + clientUsername + ".\n");
                        buffer_toClient = Encoding.Default.GetBytes("List of public files successfully sent to the you.");
                        thisClient.Send(buffer_toClient);
                    }

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

        private string makePublic(string fileName, string clientUsername)
        {
            StreamReader logReader = new StreamReader(LOGS_Path);
            string line = "";
            bool flag = false;
            int lineNo = 0;
            //bool flag2 = false;
            while ((line = logReader.ReadLine()) != null)
            {
                if (!(line.Split('\t')[0] == clientUsername))
                {
                    lineNo++;
                    continue;
                }
                string lineFileName = "";
                lineFileName = line.Split('\t')[1];
                lineFileName = lineFileName.Substring(lineFileName.IndexOf("_") + 1);
                //logs.AppendText("@" + lineFileName + "@" + fileName + "@\n");
                if (lineFileName.Trim() == fileName.Trim())
                {
                    //logs.AppendText(lineNo + "file bulundu\n");
                    flag = true;
                    if (line.Split('\t')[2] == "1")
                    {
                        logReader.Close();
                        return ("The file " + fileName + " is already public.");
                    }
                    else if (line.Split('\t')[2] == "2")
                    {
                        logReader.Close();
                        return ("The file " + fileName + " was deleted.");
                    }
                    else
                    {
                        logReader.Close();
                        string newLine = line.Split('\t')[0] + '\t' + line.Split('\t')[1] + '\t' + "1\t"+ line.Split('\t')[3] + '\t' + line.Split('\t')[4];
                        lineChanger(newLine, LOGS_Path, lineNo);
                        logs.AppendText(fileName + " made public succesfully for the user " + clientUsername + '\n');
                        return (fileName + " made public succesfully.");
                    }
                }
                lineNo++;
            }
            logReader.Close();
            if (!flag)
            {
                return ("There is no such a file " + fileName + " belongs to you.\n");
            }
            return ("something\n");
        }

        private static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            try
            {
                string[] arrLine = File.ReadAllLines(fileName);
                arrLine[line_to_edit] = newText;
                File.WriteAllLines(fileName, arrLine);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
