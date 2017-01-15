using SuperSocket.ClientEngine;
using System;
using System.Windows;
using System.Windows.Controls;
using WebSocket4Net;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebSocket _client;
        private IProgress<String> _progress;



        public string UserLogin
        {
            get { return (string)GetValue(UserLoginProperty); }
            set { SetValue(UserLoginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserLogin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserLoginProperty =
            DependencyProperty.Register("UserLogin", typeof(string), typeof(MainWindow));




        public string UserPassword
        {
            get { return (string)GetValue(UserPasswordProperty); }
            set { SetValue(UserPasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserPassword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserPasswordProperty =
            DependencyProperty.Register("UserPassword", typeof(string), typeof(MainWindow));



        public string UserNewMessage
        {
            get { return (string)GetValue(NewMessageProperty); }
            set { SetValue(NewMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewMessageProperty =
            DependencyProperty.Register("UserNewMessage", typeof(string), typeof(MainWindow));



        public MainWindow()
        {
            InitializeComponent();
        }

        private void sendMessage_Click(object sender, RoutedEventArgs e)
        {
            string newMessage = UserNewMessage;
            _client.Send(newMessage);
            UserNewMessage = "";
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            string login = UserLogin;
            string password = UserPassword;

            if (String.IsNullOrWhiteSpace(login) && String.IsNullOrWhiteSpace(password))
            {
                return;
            }
            if (_client.State != WebSocketState.Open)
            {
                _client.Open();
            }
            else
            {
                Register();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxOutput textboxOutput = new TextBoxOutput(ReceivedMessages);
            _progress = textboxOutput;
            _client = new WebSocket("ws://localhost:33333/");
            _client.Opened += OnOpenedHandler;
            _client.Error += OnErrorHandler;
            _client.Closed += OnClosedHandler;
            _client.MessageReceived += OnMessageReceivedHandler;
        }

        private void OnMessageReceivedHandler(object sender, MessageReceivedEventArgs e)
        {
            _progress.Report(e.Message);
            NewMessage.Dispatcher.Invoke(()=> {
                NewMessage.Focus();
            });
        }

        private void OnClosedHandler(object sender, EventArgs e)
        {
            _progress.Report("closed");
        }

        private void OnErrorHandler(object sender, ErrorEventArgs e)
        {
            _progress.Report("Error: " + e.Exception.Message);
        }

        private void OnOpenedHandler(object sender, EventArgs e)
        {
            _progress.Report("connected");

            Login.Dispatcher.Invoke(() =>
            {
                Register();
            });            
        }

        void Register()
        {
            string login = UserLogin;
            string password = UserPassword;

            if (String.IsNullOrWhiteSpace(login) || String.IsNullOrWhiteSpace(password))
            {
                return;
            }
            _client.Send(String.Format("Register {0} {1}", login, password));
        }
    }
    class TextBoxOutput : IProgress<String>
    {
        TextBox _textBox;
        public TextBoxOutput(TextBox box)
        {
            _textBox = box;
        }

        public void Report(String message)
        {
            _textBox.Dispatcher.Invoke(() => { _textBox.AppendText("\r\n"+message);
                _textBox.Focus();
                _textBox.CaretIndex = _textBox.Text.Length;
                _textBox.ScrollToEnd();
            });
        }
    }
}
