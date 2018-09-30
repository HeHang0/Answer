using System.ComponentModel;


namespace Answer
{
    public class Point : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        private int x = 0;
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }

        private int y = 0;
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }
    }
}
