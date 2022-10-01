using System;

namespace Supervisor.Model
{
    internal class Subject
    {
        public string Name { get; set; }
        public event Action OnTimeLeftChange;

        private TimeSpan _timeLeft;
        private TimeSpan _originTime;
        public TimeSpan TimeLeft
        {
            get => _timeLeft;
            set
            {
                this._timeLeft = value;
                this.OnTimeLeftChange?.Invoke();
            }
        }

        public Subject(string name, TimeSpan timeLeft)
        {
            this.Name = name;
            this._originTime = this.TimeLeft = timeLeft;
        }

        public void ResetTime()
        {
            this.TimeLeft = this._originTime;
        }
    }
}
