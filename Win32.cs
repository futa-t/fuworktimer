namespace fuworktimer;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;


public static class Win32
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    public static string GetActiveWindowText()
    {
        IntPtr hWnd = GetForegroundWindow();
        int nMaxCount = 256;
        StringBuilder lpString = new StringBuilder(nMaxCount);
        GetWindowText(hWnd, lpString, nMaxCount);
        return lpString.ToString();
    }

    public static string GetActiveWindowProcessName()
    {
        IntPtr hWnd = GetForegroundWindow();
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
