using System.Windows;

namespace EZSecCam
{
    /// <summary>
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        public ConnectionWindow()
        {
            InitializeComponent();

            ConnectionSettings.ReadConfig();
            IPInputTextBox.Text = ConnectionSettings.ServerIp;
            PublicInputTextBox.Text = ConnectionSettings.PublicKey;
            PublicExponentInputTextBox.Text = ConnectionSettings.PublicKeyExp;
        }

        public void ApplySettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            ConnectionSettings.ServerIp = IPInputTextBox.Text;
            ConnectionSettings.PublicKey = PublicInputTextBox.Text;
            ConnectionSettings.PublicKeyExp = PublicExponentInputTextBox.Text;
            ConnectionSettings.WriteConfig();
        }

        public void ConnectSettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Client.Connect();
        }
    }
}
