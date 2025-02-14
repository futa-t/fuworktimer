namespace fuworktimer.Model;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;


public static class Win32
{
    [DllImport("user32.dll")]
    private static extern nint GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(nint hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(nint hWnd, out uint processId);

    public static string GetActiveWindowText()
    {
        nint hWnd = GetForegroundWindow();
        int nMaxCount = 256;
        StringBuilder lpString = new StringBuilder(nMaxCount);
        GetWindowText(hWnd, lpString, nMaxCount);
        return lpString.ToString();
    }

    public static string GetActiveWindowProcessName()
    {
        nint hWnd = GetForegroundWindow();
        GetWindowThreadProcessId(hWnd, out uint processId);
        try
        {
            Process proc = Process.GetProcessById((int)processId);
            return proc.ProcessName;
        }
        catch
        {
            return "Unknown";
        }
    }

}
