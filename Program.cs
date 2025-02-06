using System.Reflection;

namespace fuworktimer;

internal static class Program
{
    private static Mutex mutex = null;

    [STAThread]
    static void Main()
    {
        try
        {

        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        string mutexName = $"Global\\{assemblyName}";

        mutex = new Mutex(true, mutexName, out bool createdNew);

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
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
        }   finally
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