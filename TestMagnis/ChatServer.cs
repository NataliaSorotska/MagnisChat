using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMagnis.DataModel;

namespace TestMagnis
{
    public class ChatServer
    {
        private readonly String onConnectedMessage = "You are connected. Now you have to register.";
        private readonly String invalidRegisterMessage = "Please, use next register message format: 'Register <login> <password>'";
        private readonly String registerSuccessMessage = "Welcome to chat!";
        private readonly String cannotRegisterUserMessage = "Registration error! Create another account or use correct password";

        private readonly WebSocketServer _server;
        private readonly Dictionary<String, WebSocketSession> _sessions;
        private readonly Dictionary<String, Account> _activeClients;
        private readonly AccountManager _accountManager;
        private readonly MessageManager _messegeManager;


        public ChatServer()
        {
            _sessions = new Dictionary<String, WebSocketSession>();
            _activeClients = new Dictionary<String, Account>();
            _accountManager = new AccountManager();
            _messegeManager = new MessageManager();

            _server = new WebSocketServer();
            _server.Setup(33333);
            _server.NewSessionConnected += OnConnected;
            _server.NewMessageReceived += OnMessageReceived;
            _server.SessionClosed += OnDisconnected;
        }

        public void Start()
        {
            _server.Start();
        }

        void OnConnected(WebSocketSession session)
        {
            _sessions.Add(session.SessionID, session);
            session.Send(onConnectedMessage + invalidRegisterMessage);
        }

        void OnMessageReceived(WebSocketSession session, string message)
        {
            Account account = _activeClients.ContainsKey(session.SessionID) ? _activeClients[session.SessionID] : null;
            bool isAuthorized = account != null;
            if (isAuthorized)
            {
                WebSocketSession activeClientSession;
                String signedMessage = string.Format("{0}: {1}", account.Name, message);
                foreach (String key in _activeClients.Keys)
                {
                    activeClientSession = _sessions[key];
                    activeClientSession.Send(signedMessage);
                }
                Message dbMessage = new Message();
                dbMessage.MessageDate = DateTime.UtcNow;
                dbMessage.MessageText = message;
                dbMessage.AccountId = account.AccountId;
                dbMessage.AccountName = account.Name;
                _messegeManager.SaveMessage(dbMessage);

            }
            else
            {
                string[] arguments = message.Split(' ');
                if (arguments.Length != 3)
                {
                    session.Send(invalidRegisterMessage);
                    return;
                }

                if (arguments[0] != "Register")
                {
                    session.Send(invalidRegisterMessage);
                    return;
                }

                string login = arguments[1];
                string password = arguments[2];

                account = _accountManager.GetAccount(login, password);

                if (account != null)
                {
                    _activeClients.Add(session.SessionID, account);
                    session.Send(registerSuccessMessage);
                    SendLastMessages(session, 500);

                    return;
                }

                account = new Account() { Name = login, Password = password };
                bool isRegisterSuccess = _accountManager.RegisterAccount(account);

                if (!isRegisterSuccess)
                {
                    session.Send(cannotRegisterUserMessage);
                    return;
                }

                _activeClients.Add(session.SessionID, account);
                session.Send(registerSuccessMessage);
                SendLastMessages(session, 500);


            }

        }
        private void SendLastMessages(WebSocketSession session, int count)

        {
            IEnumerable<Message> LastMessages = _messegeManager.GetLastMessages(count);
            foreach (Message msg in LastMessages)
            {
                session.Send(String.Format("{0}: {1}", msg.AccountName, msg.MessageText));
            }
        }

        void OnDisconnected(WebSocketSession session, CloseReason reason)
        {
            _sessions.Remove(session.SessionID);
            ;
            Account account = _activeClients.ContainsKey(session.SessionID) ? _activeClients[session.SessionID] : null;

            if (account != null)
            {
                _activeClients.Remove(session.SessionID);

                WebSocketSession activeClientSession;
                String signedMessage = string.Format("[{0} {1}]", account.Name, "disconnected");
                foreach (String key in _activeClients.Keys)
                {
                    activeClientSession = _sessions[key];
                    activeClientSession.Send(signedMessage);
                }
            }
        }
    }
}
