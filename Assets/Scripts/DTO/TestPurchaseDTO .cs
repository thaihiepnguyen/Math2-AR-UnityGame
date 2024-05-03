using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class TestPurchaseDTO
{
    public int test_purchase_id { get; set; }
    public int test_id { get; set; }
    public int user_id { get; set;}
    public DateTime purchased_time { get; set; }

}

