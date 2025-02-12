using System.Diagnostics;
using System.Reflection;
namespace fuworktimer;

internal static class Program
{
    private static Mutex mutex = null;

    public static string AppDir = Path.Combine(".");
    [STAThread]
    static void Main()
    {
        try
        {
            Debug.WriteLine(GetFileVersion());

            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "fuworktimer";
            string mutexName = $"Global\\{assemblyName}";
#if !DEBUG
            AppDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyName);
#endif

            if (!File.Exists(AppDir))
                Directory.CreateDirectory(AppDir);

            mutex = new Mutex(true, mutexName, out bool createdNew);

#if !DEBUG
            if (!createdNew)
            {
                MessageBox.Show(
                    "すでに起動しています",
                    assemblyName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
#endif
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        catch (Exception ex)
        {
            ErrorLog(ex);
        }
        finally
        {
            if (mutex != null)
            {
                try
                {
                    mutex.ReleaseMutex();
                }
                catch (ApplicationException)
                {
                }
                mutex.Dispose();
            }
        }
    }

    public static string? GetFileVersion()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        if (assembly == null) return null;

        var attribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
        return attribute?.Version;
    }




    public static void ErrorLog(Exception ex)
    {
        string filePath = Path.Combine(Program.AppDir, "error.log");
        try
        {
            using StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine("Date: " + DateTime.Now.ToString());
            writer.WriteLine("Error Message: " + ex.Message);
            writer.WriteLine("Stack Trace: " + ex.StackTrace);
            writer.WriteLine(new string('-', 40));
        }
        catch (Exception logEx)
        {
            MessageBox.Show("Error writing to log file: " + logEx.Message);
        }
        finally
        {
            MessageBox.Show("Error");
        }
    }
}

[AttributeUsage(AttributeTargets.Assembly)]
public class BuildVersionAttribute : Attribute
{
    public string BuildVersion { get; }

    public BuildVersionAttribute(string buildVersion)
    {
        BuildVersion = buildVersion;
    }
}