//using SuperSocket.SocketBase;
//using SuperWebSocket;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TestMagnis.DataModel;

//namespace TestMagnis
//{
//    class Server
//    {
//        public Dictionary<string, WebSocketSession> Sessions;
//        public Dictionary<string, Account> ActiveAccounts;
//        public WebSocketServer webserver;

//        public void Setup()
//        {
//            Sessions = new Dictionary<string, WebSocketSession>();

//            webserver = new WebSocketServer();
//            bool setupresult = webserver.Setup(2017);
//            if (!setupresult) { return; }
//            webserver.NewSessionConnected += OnConectedHandler;
//            webserver.SessionClosed += OnSessionClosedHandler;
//            webserver.NewMessageReceived += OnNewMessageReceivedHandler;

//        }
//        public void Start()
//        {
//            webserver.Start();

//        }
//        private void OnConectedHandler(WebSocketSession session)
//        {
//            Console.WriteLine("session conect");
//            Sessions.Add(session.SessionID, session);
//            this.SendMessage();

//        }
//        private void OnSessionClosedHandler(WebSocketSession session, CloseReason reason)
//        {
//            Console.WriteLine("Session Close");
//        }
//        private void OnNewMessageReceivedHandler(WebSocketSession session, string message)
//        {
//            Console.WriteLine("Message Receive");

//            Console.WriteLine(message);
//        }
//        public void SendMessage()
//        {
//            Session.Send("HI");
//        }
//    }
//}
