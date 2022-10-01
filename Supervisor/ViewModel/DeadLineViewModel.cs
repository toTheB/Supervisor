using System.Timers;
using System.Windows;
using Prism.Mvvm;
using Supervisor.Model;

namespace Supervisor.ViewModel;

public class DeadLineViewModel : BindableBase
{
    private DeadLine _deadLine;

    private string _spanText;

    private Timer _timer;
    public string SpanText
    {
        get { return _spanText; }
        set
        {
            _spanText = value;
            RaisePropertyChanged(nameof(SpanText));
        }
    }

    public string Line { get; set; }

    private void InitTimer()
    {
        this._timer = new Timer();
        _timer.Interval = 1000;
        _timer.Elapsed += (sender, args) =>
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var span = _deadLine.Span;
                SpanText = $"{span.Hours:00}:{span.Minutes:00}:{span.Seconds:00}";
            });
        };
        _timer.Start();
    }
    public DeadLineViewModel(DeadLine deadLine)
    {
        _deadLine = deadLine;
        Line = $"{_deadLine.Line.Hour:00}:{_deadLine.Line.Minute:00}:{_deadLine.Line.Second:00}";
        InitTimer();
    }
}