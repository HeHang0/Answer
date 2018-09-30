using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Answer
{
    public class AnswerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public ICommand LeftTopPoint => new DelegateCommand(() =>
        {
            LeftTopText.X = (int)WindowLeft;
            LeftTopText.Y = (int)WindowTop;
        });
        public ICommand RightTopPoint => new DelegateCommand(() =>
        {
            RightTopText.X = (int)WindowLeft;
            RightTopText.Y = (int)WindowTop;
        });
        public ICommand LeftBottomPoint => new DelegateCommand(() =>
        {
            LeftBottomText.X = (int)WindowLeft;
            LeftBottomText.Y = (int)WindowTop;
        });
        public ICommand RightBottomPoint => new DelegateCommand(() =>
        {
            RightBottomText.X = (int)WindowLeft;
            RightBottomText.Y = (int)WindowTop;
        });
        public ICommand RunAnalyzeResult => new DelegateCommand(() =>
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ResultText))
                {
                    var aa = HttpRequest.GetHttpResponse($"http://tapi.gfun.me/nsh/kj/api.php?text={ResultText.Substring(0, ResultText.Length > 10 ? 10 : ResultText.Length)}");
                    AnalyzeList.Clear();
                    AnalyzeList.AddRange(JsonConvert.DeserializeObject<ObservableCollection<AnalyzeResult>>(aa));
                }
            }
            catch (Exception)
            {
            }
        });
        public ICommand OCRResult => new DelegateCommand(() =>
        {
            try
            {
                var API_KEY = "Yzf5DWMDEab8OddGCe1kCdxA";
                var SECRET_KEY = "vys5QWFCD5293VUXK7GGIa8wHPpL4PI9";
                var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
                client.Timeout = 60000;
                var img = CaptureScreen(LeftTopText.X, LeftTopText.Y, RightTopText.X - LeftTopText.X, LeftBottomText.Y - LeftTopText.Y);
                var image = getBytesByBitmap(img);//System.IO.File.ReadAllBytes("D:\\1.png");
                var result = client.GeneralBasic(image);
                var a = JsonConvert.DeserializeObject<AnalyzeHelper>(result.ToString());
                ResultText = "";
                foreach (var item in a.words_result)
                {
                    var text = item.words;
                    if (IsCancelPunctuation)
                    {
                        text = Regex.Replace(text, "[ \\[ \\] \\^ \\-_*×――(^)（^）$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;\"‘’“”-]", "");
                    }
                    ResultText += text;
                }
            }
            catch (Exception ex)
            {
                ResultText += "报错,原因：" + ex.Message;
            }
        });
        public ICommand CapturePoint => new DelegateCommand(() =>
        {
            new CapturePoint(LeftTopText, RightTopText, LeftBottomText, RightBottomText).Show();
        });

        public ObservableCollection<AnalyzeResult> AnalyzeList { get; set; } = new ObservableCollection<AnalyzeResult>();

        public Bitmap CaptureScreen(int x, int y, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(new System.Drawing.Point(x, y), new System.Drawing.Point(0, 0), bmp.Size);
                g.Dispose();
            }
            return bmp;
        }

        public byte[] getBytesByBitmap(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以，至于区别么，下面有解释
            ms.Close();
            return bytes;
        }

        private bool isCancelPunctuation = true;
        public bool IsCancelPunctuation
        {
            get
            {
                return isCancelPunctuation;
            }
            set
            {
                isCancelPunctuation = value;
                OnPropertyChanged("IsCancelPunctuation");
            }
        }

        private string resultText = "";
        public string ResultText
        {
            get
            {
                return resultText;
            }
            set
            {
                resultText = value;
                OnPropertyChanged("ResultText");
            }
        }

        private double windowLeft = SystemParameters.WorkArea.Width*0.4;
        public double WindowLeft
        {
            get
            {
                return windowLeft;
            }
            set
            {
                windowLeft = value;
                OnPropertyChanged("WindowLeft");
            }
        }

        private double windowTop = SystemParameters.WorkArea.Height*0.4;
        public double WindowTop
        {
            get
            {
                return windowTop;
            }
            set
            {
                windowTop = value;
                OnPropertyChanged("WindowTop");
            }
        }
        
        public Point LeftTopText { get; } = new Point(0,0);
        public Point RightTopText { get; } = new Point((int)SystemParameters.WorkArea.Width,0);
        public Point LeftBottomText { get; } = new Point(0,(int)SystemParameters.WorkArea.Height);
        public Point RightBottomText { get; } = new Point((int)SystemParameters.WorkArea.Width, (int)SystemParameters.WorkArea.Height);
    }
}
