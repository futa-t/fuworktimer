using System;

namespace fuworktimer.Model;

internal class AutoSaveTimer(Action callback)
{
    private System.Threading.Timer? _timer;
    private event Action _callback = callback;

    public void Start()
    {
        DateTime now = DateTime.Now;
        DateTime addHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
        TimeSpan next = addHour - now;
        _timer = new(Callback, null, next, TimeSpan.FromHours(1));
    }

    public void Stop() => _timer?.Dispose();

    private void Callback(object? state) => Task.Run(_callback.Invoke);

}

