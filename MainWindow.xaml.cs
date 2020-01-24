using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CopyX
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            /*定义定时器,TimeSpan最后一个值是时间，单位秒，3表示3秒检测一次*/
            showTimer.Tick += new EventHandler(SetNull);
            showTimer.Interval = new TimeSpan(0, 0, 0, 3);
            showTimer.Start();
            InitializeComponent();
        }
        /*两个字符串 tempStr str*/
        private static string str;
        private static string tempStr;
        /*实例化计时器*/
        private DispatcherTimer showTimer = new DispatcherTimer();
        /*刷新按钮*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*先判断剪切板是否为空*/
            if (Clipboard.GetDataObject().GetFormats().Length != 0)
            {
                tempStr = Clipboard.GetText();
                //Image image = Clipboard.GetImage();
                if (str != tempStr)
                {
                    str = tempStr;
                    listBox.Items.Add(str);
                    //listBox.Items[i] != tempStr
                }
            }
        }
        /*自动刷新*/
        public void SetNull(object sender, EventArgs e)
        {
            /*先判断剪切板是否为空*/
            if (Clipboard.GetDataObject().GetFormats().Length != 0)
            {
                tempStr = Clipboard.GetText();
                //Image image = Clipboard.GetImage();
                if (str != tempStr)
                {
                    str = tempStr;
                    listBox.Items.Add(str);
                    //listBox.Items[i] != tempStr
                }
            }
        }
        /*选中List项复制*/
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Clipboard.SetDataObject(listBox.SelectedItem);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();
        }
    }
}
