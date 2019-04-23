using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Threading;
using Timer.Properties;

namespace Timer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public DispatcherTimer Timer { get; set; }
        public int RemainingSeconds { get; set; }

        private int _startSeconds { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            InitializeBySetting();

            Timer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = new TimeSpan(0, 0, 1),
            };
            Timer.Tick += new EventHandler(Timer_Tick);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RemainingSeconds--;
            Display();

            if (RemainingSeconds == 0)
            {
                Timer.Stop();
                MessageBox.Show("時間になりました", "", MessageBoxButton.OK, MessageBoxImage.Information);
                InitializeBySetting();
                SwitchEnabled(true);
            }
        }

        private void Display()
        {
            var remaining = new TimeSpan(0, 0, RemainingSeconds);
            lblTimer.Content = $"{remaining.Hours:00}:{remaining.Minutes:00}:{remaining.Seconds:00}";
            prgRemainTime.Maximum = _startSeconds;
            prgRemainTime.Value = RemainingSeconds;
        }

        private void SwitchEnabled(bool isEnabled)
        {
            btnReset.IsEnabled = isEnabled;
            btnSetting.IsEnabled = isEnabled;
        }

        private void AddTime(int second)
        {
            RemainingSeconds += second;
            _startSeconds += second;
            Display();
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            var window = new SettingWindow()
            {
                Topmost = Settings.Default.Topmost,
            };

            window.ShowDialog();

            if (window.Result == SettingEditResult.Cancel) return;

            InitializeBySetting();

            if (window.Result == SettingEditResult.Start) StartTimer();
        }
        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (Timer.IsEnabled)
            {
                StopTimer();
                return;
            }

            StartTimer();
        }

        private void BtnPlus1H_Click(object sender, RoutedEventArgs e)
        {
            AddTime(3600);
        }

        private void BtnPlus1M_Click(object sender, RoutedEventArgs e)
        {
            AddTime(60);
        }

        private void BtnPlus10s_Click(object sender, RoutedEventArgs e)
        {
            AddTime(10);
        }

        private void BtnMinus1H_Click(object sender, RoutedEventArgs e)
        {
            AddTime(-3600);
        }

        private void BtnMinus1M_Click(object sender, RoutedEventArgs e)
        {
            AddTime(-60);
        }

        private void BtnMinus10s_Click(object sender, RoutedEventArgs e)
        {
            AddTime(-10);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetTimer();
        }

        private void InitializeBySetting()
        {
            Topmost = Settings.Default.Topmost;
            lblTimer.Content = $"{Settings.Default.Hour:00}:{Settings.Default.Minute:00}:{Settings.Default.Second:00}";
            RemainingSeconds = (int)new TimeSpan(Settings.Default.Hour, Settings.Default.Minute, Settings.Default.Second).TotalSeconds;
        }
        private void StartTimer()
        {
            Timer.Start();
            SwitchEnabled(false);
            _startSeconds = RemainingSeconds;
        }
        private void StopTimer()
        {
            Timer.Stop();
            SwitchEnabled(true);
        }
        private void ResetTimer()
        {
            Timer.Stop();
            InitializeBySetting();
            SwitchEnabled(true);
        }

        private void LblTimer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (pnlBtnTimePlus.Visibility == Visibility.Collapsed)
                pnlBtnTimePlus.Visibility = Visibility.Visible;
            else
                pnlBtnTimePlus.Visibility = Visibility.Collapsed;
        }
    }
}
