using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

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

            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None,
                new AsyncCallback(ReceiveCallBack), socket);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);

            Console.WriteLine(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss: ") 
                + "A client has connected");
            Console.WriteLine("Total clients connected: " + _clientSockets.Count 
                + Environment.NewLine);

        }

        private static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

            try
            {
                int received = socket.EndReceive(ar);
                byte[] dataBuf = new byte[received];
                Array.Copy(_buffer, dataBuf, received);

                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss: ")
                    + Encoding.ASCII.GetString(dataBuf));
                
                for(int i = 0; i <_clientSockets.Count; ++i)
                {
                    try
                    {
                        _clientSockets[i].BeginSend(dataBuf, 0, dataBuf.Length, SocketFlags.None,
                            new AsyncCallback(SendCallBack), _clientSockets[i]);
                    }
                    catch(SocketException ex)
                    {
                        Console.WriteLine(Environment.NewLine + DateTime.Now.TimeOfDay.ToString() + ": " + "Could not send message to socket #" + i.ToString());
                        Console.WriteLine(DateTime.Now.TimeOfDay.ToString() + ": " + "Error: " + ex.Message + Environment.NewLine);
                    }
                }

                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
            }
            catch(SocketException ex)
            {
                Console.WriteLine(Environment.NewLine + DateTime.Now.TimeOfDay.ToString() 
                    + ": " + "Error: " + ex.Message);

                _clientSockets.Remove(socket);

                Console.WriteLine(DateTime.Now.TimeOfDay.ToString() + ": " + "Clients connected: " 
                    + _clientSockets.Count + Environment.NewLine);
            }
        }

        private static void SendCallBack(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }
    }
}
