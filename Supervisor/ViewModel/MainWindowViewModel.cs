using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Prism.Mvvm;
using Supervisor.Model;

namespace Supervisor.ViewModel
{
    internal class MainWindowViewModel : BindableBase
    {
        private static string SavePath = "./科目剩余时间.txt";

        private static List<Subject> _defaultSubjects = new List<Subject>()
        {
            new Subject("数学", new TimeSpan(3,0,0)),
            new Subject("英语", new TimeSpan(1,30,0)),
            new Subject("政治", new TimeSpan(1,30,0)),
            new Subject("专业课", new TimeSpan(2,0,0))
        };

        public List<SubjectViewModel> SubjectViewModels { get; set; }

        public DeadLineViewModel DeadLineViewModel { get; set; }

        private TimeSpan _totalTime;

        public TimeSpan TotalTime
        {
            get { return _totalTime; }
            set
            {
                _totalTime = value;
                RaisePropertyChanged(nameof(TotalTime));
            }
        }

        private void InitSubjectList()
        {
            var subjectList = LoadSubjectList();
            SubjectViewModels = new List<SubjectViewModel>();
            TotalTime = TimeSpan.Zero;
            foreach (var subject in subjectList)
            {
                SubjectViewModels.Add(new SubjectViewModel(subject, this));
                TotalTime += subject.TimeLeft;
            }
        }

        private void InitDeadLine()
        {
            var nowDate = DateTime.Now;
            var deadLine = new DeadLine(new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, 23, 0, 0));
            DeadLineViewModel = new DeadLineViewModel(deadLine);
        }
        public MainWindowViewModel()
        {
            InitSubjectList();
            InitDeadLine();
        }

        //记录全部科目的剩余时间
        public void SaveSubjectList()
        {
            if (!File.Exists(SavePath))
            {
                File.Create(SavePath).Close();
            }

            using (var writer = new StreamWriter(SavePath))
            {
                var now = DateTime.Now;
                writer.WriteLine($"last closed:{now.Year},{now.Month},{now.Day}");
                foreach (var subject in SubjectViewModels)
                {
                    writer.WriteLine($"{subject.Name}: {(subject.GetType().GetField("_subject", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(subject) as Subject)?.TimeLeft.TotalSeconds}");
                }
            }
        }

        public List<Subject> LoadSubjectList()
        {
            if (!File.Exists(SavePath))
            {
                return _defaultSubjects;
            }

            using (var reader = new StreamReader(SavePath))
            {
                //读取时间
                var FirstLine = reader.ReadLine();
                var LastCloseRecord = FirstLine.Split(':')[1].Split(',');
                var LastCloseTime = new DateTime(int.Parse(LastCloseRecord[0]), int.Parse(LastCloseRecord[1]),
                    int.Parse(LastCloseRecord[2]), 0, 0, 0);

                //如果已经过了一天，则恢复时间。
                if (DateTime.Now.CompareTo(LastCloseTime.AddDays(1)) > 0)
                {
                    return _defaultSubjects;
                }

                //否则用记录的时间进行初始化。
                var subjectList = new List<Subject>();
                var curLine = reader.ReadLine();
                while (curLine != null)
                {
                    var subjectAndTime = curLine.Split(':');
                    subjectList.Add(new Subject(subjectAndTime[0], TimeSpan.FromSeconds(int.Parse(subjectAndTime[1]))));
                    curLine = reader.ReadLine();
                }
                return subjectList;
            }
        }
    }
}
