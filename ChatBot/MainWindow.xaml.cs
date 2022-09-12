using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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
            Timer.Start();
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
    }
}
