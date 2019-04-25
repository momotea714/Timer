using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using Timer.Properties;

namespace Timer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private TimerController TimerController { get; }

        public MainWindow()
        {
            InitializeComponent();

            TimerController = new TimerController(lblTimer, prgRemainTime, () =>
            {
                this.ShowMessageAsync("", "時間になりました");
                SwitchEnabled(true);
            });

            Initialize();
        }

        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (TimerController.IsEnabled)
            {
                StopTimer();
                return;
            }

            StartTimer();
        }
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetTimer();
        }
        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            var window = new SettingWindow()
            {
                Topmost = Settings.Default.Topmost,
            };

            window.ShowDialog();

            if (window.Result == SettingEditResult.Cancel) return;

            Initialize();

            if (window.Result == SettingEditResult.Start) StartTimer();
        }
        private void LblTimer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var isVisible = pnlBtnTimePlus.Visibility == Visibility.Visible;
            pnlBtnTimePlus.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
        }
        private void BtnPlus1H_Click(object sender, RoutedEventArgs e)
        {
            TimerController.AddTime(3600);
        }
        private void BtnPlus1M_Click(object sender, RoutedEventArgs e)
        {
            TimerController.AddTime(60);
        }
        private void BtnPlus10s_Click(object sender, RoutedEventArgs e)
        {
            TimerController.AddTime(10);
        }
        private void BtnMinus1H_Click(object sender, RoutedEventArgs e)
        {
            TimerController.AddTime(-3600);
        }
        private void BtnMinus1M_Click(object sender, RoutedEventArgs e)
        {
            TimerController.AddTime(-60);
        }
        private void BtnMinus10s_Click(object sender, RoutedEventArgs e)
        {
            TimerController.AddTime(-10);
        }

        private void Initialize()
        {
            Topmost = Settings.Default.Topmost;
            TimerController.Initialize();
        }
        private void StartTimer()
        {
            TimerController.Start();
            SwitchEnabled(false);
        }
        private void StopTimer()
        {
            TimerController.Stop();
            SwitchEnabled(true);
        }
        private void ResetTimer()
        {
            TimerController.Stop();
            TimerController.Initialize();
            SwitchEnabled(true);
        }
        private void SwitchEnabled(bool isEnabled)
        {
            btnReset.IsEnabled = isEnabled;
            btnSetting.IsEnabled = isEnabled;
        }
    }
}
