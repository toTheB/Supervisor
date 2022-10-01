using System;

namespace Supervisor.Model;

public class DeadLine
{
    public DateTime Line { get; set; }

    public DeadLine(DateTime line)
    {
        this.Line = line;
    }


    public TimeSpan Span
    {
        get
        {
            var nowDate = DateTime.Now;
            var gap = Line - nowDate;
            return gap;
        }
    }

}