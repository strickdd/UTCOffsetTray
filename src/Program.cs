using System;
using System.Windows.Forms;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        UTCOffsetTray trayIcon = new UTCOffsetTray();
        Application.Run(); // This will run an empty form to keep the application alive
    }
}