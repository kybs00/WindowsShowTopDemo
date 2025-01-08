using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChildWindowDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window _hiddenWindow;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 创建一个隐藏窗口
            _hiddenWindow = new Window
            {
                Width = 0,
                Height = 0,
                Topmost = true,
                ShowInTaskbar = false,
                WindowStyle = WindowStyle.None // 确保不会显示在任务栏
            };
            _hiddenWindow.Show();
            this.Owner = _hiddenWindow;
        }
    }
}