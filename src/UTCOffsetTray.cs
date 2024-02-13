using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using YamlDotNet.Serialization;
using TimeZoneConverter;
using System.Linq;

public class UTCOffsetTray
{
    private NotifyIcon notifyIcon;
    private System.Threading.Timer timer;

    public UTCOffsetTray()
    {
        notifyIcon = new NotifyIcon
        {
            Visible = true
        };

        // Create a context menu and assign it to the NotifyIcon
        notifyIcon.ContextMenuStrip = Update_ContextMenu();

        // Update the time offset immediately and start a timer to update it every hour
        UpdateTimeOffset();
        timer = new System.Threading.Timer(_ => UpdateTimeOffset(), null, TimeSpan.FromHours(1), TimeSpan.FromHours(1));
    }

    public void UpdateTimeOffset()
    {
        TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
        double totalHours = offset.TotalHours;

        // Format the offset as a string, replacing '.' with '_' for half-hour and 45-minute offsets
        string offsetString = totalHours.ToString("+#;-#;0").Replace('.', '_');

        // Build the path to the icon file
        string iconFilePath = System.IO.Path.Combine(Application.StartupPath, $"images\\ICOs\\{offsetString}.ico");

        // Set the icon and text of the NotifyIcon object
        notifyIcon.Icon = new System.Drawing.Icon(iconFilePath);
        notifyIcon.Text = $"Current UTC offset: {totalHours} hours";
    }

    private ContextMenuStrip Update_ContextMenu()
    {
        var contextMenu = new ContextMenuStrip();

        // Add additional time zones to the context menu
        var config = ReadConfig();
        var sortedTimezones = config.AdditionalTimezones
            .Select(tz => new { Id = tz.Id, Offset = TZConvert.GetTimeZoneInfo(tz.Id).BaseUtcOffset.TotalHours })
            .OrderBy(tz => tz.Offset)
            .ToList();

        foreach (var timezone in sortedTimezones)
        {
            TimeZoneInfo timeZoneInfo;

            try
            {
                timeZoneInfo = TZConvert.GetTimeZoneInfo(timezone.Id);
                double offsetHours = timeZoneInfo.BaseUtcOffset.TotalHours;
                string offset = offsetHours == 0 ? "±0" : offsetHours.ToString("+#.0;-#.0").Replace(".0", string.Empty);
                string menuItemText = $"{offset} : {timezone.Id}";
                var menuItem = new ToolStripMenuItem(menuItemText);
                menuItem.Click += (sender, e) =>
                {
                    TimeZoneInfo timeZoneInfo = TZConvert.GetTimeZoneInfo(timezone.Id);
                    DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);
                    string formattedTime = currentTime.ToString(config.DateTimeFormatter);
                    var customMessageBox = new CustomMessageBox($"{formattedTime}", offset);
                    customMessageBox.ShowDialog();
                };
                contextMenu.Items.Add(menuItem);
            }
            catch (TimeZoneNotFoundException)
            {
                Console.WriteLine($"The time zone '{timezone.Id}' could not be found.");
            }
            catch (InvalidTimeZoneException)
            {
                Console.WriteLine($"The time zone '{timezone.Id}' contains invalid data.");
            }
        }

        // Add a close option to the context menu
        var closeMenuItem = new ToolStripMenuItem("Close");
        closeMenuItem.Click += CloseMenuItem_Click;
        contextMenu.Items.Add(closeMenuItem);

        return contextMenu;
    }

    private Config ReadConfig()
    {
        var deserializer = new DeserializerBuilder().Build();
        var configText = File.ReadAllText("config.yaml");
        return deserializer.Deserialize<Config>(configText);
    }

    private void CloseMenuItem_Click(object sender, EventArgs e)
    {
        // Dispose the NotifyIcon object to remove it from the system tray
        notifyIcon.Dispose();
        Application.Exit(); // Exit the application
    }
}