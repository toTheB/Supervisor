using System;
using System.Media;
using System.Speech.Synthesis;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;
using Supervisor.Model;

namespace Supervisor.ViewModel
{
    internal class SubjectViewModel : BindableBase
    {
        private string _savePath = @"./";


        public TimeSpan LeftTimeView
        {
            get { return _subject.TimeLeft; }
            set
            {
                _subject.TimeLeft = value;
                RaisePropertyChanged(nameof(LeftTimeView));
            }
        }


        private string _timerState = "继续";

        public string TimerState
        {
            get { return _timerState; }
            set
            {
                _timerState = value;
                RaisePropertyChanged(nameof(TimerState));
            }
        }

        private bool _isPaused = true;

        private Brush _textBrush = Brushes.Black;

        public Brush TextBrush
        {
            get { return _textBrush; }
            set
            {
                _textBrush = value;
                RaisePropertyChanged(nameof(TextBrush));
            }
        }

        private MainWindowViewModel _owner;

        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                if (_isPaused)
                {
                    this._timer.Stop();
                    this.TimerState = "继续";
                    this.TextBrush = Brushes.Black;
                }
                else
                {
                    this._timer.Start();
                    this.TimerState = "暂停";
                    this.TextBrush = Brushes.Red;
                }
            }
        }

        private Subject _subject;

        public string Name
        {
            get
            {
                return this._subject.Name;
            }
        }

        public DelegateCommand PauseCommand { get; set; }
        public DelegateCommand ResetCommand { get; set; }

        private Timer _timer;

        private void InitTimer()
        {
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.LeftTimeView -= TimeSpan.FromSeconds(1);
                    this._owner.TotalTime -= TimeSpan.FromSeconds(1);
                });
            };
            _timer.Stop();
        }

        private void BindingCommands()
        {
            this.PauseCommand = new DelegateCommand((() =>
            {
                this.IsPaused = !this.IsPaused;
            }));
            this._subject.OnTimeLeftChange += () =>
            {
                RaisePropertyChanged(nameof(LeftTimeView));
                if (this._subject.TimeLeft == TimeSpan.Zero)
                {
                    _timer.Stop();
                    SystemSounds.Beep.Play();
                    MessageBox.Show($"今天{this._subject.Name}的学习已经完成。");
                }
            };
            this.ResetCommand = new DelegateCommand((() =>
            {
                this._subject.ResetTime();
            }));
        }

        public SubjectViewModel(Subject subject, MainWindowViewModel owner)
        {
            this._subject = subject;
            BindingCommands();
            InitTimer();
            _owner = owner;
        }
    }
}
