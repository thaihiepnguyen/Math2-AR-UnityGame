using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class TestDTO
{
    public int test_id { get; set; }
    public int semester { get; set;}
    public int time { get; set; }
    public string test_name { get; set; }
    public int price { get; set; }
    public string status { get; set; }
    public bool is_purchased { get; set; }

}

