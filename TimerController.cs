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
        private int StartSeconds { get; set; }

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

        public void InitializeBySetting()
        {
            var s = Settings.Default;
            DisplayTargetLabel.Content = $"{s.Hour:00}:{s.Minute:00}:{s.Second:00}";
            RemainingSeconds = (int)new TimeSpan(s.Hour, s.Minute, s.Second).TotalSeconds;
        }
        public void Start()
        {
            Timer.Start();
            StartSeconds = RemainingSeconds;
        }
        public void Stop()
        {
            Timer.Stop();
        }
        public void AddTime(int seconds)
        {
            RemainingSeconds += seconds;
            StartSeconds += seconds;
            Display();
        }

        private void Display()
        {
            var remaining = new TimeSpan(0, 0, RemainingSeconds);
            DisplayTargetLabel.Content = $"{remaining.Hours:00}:{remaining.Minutes:00}:{remaining.Seconds:00}";
            DisplayTargetProgressBar.Maximum = StartSeconds;
            DisplayTargetProgressBar.Value = RemainingSeconds;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            RemainingSeconds--;
            Display();

            if (RemainingSeconds == 0)
            {
                Timer.Stop();
                InitializeBySetting();
                FinishAction();
            }
        }
    }
}
