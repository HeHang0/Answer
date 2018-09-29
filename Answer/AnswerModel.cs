using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
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
            LeftTopTextX = WindowLeft;
            LeftTopTextY = WindowTop;
        });
        public ICommand RightTopPoint => new DelegateCommand(() =>
        {
            RightTopTextX = WindowLeft;
            RightTopTextY = WindowTop;
        });
        public ICommand LeftBottomPoint => new DelegateCommand(() =>
        {
            LeftBottomTextX = WindowLeft;
            LeftBottomTextY = WindowTop;
        });
        public ICommand RightBottomPoint => new DelegateCommand(() =>
        {
            RightBottomTextX = WindowLeft;
            RightBottomTextY = WindowTop;
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
                var img = CaptureScreen((int)LeftTopTextX, (int)LeftTopTextY, (int)RightTopTextX - (int)LeftTopTextX, (int)LeftBottomTextY - (int)LeftTopTextY);
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
                //var aa = HttpRequest.GetHttpResponse($"http://tapi.gfun.me/nsh/kj/api.php?text={ResultText.Substring(0, ResultText.Length > 10 ? 10 : ResultText.Length)}");
                //AnalyzeList.Clear();
                //AnalyzeList.AddRange(JsonConvert.DeserializeObject<ObservableCollection<AnalyzeResult>>(aa));
                //ResultText += aa;
            }
            catch (Exception ex)
            {
                ResultText += "报错,原因：" + ex.Message;
            }
        });

        public ObservableCollection<AnalyzeResult> AnalyzeList { get; set; } = new ObservableCollection<AnalyzeResult>();

        public Bitmap CaptureScreen(int x, int y, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(new Point(x, y), new Point(0, 0), bmp.Size);
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

        private double windowLeft = 0;
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

        private double windowTop = 0;
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

        private double leftTopTextX = 0;
        public double LeftTopTextX
        {
            get
            {
                return leftTopTextX;
            }
            set
            {
                leftTopTextX = value;
                OnPropertyChanged("LeftTopTextX");
            }
        }

        private double rightTopTextX = 0;
        public double RightTopTextX
        {
            get
            {
                return rightTopTextX;
            }
            set
            {
                rightTopTextX = value;
                OnPropertyChanged("RightTopTextX");
            }
        }

        private double leftBottomTextX = 0;
        public double LeftBottomTextX
        {
            get
            {
                return leftBottomTextX;
            }
            set
            {
                leftBottomTextX = value;
                OnPropertyChanged("LeftBottomTextX");
            }
        }

        private double rightBottomTextX = 0;
        public double RightBottomTextX
        {
            get
            {
                return rightBottomTextX;
            }
            set
            {
                rightBottomTextX = value;
                OnPropertyChanged("RightBottomTextX");
            }
        }

        private double leftTopTextY = 0;
        public double LeftTopTextY
        {
            get
            {
                return leftTopTextY;
            }
            set
            {
                leftTopTextY = value;
                OnPropertyChanged("LeftTopTextY");
            }
        }

        private double rightTopTextY = 0;
        public double RightTopTextY
        {
            get
            {
                return rightTopTextY;
            }
            set
            {
                rightTopTextY = value;
                OnPropertyChanged("RightTopTextY");
            }
        }

        private double leftBottomTextY = 0;
        public double LeftBottomTextY
        {
            get
            {
                return leftBottomTextY;
            }
            set
            {
                leftBottomTextY = value;
                OnPropertyChanged("LeftBottomTextY");
            }
        }

        private double rightBottomTextY = 0;
        public double RightBottomTextY
        {
            get
            {
                return rightBottomTextY;
            }
            set
            {
                rightBottomTextY = value;
                OnPropertyChanged("RightBottomTextY");
            }
        }
    }
}
