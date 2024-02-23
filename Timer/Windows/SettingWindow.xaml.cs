using MahApps.Metro.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Timer.Properties;

namespace Timer
{
    /// <summary>
    /// SettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : MetroWindow
    {
        public SettingEditResult Result { get; private set; }

        public SettingWindow()
        {
            InitializeComponent();
            sldHour.Ticks = new DoubleCollection(Enumerable.Range(0, 24).Select(x => (double)x));
            sldMinute.Ticks = new DoubleCollection(Enumerable.Range(0, 59).Select(x => (double)x));
            sldSecond.Ticks = new DoubleCollection(Enumerable.Range(0, 59).Select(x => (double)x));
            DisplaySetting();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            CommitSetting();
            Result = SettingEditResult.Start;
            Close();
        }
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            CommitSetting();
            Result = SettingEditResult.OK;
            Close();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = SettingEditResult.Cancel;
            Close();
        }

        private void DisplaySetting()
        {
            sldHour.Value = Settings.Default.Hour;
            sldMinute.Value = Settings.Default.Minute;
            sldSecond.Value = Settings.Default.Second;
            chkTopmost.IsChecked = Settings.Default.Topmost;
        }
        private void CommitSetting()
        {
            Settings.Default.Hour = (int)sldHour.Value;
            Settings.Default.Minute = (int)sldMinute.Value;
            Settings.Default.Second = (int)sldSecond.Value;
            Settings.Default.Topmost = (bool)chkTopmost.IsChecked;
            Settings.Default.Save();
        }
    }
}
