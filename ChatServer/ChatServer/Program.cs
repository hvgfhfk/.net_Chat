using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;

namespace ChatServer
{
    internal class Program
    {
        private Socket socket;
        private static List<Socket> connectedClients = new List<Socket>();

        static void Main(string[] args)
        {
            using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(""), 20000);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(1000);

                SocketAsyncEventArgs arg = new SocketAsyncEventArgs();
                arg.Completed += AcceptCompleted;

                bool pending = serverSocket.AcceptAsync(arg);

                if(pending == false)
                {
                    AcceptCompleted(serverSocket, arg);
                }

                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            Socket serverSocket = (Socket)sender;
            Socket clientSocket = e.AcceptSocket;
            Console.WriteLine(clientSocket.RemoteEndPoint);

            connectedClients.Add(clientSocket);

            e.AcceptSocket = null;
            bool pending = serverSocket.AcceptAsync(e);

            if(pending == false)
            {
                AcceptCompleted(serverSocket, e);
            }

            SocketAsyncEventArgs arg = new SocketAsyncEventArgs();
            arg.Completed += ReceiveCompleted;
            byte[] buffer = new byte[256];
            arg.SetBuffer(buffer, 0, buffer.Length);

            bool pending2 = clientSocket.ReceiveAsync(arg);

            if(pending2 == false)
            {
                ReceiveCompleted(clientSocket, arg);
            }
        }

        private static async void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            Socket clientSocket = (Socket)sender;
            if (e.BytesTransferred < 1)
            {
                Console.WriteLine("Client disconnect");
                clientSocket.Dispose();
                e.Dispose();

                // 연결된 클라이언트 리스트에서 제거
                connectedClients.Remove(clientSocket);
                return;
            }

            string receivedMessage = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
            // 모든 클라이언트에게 수신한 메시지 전송
            await BroadcastMessage(receivedMessage, clientSocket);

            Console.WriteLine(receivedMessage);
            byte[] buffer = new byte[256];
            e.SetBuffer(buffer, 0, buffer.Length);

            bool pending = clientSocket.ReceiveAsync(e);

            if (pending == false)
            {
                ReceiveCompleted(clientSocket, e);
            }
        }
        private static async Task BroadcastMessage(string message, Socket senderSocket)
        {
            foreach(Socket clientSocket in connectedClients)
            {
                // 메시지를 보낸 클라이언트 제외한 나머지 메시지 전송
                if(clientSocket != senderSocket)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await clientSocket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                }
            }
        }
    }
}

