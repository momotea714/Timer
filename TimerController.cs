using System;
using System.Windows.Controls;
using System.Windows.Threading;
using Timer.Properties;

namespace Timer
{
    internal class TimerController
    {
        public bool IsEnabled => Timer.IsEnabled;

        private DispatcherTimer Timer { get; }
        private Label DisplayTargetLabel { get; }
        private ProgressBar DisplayTargetProgressBar { get; }
        private Action FinishAction { get; }
        private int RemainingSeconds { get; set; }

        public TimerController(Label displayTargetLabel, ProgressBar displayTargetProgressBar, Action finishAction)
        {
            DisplayTargetLabel = displayTargetLabel;
            DisplayTargetProgressBar = displayTargetProgressBar;
            FinishAction = finishAction;

            Timer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = new TimeSpan(0, 0, 1),
            };
            Timer.Tick += new EventHandler(Timer_Tick);
        }

        public void Initialize()
        {
            var s = Settings.Default;
            var totalSeconds = (int)new TimeSpan(s.Hour, s.Minute, s.Second).TotalSeconds;
            RemainingSeconds = totalSeconds;
            DisplayTargetProgressBar.Maximum = totalSeconds;

            Display();
        }
        public void Start()
        {
            Timer.Start();
        }
        public void Stop()
        {
            Timer.Stop();
        }
        public void AddTime(int seconds)
        {
            // 負になる場合は処理を行わない
            if (RemainingSeconds + seconds < 0) return;

            RemainingSeconds += seconds;
            DisplayTargetProgressBar.Maximum += seconds;
            Display();
        }

        private void Display()
        {
            var remaining = new TimeSpan(0, 0, RemainingSeconds);
            DisplayTargetLabel.Content = $"{remaining.Hours:00}:{remaining.Minutes:00}:{remaining.Seconds:00}";
            DisplayTargetProgressBar.Value = RemainingSeconds;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            RemainingSeconds--;
            Display();

            if (RemainingSeconds == 0)
            {
                Timer.Stop();
                Initialize();
                FinishAction();
            }
        }
    }
}
