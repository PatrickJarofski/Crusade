using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using ReqRspLib;

namespace CrusadeGameClient
{
    internal class ServerConnection : ReqRspLib.ICrusadeClient
    {
        private const int _port = 777;
        private readonly TcpClient _client;

        private BinaryFormatter binaryFormatter;

        private Thread receiveThread;
        private bool shouldReceive = false;

        private Guid clientId;
        private object lockObject = new object();

        public Guid ID { get { return clientId; } }

        public ServerConnection()
        {
            try
            {
                binaryFormatter = new BinaryFormatter();
                IPAddress[] hosts = Dns.GetHostAddresses("primefusion.ddns.net");
                IPAddress serverIP = hosts[0];
                IPEndPoint ep = new IPEndPoint(serverIP, _port);

                _client = new TcpClient();
                _client.ReceiveTimeout = 3000;
                _client.SendTimeout = 3000;
                _client.Connect(ep);

                shouldReceive = true;
                receiveThread = new Thread(new ThreadStart(Receive));
                receiveThread.Start();

                DebugSendMessage();             
            }
            catch(SocketException ex)
            {
                WriteErrorToLog("Connect Error: " + ex.Message);
                WriteErrorToConsole("Connect Error: " + ex.Message);
            }
        }


        private void DebugSendMessage()
        {
            ReqRspLib.RequestTest req = new RequestTest();
            SendRequestToServer(req);
        }


        private void SendRequestToServer(ReqRspLib.IRequest request)
        {
            try
            {
                NetworkStream stream = _client.GetStream();
                binaryFormatter.Serialize(stream, request);
            }
            catch(SocketException ex)
            {
                WriteErrorToConsole(ex.Message);
                WriteErrorToLog(ex.Message);
            }
            catch(System.Runtime.Serialization.SerializationException ex)
            {
                WriteErrorToConsole("Send Error: " + ex.Message);
                WriteErrorToLog("Send Error: " + ex.Message);
            }
        }


        private void Receive()
        {
            NetworkStream stream;

            while(shouldReceive)
            {
                try
                {
                    if (_client.GetStream().DataAvailable)
                    {
                        stream = _client.GetStream();
                        IResponse rsp = (IResponse)binaryFormatter.Deserialize(stream);
                        ProcessResponse(rsp);
                    }
                }
                catch(SocketException ex)
                {
                    WriteErrorToLog("Receive Error: " + ex.Message);
                    WriteErrorToConsole("Receive Error: " + ex.Message);
                }
                catch(IOException ex)
                {
                    WriteErrorToLog("Receive Error: " + ex.Message);
                    WriteErrorToConsole("Receive Error: " + ex.Message);
                }
            }

            Console.WriteLine("No longer receiving messages.");
        }


        private void ProcessResponse(IResponse serverResponse)
        {
            if(serverResponse is ResponseClientID)
            {
                ResponseClientID rsp = serverResponse as ResponseClientID;
                clientId = rsp.ID;

                Console.WriteLine("ID assigned: {0}", rsp.ID.ToString());
            }
         
            else
                serverResponse.Execute(this);
        }


        public void RequestGameHand()
        {
            ReqRspLib.RequestHand req = new RequestHand(clientId);
            SendRequestToServer(req);
        }


        public void Disconnect()
        {
            try
            {
                lock (lockObject)
                {
                    shouldReceive = false;
                    _client.Close();
                }
            }
            catch(SocketException ex)
            {
                WriteErrorToConsole("Disconnect Error: " + ex.Message);
                WriteErrorToLog("Disconnect Error: " + ex.Message);
            }
        }


        private void WriteErrorToLog(string error)
        {
            lock (lockObject)
            {
                string path = DateTime.Now.ToString("yyyy-MM-dd") + " Client Log.txt";
                string msg = DateTime.Now.ToString("hh:mm:ss ") + error + Environment.NewLine;
                File.AppendAllText(path, msg);
            }
        }


        private void WriteErrorToConsole(string error)
        {
            lock (lockObject)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("====================================================================");
                Console.WriteLine(error);
                Console.WriteLine("====================================================================");
                Console.WriteLine(Environment.NewLine);
            }
        }


        public void DisplayHand(List<string> hand)
        {
            Console.WriteLine(Environment.NewLine + "Hand:");
            foreach (string item in hand)
                Console.WriteLine(item);

            Console.WriteLine(Environment.NewLine);
        }
    }
}
