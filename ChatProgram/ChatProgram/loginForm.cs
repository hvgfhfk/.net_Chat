using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace ChatProgram
{
    //niKkHTSJAhCMdFzD
    public partial class loginForm : Form
    {
        public static IMongoClient client = new MongoClient("");
        public static IMongoDatabase db = client.GetDatabase("");
        public static IMongoCollection<Signup> coll = db.GetCollection<Signup>("");

        private HashSet<string> loggedInUsers = new HashSet<string>(); // 전역 HashSet 선언

        private Socket socket;
        public loginForm()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            FindUserDataLoginInfo();
        }
        private async void FindUserDataLoginInfo()
        {
            var filter = Builders<Signup>.Filter.Eq("email", emailText.Text);
            var user = coll.Find(filter).FirstOrDefault();

            if (user != null && user.password == passwordText.Text)
            {
                // 중복 로그인 방지
                if(!loggedInUsers.Contains(user.email)) // 이미 로그인한 사용자인지 확인
                {
                    loggedInUsers.Add(user.email);

                    // 전역 변수에 유저 정보 저장
                    Global.LoggedInUser = user;
                    await LoginConnectRequest(Global.LoggedInUser.email);

                    mainForm mainForm = new mainForm(socket, user);
                    mainForm.FormClosed += async (sender, args) => await LogoutUser();
                    mainForm.Show();

                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Already logged in.");
                }
            }
            else
            {
                MessageBox.Show("Login Failed");
            }
        }
        private async Task LoginConnectRequest(string username)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(""), 20000);

            await socket.ConnectAsync(endPoint);

            string str =  username + " 연결 완료";

            byte[] buffer = Encoding.UTF8.GetBytes(str);
            await socket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

        }

        private async Task LogoutUser()
        {
            if(socket !=null && socket.Connected)
            {
                string username = Global.LoggedInUser.email;

                // 서버에 로그아웃 메시지 보내기
                await SendLogoutMessage(username);

                // 사용자 로그아웃 처리
                loggedInUsers.Remove(username);

                // 소켓 연결 종료
                socket.Dispose();
                socket.Close();
            }
        }

        private async Task SendLogoutMessage(string username)
        {
            if(socket != null && socket.Connected)
            {
                string logoutMessage = username + " 연결 종료"; // 로그아웃 메시지

                byte[] buffer = Encoding.UTF8.GetBytes(logoutMessage);
                await socket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
            }
        }
    }

    public static class Global
    {
        public static Signup LoggedInUser { get; set; }
    }
    public class Signup
    {
        [BsonId]

        public ObjectId Id { get; set; }

        [BsonElement("username")]

        public string username { get; set; }

        [BsonElement("email")]

        public string email { get; set; }

        [BsonElement("password")]
        public string password { get; set; }

        public Signup(string username, string email, string password)
        {
            this.username = username;
            this.email = email;
            this.password = password;
        }
    }
}
