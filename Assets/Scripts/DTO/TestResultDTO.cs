using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class TestResultDTO
{
    public int test_result_id { get; set; }
    public int user_id { get; set;}
    public int test_id { get; set; }
    public string? completed_time { get; set; }
    public string? point { get; set; }

    public DateTime? date { get; set; }

    

}

