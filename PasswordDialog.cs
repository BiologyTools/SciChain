using System;
using Gtk;

class PasswordDialog : Window
{
    private Entry passwordEntry;
    private Label statusLabel;

    public PasswordDialog() : base("Password Request")
    {
        SetDefaultSize(300, 150);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        // Create vertical box containera
        VBox vbox = new VBox(false, 10);
        vbox.BorderWidth = 15;

        // Add instruction label
        Label instructionLabel = new Label("Please enter your password:");
        instructionLabel.Xalign = 0;
        vbox.PackStart(instructionLabel, false, false, 0);

        // Create password entry field
        passwordEntry = new Entry();
        passwordEntry.Visibility = false; // Hide password characters
        passwordEntry.InvisibleChar = '•'; // Use bullet for hidden characters
        passwordEntry.Activated += OnPasswordEntered; // Enter key support
        vbox.PackStart(passwordEntry, false, false, 0);

        // Create button box
        HBox buttonBox = new HBox(true, 5);

        Button submitButton = new Button("Submit");
        submitButton.Clicked += OnPasswordEntered;
        buttonBox.PackStart(submitButton, true, true, 0);

        Button cancelButton = new Button("Cancel");
        cancelButton.Clicked += OnCancel;
        buttonBox.PackStart(cancelButton, true, true, 0);

        vbox.PackStart(buttonBox, false, false, 0);

        // Status label for feedback
        statusLabel = new Label("");
        statusLabel.ModifyFg(StateType.Normal, new Gdk.Color(200, 0, 0));
        vbox.PackStart(statusLabel, false, false, 0);

        Add(vbox);
        ShowAll();
    }

    private void OnPasswordEntered(object sender, EventArgs e)
    {
        string password = passwordEntry.Text;

        if (string.IsNullOrEmpty(password))
        {
            statusLabel.Text = "Password cannot be empty!";
            return;
        }

        // Here you would typically validate the password
        Console.WriteLine($"Password entered (length: {password.Length})");
        statusLabel.ModifyFg(StateType.Normal, new Gdk.Color(0, 150, 0));
        statusLabel.Text = "Password submitted successfully!";

        // Clear the password field for security
        passwordEntry.Text = "";
    }

    private void OnCancel(object sender, EventArgs e)
    {
        Application.Quit();
    }

}