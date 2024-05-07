using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class QuestionResultDTO
{
    public int question_result_id { get; set; }
    public int test_result_id { get; set; }
    public int exercise_id { get; set;}
    public string user_answer { get; set; }
    
}

