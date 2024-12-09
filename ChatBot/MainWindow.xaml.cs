using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AIMLbot;
using Google.Cloud.Translation.V2;

namespace ChatBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public DateTime now { get; set; }=DateTime.Now;
        private string time;

        public string Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged(); }
        }
        public DispatcherTimer Timer { get; set; } = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Time = now.ToString("hh:mm");
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            //Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            now = DateTime.Now;
            Time = now.ToString("hh:mm");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MessageTxtb_MouseEnter(object sender, MouseEventArgs e)
        {
            if(MessageTxtb.Text=="Type a message")
            {
                MessageTxtb.Text = String.Empty;
                MessageTxtb.FontWeight = FontWeights.Normal;
            }
        }

        private void MessageTxtb_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MessageTxtb.Text == String.Empty)
            {
                MessageTxtb.FontWeight = FontWeights.ExtraLight;
                MessageTxtb.Text = "Type a message";
            }
        }



        private void SendingMessage()
        {
            Border border = new Border()
            {
                CornerRadius = new CornerRadius(10),
                Background = new SolidColorBrush(Color.FromRgb(227, 225, 228)),
                Width = 170,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 10, 0)
            };

            TextBlock textBlock = new TextBlock();
            textBlock.Text = MessageTxtb.Text;
            textBlock.FontSize = 15;
            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 101));
            textBlock.Margin = new Thickness(10, 0, 0, 0);
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.TextTrimming = TextTrimming.None;
            int charactherCount = MessageTxtb.Text.Length;
            int size = (charactherCount - 20) / 20 +1;
            border.Height += size * 15;
            border.Child = textBlock;
            MessagesStckPnl.Children.Add(border);
            MessageTxtb.Text = String.Empty;
            MessagesScrollViewer.ScrollToEnd();
        }

        private void ReceiveMessage(string Output)
        {
            Border border = new Border()
            {
                CornerRadius = new CornerRadius(10),
                Background = new SolidColorBrush(Color.FromRgb(185, 215, 212)),
                Width = 170,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 10, 10, 0)
            };

            TextBlock textBlock = new TextBlock();
            textBlock.Text = Output;
            textBlock.FontSize = 15;
            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 101));
            textBlock.Margin = new Thickness(10, 0, 0, 0);
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.TextTrimming = TextTrimming.None;
            int charactherCount = Output.Length;
            int size = (charactherCount - 20) / 20+1;
            border.Height += size * 15;
            border.Child = textBlock;
            MessagesStckPnl.Children.Add(border);
            MessageTxtb.Text = String.Empty;

            MessagesScrollViewer.ScrollToEnd();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            //dll paketi
            Bot AI = new Bot();
            AI.loadSettings();
            AI.loadAIMLFromFiles();
            AI.isAcceptingUserInput = false;
            User myuser = new User("ChatBot", AI);
            AI.isAcceptingUserInput = true;
            Request r = new Request(MessageTxtb.Text,myuser,AI);
            Result res = AI.Chat(r);
            string answer = res.Output;

            SendingMessage();
            ReceiveMessage(answer);
        }
    }
}
