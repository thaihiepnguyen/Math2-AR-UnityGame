using System;

[Serializable]
public class NoteDTO
{
    public int note_id { get; set; }
    public int lesson_id { get; set; }
    public string note { get; set; }
}