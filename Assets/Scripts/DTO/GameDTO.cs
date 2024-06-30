using System;

[Serializable]
public class GameDTO
{
    public GameConfig gameConfig { get; set; }
    public GameData[] gameData { get; set; }
}
[Serializable]
public class GameData
{
    public int id { get; set; }
    public string question { get; set; }
    public string answer { get; set; }
    public string right_answer { get; set; }
}

[Serializable]
public class GameConfig
{
    public int id { get; set; }
    public int game_type_id { get; set; }
    public int lesson_id { get; set; }
    public float? time { get; set; }
    public int question_count { get; set; }
    public string game_type_name { get; set; }
    public string game_type_name_vi { get; set; }
}
