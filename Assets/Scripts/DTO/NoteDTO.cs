using System;

[Serializable]
public class NoteDTO
{
    public int note_id { get; set; }
    public int lesson_id { get; set; }
    public string note { get; set; }
    public string lesson_name { get; set; }
    public NoteDTO(int note_id, int lesson_id, string note, string lesson_name)
    {
        this.note_id = note_id;
        this.lesson_id = lesson_id;
        this.note = note;
        this.lesson_name = lesson_name;
    }
}