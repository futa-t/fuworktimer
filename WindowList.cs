using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MemoryPack;

using static fuworktimer.Win32;
using static fuworktimer.ColorUtil;
using System.Drawing.Drawing2D;

namespace fuworktimer;

internal class WindowList
{
    readonly Dictionary<string, WindowData> _windowDict = [];

    public string? FocusWindow { get; set; } = null;

    internal static WindowList FromSaveFile(string fileName)
    {
        WindowList windowList = new();
        windowList.Load(fileName);
        return windowList;
    }

    internal WindowData? GetWindowData(string processName)
    {
        _windowDict.TryGetValue(processName, out WindowData? wd);
        return wd;
    }

    internal WindowData GetActiveWindowData()
    {
        string activeProc = GetActiveWindowProcessName();

        if (GetWindowData(activeProc) is not WindowData wd)
        {
            wd = new WindowData(activeProc, GetColorFromText(activeProc));
            _windowDict[activeProc] = wd;
        }
        return wd;
    }

    internal WindowData? GetFocusWindowData()
    {
        if (FocusWindow == null) return null;

        return GetWindowData(FocusWindow);
    }

    internal void ResetSessionTime()
    {
        foreach (var window in _windowDict.Values)
            window.ActiveTimeSession = 0;
    }

    internal bool Save(string fileName)
    {
        try
        {
            List<WindowDataPack> list = [];

            foreach (var item in _windowDict.Values)
                list.Add(new WindowDataPack(item.ProcessName, item.Color.ToArgb(), item.ActiveTimeTotal));

            File.WriteAllBytes(fileName, MemoryPackSerializer.Serialize(list));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    internal bool Load(string fileName)
    {
        List<WindowData> windowDatas = [];
        try
        {
            byte[] bytes = File.ReadAllBytes(fileName);
            var datas = MemoryPackSerializer.Deserialize<List<WindowDataPack>>(bytes);
            if (datas != null)
                foreach (var data in datas)
                    _windowDict[data.ProcessName] = 
                        new WindowData(
                            data.ProcessName,
                            Color.FromArgb(data.Color),
                            data.ActiveTime);
            return true;
        }
        catch
        {
            return false;
        }
    }

}
