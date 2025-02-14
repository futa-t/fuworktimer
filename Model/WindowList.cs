using System;
using System.Collections.Generic;
using System.Diagnostics;

using MemoryPack;

using static fuworktimer.Model.Win32;
using static fuworktimer.Utility.ColorUtil;

namespace fuworktimer.Model;

internal class WindowList
{
    readonly Dictionary<string, WindowData> _windowDict = [];

    public string? FocusWindow { get; set; } = null;

    internal List<WindowData> WindowDataList => _windowDict.Values.ToList();

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

    internal WindowData GetActiveWindow()
    {
        var activeProc = GetActiveWindowProcessInfo();
        if (activeProc == null) return new ("unknown", "unknown", Color.White);

        return WindowData.FromProcessInfo(activeProc);
    }

    internal WindowData GetActiveWindowData()
    {
        WindowData active = GetActiveWindow();

        if(GetWindowData(active.ProcessName) is not WindowData wd)
        {
            wd = active;
            _windowDict[active.ProcessName] = active;
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
            window.SessionTime = 0;
    }

    internal bool Save(string fileName) => WindowDataStorage.Save(_windowDict.Values, fileName);

    internal void Load(string fileName)
    {
        foreach (var data in WindowDataStorage.Load(fileName))
            _windowDict[data.ProcessName] = data;
    }
}

class WindowDataStorage
{
    public static bool Save(IEnumerable<WindowData> windowDatas, string fileName)
    {
        try
        {
            List<string> list = [];

            foreach (var item in windowDatas)
                list.Add(item.ToJson());

            File.WriteAllBytes(fileName, MemoryPackSerializer.Serialize(list));
            return true;
        }
        catch (Exception ex)
        {
            Program.ErrorLog(ex);
            return false;
        }
    }

    public static IEnumerable<WindowData> Load(string fileName)
    {
        List<WindowData> windowDatas = [];
        try
        {
            byte[] bytes = File.ReadAllBytes(fileName);
            var datas = MemoryPackSerializer.Deserialize<List<string>>(bytes);
            if (datas != null)
                foreach (var data in datas)
                    if (WindowData.FromJson(data) is WindowData wd) 
                        windowDatas.Add(wd);
        }
        catch (FileNotFoundException) { }
        catch (Exception ex)
        {
            Program.ErrorLog(ex);
        }
        return windowDatas;
    }
}
