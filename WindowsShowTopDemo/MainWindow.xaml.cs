using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace WindowsShowTopDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Window _hiddenWindow;

        public MainWindow()
        {
            InitializeComponent();

            //// 创建一个隐藏窗口
            //_hiddenWindow = new Window
            //{
            //    Width = 0,
            //    Height = 0,
            //    ShowInTaskbar = false,
            //    WindowStyle = WindowStyle.None // 确保不会显示在任务栏
            //};

            //_hiddenWindow.Show();

            //// 设置实际窗口的 Owner
            //this.Owner = _hiddenWindow;
            //当前登录的用户变化
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetWindowStyles(this);
        }
        const int GWL_EXSTYLE = -20;
        const int WS_EX_TOOLWINDOW = 0x00000080;

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOACTIVATE = 0x0010;
        const uint SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_TOP = new IntPtr(0);

        public static void SetWindowStyles(Window window)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            IntPtr hWnd = helper.Handle;

            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

            // 设置窗口样式为工具窗口, 不在任务栏显示
            exStyle |= WS_EX_TOOLWINDOW;
            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

            // 将窗口设置为顶层窗口
            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE | SWP_SHOWWINDOW);

            // 尝试将窗口置于所有窗口之上
            SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE | SWP_SHOWWINDOW);
        }
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                //解锁屏
                case SessionSwitchReason.SessionUnlock:
                    Topmost = true;
                    break;
                //锁屏
                case SessionSwitchReason.SessionLock:
                    Topmost = false;
                    break;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Owner = null;
            _hiddenWindow?.Close();
            this.Close();
        }
    }
}