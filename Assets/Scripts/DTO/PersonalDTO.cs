using System;

[Serializable]
public class SkinsDTO
{
    public int skinId { get; set; }
    public string imageUrl { get; set; }
}

[Serializable]
public class FramesDTO
{
    public int frameId { get; set; }
    public string imageUrl { get; set; }
}



[Serializable]
public class PersonalDTO 
{
    public string username { get; set; }
    public int totalAchievement { get; set; }
    public int totalNote { get; set; }
    public string skinUrl { get; set; }
    public string frameUrl { get; set; }
    public SkinsDTO[] skinsPurchased { get; set; }
    public FrameDTO[] framesPurchased { get; set; }
}