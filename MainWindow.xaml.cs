using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
            /*改变关闭动作*/
            InitialTray();
        }
        /*两个字符串 tempStr str*/
        private static string str;
        private static string tempStr;
        /*实例化计时器*/
        private DispatcherTimer showTimer = new DispatcherTimer();
        /*资源锁*/
        private static Mutex mut = new Mutex();
        /*小化到托盘*/
        private System.Windows.Forms.NotifyIcon _notifyIcon = null;
        /*刷新按钮*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mut.WaitOne();
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
            mut.ReleaseMutex();
        }
        /*自动刷新*/
        public void SetNull(object sender, EventArgs e)
        {
            mut.WaitOne();
            /*先判断剪切板是否为空*/
            if (Clipboard.GetDataObject().GetFormats().Length != 0)
            {
                tempStr = Clipboard.GetText();
                if (str != tempStr)
                {
                    str = tempStr;
                    listBox.Items.Add(str);
                    /*去除重复项*/
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        for (int j = i + 1; j < listBox.Items.Count; j++)
                        {
                            if (listBox.Items[i].Equals(listBox.Items[j]))
                                listBox.Items.Remove(listBox.Items[j]);
                        }
                    }
                }
            }
            mut.ReleaseMutex();
        }
        /*选中List项复制*/
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(listBox.SelectedItem);
            }
            catch
            {
                
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();
        }

        /*最小化系统托盘*/
        private void InitialTray()
        {
            /*隐藏主窗体*/
            this.Visibility = Visibility.Hidden;
            /*设置托盘的各个属性*/
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.BalloonTipTitle = "CopyX-Lite缩小至托盘，开始检测剪切板";
            _notifyIcon.BalloonTipText = "开源地址(Github):https://github.com/hanbings/CopyX-Lite";
            /*托盘气泡显示内容*/
            _notifyIcon.Text = "CopyX-Lite";
            _notifyIcon.Visible = true;
            /*托盘按钮是否可见*/
            _notifyIcon.Icon = new Icon(@"clipboard_128px_1230181_easyicon.net.ico");
            /*托盘中显示的图标*/
            _notifyIcon.ShowBalloonTip(2000);
            /*托盘气泡显示时间*/
            _notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
            /*窗体状态改变时触发*/
            this.StateChanged += MainWindow_StateChanged;
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }
        /*托盘图标单击弹出窗口*/
        private void notifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    this.Activate();
                }
            }
        }
    }
}
