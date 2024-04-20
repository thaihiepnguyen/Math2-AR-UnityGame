using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LessonDTO
{
    public int lesson_id { get; set; }
    public string chapter { get; set; }
    public int semester { get; set; }
    public string name { get; set; }
    public string video_url { get; set; }
    public string book_url { get; set; }
    public string ar_url { get; set; }
  
}

[Serializable]

public class ChapterDTO {
    public string chapter {get; set;}
}
