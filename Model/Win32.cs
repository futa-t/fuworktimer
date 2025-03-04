namespace fuworktimer.Model;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using Windows.Win32;
using Windows.Win32.Foundation;

public record ProcessInfo(string ProcessName, string Description, string FilePath);

public static class Win32
{
    public static ProcessInfo? GetActiveWindowProcessInfo()
    {
        try
        {
            HWND hWnd = PInvoke.GetForegroundWindow();
            unsafe
            {
                uint processId = 0;
                uint threadId = PInvoke.GetWindowThreadProcessId(hWnd,  &processId);
                using Process proc = Process.GetProcessById((int)processId);
                var pm = proc.MainModule;
                string? description = pm?.FileVersionInfo?.FileDescription;
                if (string.IsNullOrEmpty(description) ){
                    description = proc.ProcessName;
                }
                string filePath = pm?.FileName ?? String.Empty;
                return new ProcessInfo(proc.ProcessName, description, filePath);
            }
        }
        catch
        {
            return null;
        }

    }
}
