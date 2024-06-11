using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using MongoDB.Driver;
using DnsClient.Protocol;

namespace ChatProgram
{
    public partial class ChatForm : Form
    {
        private Socket socket;
        private IPEndPoint endPoint;

        private Signup loggedInUser;
        private string selectedFriend;

        public ChatForm(Socket connectedSocket, Signup user, string friend)
        {
            socket = connectedSocket;
            loggedInUser = user;
            selectedFriend = friend;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _ = ReceiveMessagesAsync();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
           string userMessage = $"{loggedInUser.username} to {selectedFriend} : {txtMessage.Text}";
           await SendMessage(userMessage);
        }

        private void txtSend_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private async Task SendMessage(string message)
        {
            if(socket == null || !socket.Connected)
            {
                MessageBox.Show("Socket is not connected.");
                return;
            }

            string formattedMessage = FormatMessage($"{loggedInUser.username} : {txtMessage.Text}");
            listView1.Items.Add(formattedMessage);

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
            txtMessage.Clear();
        }

        private async Task ReceiveMessagesAsync()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[256];
                    int received = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

                    if (received > 0)
                    {
                        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, received);
                        string[] parts = receivedMessage.Split(new string[] { " to " }, StringSplitOptions.None);

                        if (parts.Length == 2)
                        {
                            string recipientInfo = parts[1];
                            string[] recipientParts = recipientInfo.Split(new string[] { " : " }, StringSplitOptions.None);

                            if (recipientParts.Length == 2 && recipientParts[0] == loggedInUser.username)
                            {
                                AddMessageToListView(receivedMessage);
                            }
                        }
                    }
                }
                catch (SocketException ex)
                {
                    socket.Dispose();
                    break;
                }
                catch (ObjectDisposedException)
                {
                    // Socket has been closed
                    socket.Dispose();
                    break;
                }
                catch (Exception ex)
                {
                    socket.Dispose();
                    break;
                }
            }
        }

        private void AddMessageToListView(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AddMessageToListView), message);
                return;

            }

            string formattedMessage = FormatMessage(message);
            listView1.Items.Add(formattedMessage); // 포멧팅 된 메시지만 추가
        }

        private string FormatMessage(string message)
        {
            
            string[] parts = message.Split(new string[] { " to " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                string sender = parts[0];
                string recipientInfo = parts[1];
                string[] recipientParts = recipientInfo.Split(new string[] { " : " }, StringSplitOptions.None);
                if (recipientParts.Length == 2)
                {
                    string msgText = recipientParts[1];
                    return $"{sender} : {msgText}";
                }
            }
            return message;
        }
    }
}
