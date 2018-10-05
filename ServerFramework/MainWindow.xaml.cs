using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ServerFramework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static byte[] result = new byte[1024];
        private static int myProt = 8885;   //端口  
        static Socket serverSocket;
        private static Boolean isListen = true;
        //Socket clientSocket;
        public static Dictionary<String, Socket> clientList = null;
        // Thread myThread;
        //Thread LoadThread;
        FileStream fs;
        StreamWriter sw;
        Boolean switcher=false;
        Boolean fetch_food = false;
        FileStream oStream;
        FileStream iStream;
        
        System.IO.StreamReader sr;
        StreamReader fileA;
        StreamReader fileB;
        StreamReader file_move;
        StreamReader fileD;
        StreamReader file_fetching_tool;
        StreamReader file_place_tool;
        public MainWindow()
        {
            InitializeComponent();
            clientList = new Dictionary<string, Socket>();
            IPAddress ip = IPAddress.Parse("172.18.13.122");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
            serverSocket.Listen(10);    //设定最多10个排队连接请求  
            MessageBox.Show("启动监听成功\n");

            Thread myThread = new Thread(ListenClientConnect);
            //LoadThread = new Thread(MyController.Load);
            myThread.Start();
            txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
            {
                txtReceiveMsg.Text += "服务启动...\r\n";
            }));
            //fs = new FileStream("test.txt", FileMode.Open);
            oStream = new FileStream("test.txt", FileMode.Append, FileAccess.Write, FileShare.Read);
            iStream = new FileStream("test.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
           // sw = new System.IO.StreamWriter(oStream);
            sr = new System.IO.StreamReader(iStream);
            
            //fileA = new StreamReader("textA.txt");
            //fileB = new StreamReader("textB.txt");
            //fileC = new StreamReader("textC.txt");
            //fileD = new StreamReader("textD.txt");
            //file_fetching_tool = new StreamReader("fetching_tool.txt");
            //file_place_tool = new StreamReader("place_tool.txt");
            
        }

        private void ListenClientConnect()
        {
            
            while (isListen)
            {
                try
                {
                    Socket clientSocket = serverSocket.Accept();
                    //clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello"));
                    Int32 receiveNumber = clientSocket.Receive(result);
                    string clientName = Encoding.ASCII.GetString(result, 0, receiveNumber);
                    if (!clientList.ContainsKey((clientName)))
                    {
                        clientList.Add(clientName, clientSocket);
                        txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            txtReceiveMsg.Text += clientName + "  log in\n";


                        }));
                        switch (clientName)
                        {
                            case "L":
                                //clientSocket.Send(Encoding.ASCII.GetBytes("xixixixi"));
                                Thread LeapThread = new Thread(LeapMotionThread);
                                LeapThread.Start(clientSocket);
                                break;
                            case "R":
                                //clientSocket.Send(Encoding.ASCII.GetBytes("xixixixi"));
                                Thread robotThread = new Thread(RobotThread);
                                robotThread.Start(clientSocket);
                                break;
                            case "C":
                                Thread chenweixiang = new Thread(Chenweixiang);
                                chenweixiang.Start(clientSocket);
                                break;
                        }

                    }
                    /*
                    txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txtReceiveMsg.Text += Encoding.ASCII.GetString(result, 0, receiveNumber) + "  log in\n";


                    }));
                    */
                    
                    //Thread receiveThread = new Thread(ReceiveMessage);
                    //receiveThread.Start(clientSocket);
                }
                catch(Exception e) { }
            }
        }

        private void Chenweixiang(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            Int32 receiveNumber = myClientSocket.Receive(result);

            while (isListen)
            {
                try
                {
                    //通过clientSocket接收数据  
                    receiveNumber = myClientSocket.Receive(result);
                    string send_message = Encoding.ASCII.GetString(result, 0, receiveNumber);
                    txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txtReceiveMsg.Text += "chen  wei xiang send : " + send_message + "\n";

                    }));

                    fetch_food = true;
                    place_tool();
                    //fetching_tool();
                    
                    switch (send_message)
                    {
                        case "A":
                            fetching_foodA();
                            break;
                        case "B":
                            fetching_foodB();
                            break;
                        
                        default:
                            break;
                    }

                    //ready_to_cook();

                    //fetch_food = true;
                    txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txtReceiveMsg.Text += "finish fetching food A/B/C " + send_message + "\n";

                    }));


                    fetching_tool();

                    //place_tool();
                    txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txtReceiveMsg.Text += "finish fetching_tool HAHA" + send_message + "\n";

                    }));
                    //Thread.Sleep(3000);
                    fetch_food = false;

                }
                catch { }
            }
        }

        private void fetching_foodD()
        {
            string str = null;
            //string[] rol = new string[6];
            while ((str = fileD.ReadLine()) != null)
            {
                Thread.Sleep(333);
                if (clientList.ContainsKey(("R")))
                {
                    clientList["R"].Send(Encoding.ASCII.GetBytes(str));
                }
            }

           // ready_to_cook();
        }

        private void fetching_tool()
        {
            file_fetching_tool = new StreamReader("fetching_tool.txt");
            string str = null;
            //string[] rol = new string[6];
            while ((str = file_fetching_tool.ReadLine()) != null)
            {
                Thread.Sleep(2000);
                if (clientList.ContainsKey(("R")))
                {
                    clientList["R"].Send(Encoding.ASCII.GetBytes(str));
                }

                txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                {
                    txtReceiveMsg.Text += "fetching tool send : " + str + "\n";

                }));
            }

            file_place_tool.Close();
            //fetch_food = false;

        }

        
        private void fetching_foodB()
        {
            fileB = new StreamReader("textB.txt");
            string str = null;
            //string[] rol = new string[6];
            while ((str = fileB.ReadLine()) != null)
            {
                Thread.Sleep(2000);
                if (clientList.ContainsKey(("R")))
                {
                    clientList["R"].Send(Encoding.ASCII.GetBytes(str));
                }

                txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                {
                    txtReceiveMsg.Text += "fetching B send : " + str + "\n";

                }));
            }

            fileB.Close();
        }

        private void fetching_foodA()
        {
            /*
            string str = null;
            //string[] rol = new string[6];
            string[] str_act = new string[2];
            str_act[0]= "0,0,0,0,0,-90,0";
            str_act[1] = "0,0,-56,33,0,-67,0";

            
            while ((str = fileA.ReadLine()) != null)
            {
                Thread.Sleep(333);
                if (clientList.ContainsKey(("R")))
                {
                    clientList["R"].Send(Encoding.ASCII.GetBytes(str));
                }
            }
            
            for (int i = 0; i < 2; i++)
            {
                clientList["R"].Send(Encoding.ASCII.GetBytes(str_act[i]));
               
                Thread.Sleep(3000);
            }

            txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
            {
                txtReceiveMsg.Text += "finish fetching tool AAA:" + str + "\n";

            }));

            */
            fileA = new StreamReader("textA.txt");
            string str = null;
            //string[] rol = new string[6];
            while ((str = fileA.ReadLine()) != null)
            {
                Thread.Sleep(2000);
                if (clientList.ContainsKey(("R")))
                {
                    clientList["R"].Send(Encoding.ASCII.GetBytes(str));
                }

                txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                {
                    txtReceiveMsg.Text += "fetching A send : " + str + "\n";

                }));
            }

            fileA.Close();
        }

        private void place_tool()
        {
            file_place_tool = new StreamReader("place_tool.txt");
            string str = null;
            //string[] rol = new string[6];
            txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
            {
                txtReceiveMsg.Text += "read place tool file : " + str + "\n";

            }));
            while ((str = file_place_tool.ReadLine()) != null)
            {
                Thread.Sleep(2000);
                if (clientList.ContainsKey(("R")))
                {
                    clientList["R"].Send(Encoding.ASCII.GetBytes(str));
                }

                txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                {
                    txtReceiveMsg.Text += "place tool send : " + str + "\n";

                }));
            }

            //fetch_food = false;
            
            file_place_tool.Close();
        }

        private void RobotThread(object clientSocket)
        {
            //throw new NotImplementedException();
        }

        private void LeapMotionThread(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
             Int32 receiveNumber = myClientSocket.Receive(result);
            /*
            if (!clientList.ContainsKey((Encoding.ASCII.GetString(result, 0, receiveNumber))))
                clientList.Add((Encoding.ASCII.GetString(result, 0, receiveNumber)), myClientSocket);
            txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
            {
                txtReceiveMsg.Text += Encoding.ASCII.GetString(result, 0, receiveNumber) + "  log in\n";


            }));
            */
            while (isListen)
            {
                try
                {
                    //通过clientSocket接收数据  
                    receiveNumber = myClientSocket.Receive(result);
                    string send_message = Encoding.ASCII.GetString(result, 0, receiveNumber);

                    string[] str_array;
                    str_array = send_message.Split(',');
                   
                    if (str_array[0] == "9")
                    {
                        switcher = !switcher;
                        if (switcher)
                        {
                            if (clientList != null)
                            {
                                clientList["L"].Send(Encoding.ASCII.GetBytes("true"));

                            }
                        }
                        else
                        {
                            if (clientList != null)
                            {

                                clientList["L"].Send(Encoding.ASCII.GetBytes("false"));

                            }
                        }

                        fetch_food = true;
                        txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            txtReceiveMsg.Text += "move : " + str_array[0] + "\n";

                        }));
                        file_move = new StreamReader("move.txt");
                        txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            txtReceiveMsg.Text += "read finish : " +"\n";

                        }));
                        string str=null;
                        while ((str = file_move.ReadLine()) != null)
                        {
                            Thread.Sleep(2000);
                            if (clientList.ContainsKey(("R")))
                            {
                                clientList["R"].Send(Encoding.ASCII.GetBytes(str));
                            }

                            txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                txtReceiveMsg.Text += "place tool send : " + str + "\n";

                            }));
                        }
                        file_move.Close();
                        fetch_food = false;

                        switcher = !switcher;
                        if (switcher)
                        {
                            if (clientList != null)
                            {
                                clientList["L"].Send(Encoding.ASCII.GetBytes("true"));

                            }
                        }
                        else
                        {
                            if (clientList != null)
                            {

                                clientList["L"].Send(Encoding.ASCII.GetBytes("false"));

                            }
                        }
                    }
                    txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txtReceiveMsg.Text += "from L message " + send_message + "\n";

                    }));

                    if (clientList.ContainsKey(("R")) && !fetch_food)
                    {
                        clientList["R"].Send(Encoding.ASCII.GetBytes(send_message));
                        txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            //sw.WriteLine(Encoding.ASCII.GetString(result, 0, receiveNumber));
                            //sw.Close();
                            //oStream.Write(send_message);
                            txtReceiveMsg.Text += " sending to R:" + send_message + "\n";
                        }));
                    }
                    
                }
                catch (Exception ex)
                {

                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }

        private void ReceiveMessage(object clientSocket)
        {

            Socket myClientSocket = (Socket)clientSocket;
            Int32 receiveNumber = myClientSocket.Receive(result);
            if (!clientList.ContainsKey((Encoding.ASCII.GetString(result, 0, receiveNumber))))
               clientList.Add((Encoding.ASCII.GetString(result, 0, receiveNumber)), myClientSocket);
            txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
            {
                txtReceiveMsg.Text += Encoding.ASCII.GetString(result, 0, receiveNumber) + "  log in\n";


                 }));
            while (isListen)
            {
                try
                {
                    //通过clientSocket接收数据  
                    receiveNumber = myClientSocket.Receive(result);
                    txtReceiveMsg.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        //sw.WriteLine(Encoding.ASCII.GetString(result, 0, receiveNumber));
                        txtReceiveMsg.Text += myClientSocket.RemoteEndPoint.ToString() + "  is  sending :" + Encoding.ASCII.GetString(result, 0, receiveNumber) + "\n";
                    }));
                 // sw.WriteLine(Encoding.ASCII.GetString(result, 0, receiveNumber));
                    // txtReceiveMsg.Text += myClientSocket.RemoteEndPoint.ToString() + "  is  sending :" + Encoding.ASCII.GetString(result, 0, receiveNumber) + "\n";
                }
                catch (Exception ex)
                {

                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            isListen = true;
            txtReceiveMsg.Text += "Server is working\n";
        }

        private void btnDisConnect_Click(object sender, RoutedEventArgs e)
        {
            if (serverSocket != null) { 
                 isListen = false;
                foreach (var socket in clientList.Values)
                {
                    socket.Close();
                }
                clientList.Clear();

                serverSocket.Close();
                serverSocket = null;
                //isListen = false;
                //txtReceiveMsg.Text += "服务停止\r\n";
                txtReceiveMsg.Text += "Server has shut down\n";
                //sw.Close();
            }
        }

        private void btnLeapTest_Click(object sender, RoutedEventArgs e)
        {
            switcher = !switcher;
            if (switcher)
            {
                if (clientList != null)
                {
                        clientList["L"].Send(Encoding.ASCII.GetBytes("true"));
                    
                }
            }
            else
            {
                if (clientList != null)
                {

                    clientList["L"].Send(Encoding.ASCII.GetBytes("false"));
                    
                }
            }
        }




    }
}
