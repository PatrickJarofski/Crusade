using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace CrusadeServer
{
    class Program
    {
        private static byte[] _buffer = new byte[1024];
        private static List<Socket> _clientSockets = new List<Socket>();
        private static Socket _serverSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int _PORT = 777;


        static void Main(string[] args)
        {
            Console.Title = "Crusade Server";
            SetupServer();
            Console.ReadLine();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _PORT));
            _serverSocket.Listen(2);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            Console.WriteLine("Now accepting client connections.");
        }

        private static void AcceptCallBack(IAsyncResult ar)
        {
            Socket socket = _serverSocket.EndAccept(ar);
            _clientSockets.Add(socket);

            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);

            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);

            Console.WriteLine(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss: ") + "A client has connected");
            Console.WriteLine("Total clients connected: " + _clientSockets.Count + Environment.NewLine);
        }

        private static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

            try
            {
                int received = socket.EndReceive(ar);
                byte[] dataBuf = new byte[received];
                Array.Copy(_buffer, dataBuf, received);

                string clientMsg = Encoding.ASCII.GetString(dataBuf);
                string sendMsg;

                if (clientMsg != "CLOSE_CONNECTION")
                {
                    sendMsg = clientMsg;   
                }
                else
                {
                    sendMsg = Environment.NewLine + "A client has disconnected." + Environment.NewLine;
                    _clientSockets.Remove(socket);
                    socket.Disconnect(true);
                    socket.Close();
                }

                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss: ") + sendMsg);
                dataBuf = Encoding.ASCII.GetBytes(sendMsg);

                for (int i = 0; i < _clientSockets.Count; ++i)
                {
                    SendData(_clientSockets[i], dataBuf);
                }

                if (socket.Connected)
                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);  
                
                else
                    PrintNumConnections();
                
            } 

            catch (SocketException ex)
            {
                string error = "ReceiveCallback Error: " + ex.Message;
                WriteErrorToConsole(ref error);
                WriteToErrorLog(ref error);

                _clientSockets.Remove(socket);
                PrintNumConnections();
            }
        }


        private static void SendData(Socket socket, byte[] bufferToSend)
        {
            try
            {
                socket.BeginSend(bufferToSend, 0, bufferToSend.Length, SocketFlags.None,
                    new AsyncCallback(SendCallBack), socket);
            }
            catch (SocketException ex)
            {
                string error = "SendData Error: " + ex.Message;
                WriteErrorToConsole(ref error);
                WriteToErrorLog(ref error);
            }
        }


        private static void SendCallBack(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndSend(ar);
            }
            catch(SocketException ex)
            {
                string error = "SendCallback Error: " + ex.Message;
                WriteErrorToConsole(ref error);
                WriteToErrorLog(ref error);
            }
        }

        private static void PrintNumConnections()
        {
            Console.WriteLine(DateTime.Now.TimeOfDay.ToString() + ": " + "Clients connected: "
                    + _clientSockets.Count + Environment.NewLine);
        }

        private static void WriteToErrorLog(ref string error)
        {
            string separator = Environment.NewLine + "============================" + Environment.NewLine;
            File.AppendAllText("Server Error Log.txt", separator + DateTime.Now.ToString("yyyyMMdd||hh:mm:ss ") + error + separator);
        }

        private static void WriteErrorToConsole(ref string error)
        {
            string separator = Environment.NewLine + "============================" + Environment.NewLine;
            Console.WriteLine(separator + DateTime.Now.ToString("yyyyMMdd||hh:mm:ss ") + error + separator);
        }
    }
}
