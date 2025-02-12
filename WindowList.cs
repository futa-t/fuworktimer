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

internal partial class WindowList
{
    readonly Dictionary<string, WindowData> _windowDict = [];

    public string? FocusWindow { get; set; } = string.Empty;

    internal static WindowList FromDailySaveFile()
    {
        WindowList windowList = new();
        windowList.Load();
        return windowList;
    }

    internal WindowData? GetWindowData(string processName)
    {
        _windowDict.TryGetValue(processName, out var wd);
        return wd;
    }

    internal WindowData GetActiveWindw()
    {
        string activeProc = GetActiveWindowProcessName();

        if (GetWindowData(activeProc) is not WindowData wd)
        {
            wd = new WindowData(activeProc, GetColorFromText(activeProc));
            _windowDict[activeProc] = wd;
        }
        return wd;
    }

    internal WindowData? GetFocusWindow()
    {
        if (FocusWindow == null) return null;

        return GetWindowData(FocusWindow);
    }

    internal void ResetSessionTime()
    {
        foreach (var window in _windowDict.Values)
            window.ActiveTimeSession = 0;
    }

}


// Data Store
internal partial class WindowList { 
    
    string saveDir = Program.AppDir;

    string DailySaveFileName()
    {
        string today = DateTime.Now.ToString("yyMMdd");

        return Path.Combine(saveDir, $"{today}.pack");
    }

    internal bool Save(string? fileName = null)
    {
        try
        {
            string fname = fileName ?? DailySaveFileName();
            List<WindowDataPack> list = [];

            foreach (var item in _windowDict.Values)
                list.Add(new WindowDataPack(item.ProcessName, item.Color.ToArgb(), item.ActiveTimeTotal));

            File.WriteAllBytes(fname, MemoryPackSerializer.Serialize(list));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    internal bool Load(string? fileName = null)
    {
        string fname = fileName ?? DailySaveFileName();
        List<WindowData> windowDatas = [];
        try
        {
            byte[] bytes = File.ReadAllBytes(fname);
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
