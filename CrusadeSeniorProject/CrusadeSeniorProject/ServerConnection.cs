using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace CrusadeSeniorProject
{
    public class ServerConnection
    {
        private readonly CrusadeGameClient _GameClient;

        private readonly Socket _clientSocket;        
        private readonly IPAddress _IPAddress;
        private const int _Port = 777;

        private byte[] _Buffer = new byte[1024];

        private readonly string _name;

        public ServerConnection(CrusadeGameClient game)
        {
            _GameClient = game;

            _name = "SomeName";
            IPAddress[] ipHostInfo = Dns.GetHostAddresses("primefusion.ddns.net");
            _IPAddress = ipHostInfo[0];

            _clientSocket = new Socket
                (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                IPEndPoint endpoint = new IPEndPoint(_IPAddress, _Port);
                _clientSocket.Connect(endpoint);

                _Buffer = Encoding.ASCII.GetBytes("New Client " + _name + " has joined.");
                _clientSocket.BeginSend(_Buffer, 0, _Buffer.Length, 
                        SocketFlags.None, new AsyncCallback(OnSend), null);

                _Buffer = new byte[1024];
                _clientSocket.BeginReceive(_Buffer, 0, _Buffer.Length,
                        SocketFlags.None, new AsyncCallback(OnReceive), null);
            }

            catch(SocketException ex)
            {
                throw ex;
            }
        }


        private void OnSend(IAsyncResult ar)
        {
            try
            {
                _clientSocket.EndSend(ar);
            }
            catch(SocketException ex)
            {
                throw ex;
            }
        }


        private void OnReceive(IAsyncResult ar)
        {
            string msg = "default";
            try
            {
                if(_clientSocket.Connected)
                    _clientSocket.EndReceive(ar);

                msg = Encoding.ASCII.GetString(_Buffer);
            }

            catch (SocketException ex)
            {
                throw ex;
            }

            finally
            {
                if(msg != "CLOSE_CONNECTION")
                {
                    _Buffer = new byte[1024];
                    _clientSocket.BeginReceive(_Buffer, 0, _Buffer.Length,
                        SocketFlags.None, new AsyncCallback(OnReceive), null);
                }

            }
        }       


        internal void EndConnection()
        {
            _Buffer = Encoding.ASCII.GetBytes("CLOSE_CONNECTION");
            try
            {
                _clientSocket.Send(_Buffer, 0, _Buffer.Length, SocketFlags.None);
                _clientSocket.Disconnect(true);
                _clientSocket.Close();
            }

            catch (SocketException)
            {
                ///TODO
            }
        }
    }
}
