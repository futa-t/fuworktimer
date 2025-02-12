using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fuworktimer;

internal class AutoSaveTimer(Action callback)
{
    private System.Threading.Timer? _timer;
    private event Action _callback = callback;

    public void Start()
    {
        DateTime now = DateTime.Now;
        DateTime addHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
        TimeSpan next = addHour - now;
        this._timer = new(Callback, null, next, TimeSpan.FromHours(1));
    }

    public void Stop() => this._timer?.Dispose();

    private void Callback(object? state) => Task.Run(this._callback.Invoke);

}

