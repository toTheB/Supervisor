using System;

namespace Supervisor.Model
{
    internal class Subject
    {
        public string Name { get; set; }
        public event Action OnTimeLeftChange;

        private int _timeLeft;
        private int _originTime;
        public int TimeLeft
        {
            get => _timeLeft;
            set
            {
                this._timeLeft = value;
                this.OnTimeLeftChange?.Invoke();
            }
        }

        public Subject(string name, int timeLeft)
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
