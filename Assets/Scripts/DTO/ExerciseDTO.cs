using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExerciseDTO 
{
    // Start is called before the first frame update
    public int exercise_id { get; set; }
    public int lesson_id { get; set; }
    public string question { get; set; }
    public string answer { get; set; }
    public string right_answer { get; set; }
    public string type { get; set; }
    public int? test_id { get; set; }
    public string image_url { get; set; }
  
}

[Serializable]
public class ExerciseResponseDTO
{
    public string message;
    public List<ExerciseDTO> data;
}