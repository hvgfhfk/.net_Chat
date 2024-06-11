using Microsoft.VisualBasic.ApplicationServices;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatProgram
{
    public partial class mainForm : Form
    {
        private HashSet<string> existingNicknames = new HashSet<string>();

        public static IMongoClient client = new MongoClient("");
        public static IMongoDatabase db = client.GetDatabase("");
        public static IMongoCollection<Signup> coll = db.GetCollection<Signup>("");

        private Socket socket;
        private Signup loggedInUser;

        private ListView friendListView;
        public mainForm(Socket connectedSocket, Signup user)
        {
            socket = connectedSocket;
            loggedInUser = user;
            InitializeComponent();
        }

        private void main_Load(object sender, EventArgs e)
        {
            InitializeListView();
            LoadToFriendList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddToFriend();
        }

        private void FriendListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (friendListView.SelectedItems.Count > 0)
            {
                string selectedFriend = friendListView.SelectedItems[0].Text;
                ChatForm chatForm = new ChatForm(socket, loggedInUser, selectedFriend);
                chatForm.Show();
            }
            //ChatForm chatForm = new ChatForm(socket, loggedInUser);
            //chatForm.Show();
        }

        private void InitializeListView()
        {
            friendListView = new ListView
            {
                Name = "친구목록",
                View = View.Details,
                Dock = DockStyle.Fill
            };
            friendListView.Columns.Add("친구목록", -2, HorizontalAlignment.Left);
            // ListView를 폼에 추가
            this.Controls.Add(friendListView);
            friendListView.MouseDoubleClick += FriendListView_MouseDoubleClick;
        }

        private async void AddToFriend()
        {
            var filter = Builders<Signup>.Filter.Eq("email", textBox1.Text);
            var users = await coll.Find(filter).ToListAsync();

            if (users.Count > 0)
            {
                if (friendListView == null)
                {
                    InitializeListView();
                }

                foreach (var user in users)
                {
                    if (!existingNicknames.Contains(user.username) && textBox1.Text != Global.LoggedInUser.email)
                    {
                        friendListView.Items.Add(new ListViewItem(user.username));
                        AddToFriendDataBase(textBox1.Text, user.username);
                        existingNicknames.Add(user.username);
                    }
                    else if(textBox1.Text == Global.LoggedInUser.email)
                    {
                        MessageBox.Show("본인을 추가 할 수 없습니다.");
                    }
                    else
                    {
                        MessageBox.Show("이미 추가된 닉네임입니다.");
                    }
                }
            }
            else
            {
                MessageBox.Show("존재하지 않는 이메일");
            }
        }

        private void LoadToFriendList()
        {
            var friendDataBase = client.GetDatabase("Friend");
            var friendList = friendDataBase.GetCollection<BsonDocument>(Global.LoggedInUser.email);

            // MongoDB 컬렉션에서 모든 문서 가져오기
            var documents = friendList.Find(new BsonDocument()).ToList();

            // 가져온 문서를 ListView에 추가
            foreach (var document in documents)
            {
                var friendName = document["FriendName"].AsString;
                friendListView.Items.Add(friendName);
            }
        }

        private void AddToFriendDataBase(string email, string saveFriendName)
        {
            var friendDataBase = client.GetDatabase("Friend"); // 데이터 베이스 이름을 Friend로 지정
            var friendList = friendDataBase.GetCollection<BsonDocument>(Global.LoggedInUser.email);

            var document = new BsonDocument
            {
                { "Email", email },
                { "FriendName", saveFriendName }
            };

            friendList.InsertOne(document);
        }
    }
}
