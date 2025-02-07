using System.Reflection;
namespace fuworktimer;

internal static class Program
{
    private static Mutex mutex = null;

    public static string AppDir = null;
    [STAThread]
    static void Main()
    {
        try
        {

            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string mutexName = $"Global\\{assemblyName}";
#if DEBUG
            AppDir = ".";
#else
            AppDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyName);
#endif

            if (!File.Exists(AppDir))
                Directory.CreateDirectory(AppDir);

            mutex = new Mutex(true, mutexName, out bool createdNew);

#if !DEBUG
            if (!createdNew)
            {
                MessageBox.Show(
                    "ä˘Ç…ãNìÆÇµÇƒÇ¢Ç‹Ç∑ÅB",
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
}