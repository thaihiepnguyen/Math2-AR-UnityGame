using System;

[Serializable]
public class AchievementDTO
{
    public int id { get; set; }
    public string name { get; set; }
    public bool is_completed { get; set; }
}