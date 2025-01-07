using System.Runtime.InteropServices;
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
using Timer = System.Timers.Timer;

namespace Win32TopmostDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private nint _handle;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _handle = new WindowInteropHelper(this).Handle;
            SetWindowPos(_handle, -1, 0, 0, 0, 0, 1);
            //定时器置顶
            var timer = new Timer();
            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            SetWindowPos(_handle, -1, 0, 0, 0, 0, 1);
        }

        /// <summary>设置窗口位置</summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hWndInsertAfter">跟随的窗口句柄</param>
        /// <param name="x">X轴坐标</param>
        /// <param name="y">Y轴坐标</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="uFlags">标志位</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int x, int y, int width, int height, uint uFlags);
    }
}