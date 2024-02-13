using System;
using System.Drawing;
using System.Windows.Forms;

public partial class CustomMessageBox : Form
{
    private Label messageLabel;
    private const int displayOffset = 400;

    public CustomMessageBox(string message, string offset)
    {
        // Create the label
        messageLabel = new Label
        {
            AutoSize = true,
            Location = new Point(13, 13),
            Text = message,
            Font = new Font(FontFamily.GenericSansSerif, 20),
        };
        Controls.Add(messageLabel);

        Icon = new Icon($"images/ICOs/{offset}.ico");

        // Set the size of the form
        ClientSize = new Size(
            messageLabel.Width + 26,
            messageLabel.Height + 33
        );

        // Set the start position of the form to manual so we can set it ourselves
        StartPosition = FormStartPosition.Manual;

        // Get the screen where the form is currently located
        Screen screen = Screen.FromControl(this);

        // Check if the screen resolution is low
        if (screen.Bounds.Width <= 800 && screen.Bounds.Height <= 600)
        {
            // If the resolution is low, center the form
            StartPosition = FormStartPosition.CenterScreen;
        }
        else
        {
            // If the resolution is not low, position the form at the bottom right of the screen
            StartPosition = FormStartPosition.Manual;
            Location = new Point(screen.WorkingArea.Right - Width - displayOffset, screen.WorkingArea.Bottom - Height - displayOffset);
        }
    }
}