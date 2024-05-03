using System;

[Serializable]
public class AchievementDTO
{
    public int id { get; set; }
    public string name { get; set; }
    public int status_type { get; set; }
    public int price {get; set;}
    public string image_url {get; set;}
}