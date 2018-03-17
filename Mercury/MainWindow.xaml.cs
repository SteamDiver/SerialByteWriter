using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mercury
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort mySerialPort = new SerialPort();

        public MainWindow()
        {
            InitializeComponent();

            ComsCb.ItemsSource = SerialPort.GetPortNames();
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;

            mySerialPort.DataReceived += DataReceivedHandler;
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            OuputTB.Text += "In: " + mySerialPort.ReadExisting() + "/r/n";
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommandTb.Text.Trim() != "")
                {
                    byte[] res = new byte[0];

                    int sz = 0;
                    string[] a = CommandTb.Text.Split(' ');
                    for (int j = 0; j < a.Length; j++)
                    {
                        Array.Resize(ref res, ++sz);
                        res[sz - 1] = Convert.ToByte(a[j], 16);
                    }

                    mySerialPort.PortName = ComsCb.Text;
                    mySerialPort.Open();
                    mySerialPort.Write(res, 0, res.Length);
                    mySerialPort.Close();
                    OuputTB.Text += "Out: " + BitConverter.ToString(res) + "\r\n";
                    return;
                }
                if (CommandTextTb.Text.Trim() != "")
                {
                    mySerialPort.PortName = ComsCb.Text;
                    mySerialPort.Open();
                    mySerialPort.WriteLine(CommandTextTb.Text);
                    mySerialPort.Close();
                    OuputTB.Text += "Out: " + CommandTextTb.Text + "\r\n";
                    return;
                }
            }
            catch (Exception ex)
            {
                OuputTB.Text += ex.Message + "\r\n";
            }
        }

        private void ClearInpBtn_Click(object sender, RoutedEventArgs e)
        {
            CommandTb.Text = String.Empty;
        }

        private void ClearInpTextBtn_Click(object sender, RoutedEventArgs e)
        {
            CommandTextTb.Text = String.Empty;
        }
    }
}
