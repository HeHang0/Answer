using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Answer
{
    public class CapturePointModel : INotifyPropertyChanged
    {
        public CapturePointModel()
        {

        }

        public CapturePointModel(Point leftTopText, Point rightTopText, Point leftBottomText, Point rightBottomText)
        {
            this.leftTopText = leftTopText;
            this.rightTopText = rightTopText;
            this.leftBottomText = leftBottomText;
            this.rightBottomText = rightBottomText;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Thickness margin = new Thickness(0,0,0,0);
        public Thickness Margin {
            get
            {
                return margin;
            }
            set
            {
                margin = value;
                OnPropertyChanged("Margin");
            }
        }

        private int width = 0;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        private int height = 0;
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }
        private bool isCloseWindow = true;
        public bool IsCloseWindow
        {
            get
            {
                return isCloseWindow;
            }
            set
            {
                isCloseWindow = value;
                OnPropertyChanged("IsCloseWindow");
            }
        }

        private bool isLeftButtonDown = false;
        private Point leftTopText;
        private Point rightTopText;
        private Point leftBottomText;
        private Point rightBottomText;

        public ICommand MouseDoubleClick => new DelegateCommand(() =>
        {
            leftTopText.X = (int)Margin.Left;
            leftTopText.Y = (int)Margin.Top;
            rightTopText.X = (int)Margin.Left + Width;
            rightTopText.Y = (int)Margin.Top;
            leftBottomText.X = (int)Margin.Left;
            leftBottomText.Y = (int)Margin.Top + Height;
            rightBottomText.X = (int)Margin.Left + Width;
            rightBottomText.Y = (int)Margin.Top + Height;
            IsCloseWindow = false;
        });
        public ICommand MouseLeftButtonDown => new DelegateCommand(() =>
        {
            MPoint p = new MPoint();
            GetCursorPos(out p);
            Margin = new Thickness(p.X, p.Y, 0, 0);
            isLeftButtonDown = true;
        });
        public ICommand MouseLeftButtonUp => new DelegateCommand(() =>
        {
            isLeftButtonDown = false;
        });
        public ICommand MouseRightButtonUp => new DelegateCommand(() =>
        {
            IsCloseWindow = false;
        });
        public ICommand MouseMove => new DelegateCommand(() =>
        {
            if (isLeftButtonDown)
            {
                MPoint p = new MPoint();
                GetCursorPos(out p);
                Width = p.X - (int)Margin.Left-1;
                Height = p.Y - (int)Margin.Top-1;
            }
        });


        /// <summary>
        /// 获取鼠标位置
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out MPoint pt);
        public struct MPoint
        {
            public int X;
            public int Y;
            public MPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
    }
}
