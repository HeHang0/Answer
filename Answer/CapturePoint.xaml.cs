using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Answer
{
    /// <summary>
    /// CapturePoint.xaml 的交互逻辑
    /// </summary>
    public partial class CapturePoint : Window
    {
        public CapturePoint(Point leftTopText, Point rightTopText, Point leftBottomText, Point rightBottomText)
        {
            InitializeComponent();
            Height = SystemParameters.WorkArea.Height;//获取屏幕的宽高  使之不遮挡任务栏  
            Width = SystemParameters.WorkArea.Width;
            Top = 0;
            Left = 0;
            DataContext = new CapturePointModel(leftTopText, rightTopText, leftBottomText, rightBottomText);
        }

        private void Grid_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
            {
                Close();
            }
        }
    }
}
