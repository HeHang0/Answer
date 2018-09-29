using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Answer
{
    public class AnalyzeHelper
    {
        public string log_id { get; set; }
        public string words_result_num { get; set; }
        public List<Content> words_result { get; set; }
    }

    public class Content
    {
        public string words { get; set; }
    }

    public class AnalyzeResult
    {
        public string id { get; set; }
        public string pinyin { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
    }
}
