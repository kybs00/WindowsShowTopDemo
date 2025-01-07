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

            // 创建一个隐藏窗口
            _hiddenWindow = new Window
            {
                Width = 0,
                Height = 0,
                ShowInTaskbar = false,
                WindowStyle = WindowStyle.None // 确保不会显示在任务栏
            };

            _hiddenWindow.Show();

            // 设置实际窗口的 Owner
            this.Owner = _hiddenWindow;
            //当前登录的用户变化
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetShowInTaskbar(this, false);
        }
        const int GWL_EXSTYLE = -20;
        const int WS_EX_APPWINDOW = 0x00040000;
        const int WS_EX_TOOLWINDOW = 0x00000080;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_FRAMECHANGED = 0x0020;

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public static void SetShowInTaskbar(Window window, bool showInTaskbar)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            IntPtr hWnd = helper.Handle;

            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

            if (showInTaskbar)
            {
                exStyle |= WS_EX_APPWINDOW;
                exStyle &= ~WS_EX_TOOLWINDOW;
            }
            else
            {
                exStyle |= WS_EX_TOOLWINDOW;
                exStyle &= ~WS_EX_APPWINDOW;
            }

            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

            // 强制窗口样式更新
            SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOZORDER | SWP_FRAMECHANGED);
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
            _hiddenWindow.Close();
            this.Close();
        }
    }
}