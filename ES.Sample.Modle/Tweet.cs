using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//reference :
//https://github.com/elastic/elasticsearch-net

namespace ES.Sample.Model
{
    public class Tweet
    {
        public long Id { get; set; }
        public string User { get; set; }
        public DateTime PostDate { get; set; }
        public string Message { get; set; }
    }
}
