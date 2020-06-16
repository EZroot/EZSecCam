using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    }
}
