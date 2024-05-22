using System;

[Serializable]
public class SkinsDTO
{
    public int skinId { get; set; }
    public int? imageSkinId { get; set; }
}

[Serializable]
public class FramesDTO
{
    public int frameId { get; set; }
    public int? imageFrameId { get; set; }
}



[Serializable]
public class PersonalDTO 
{
    public string username { get; set; }
    public int totalAchievement { get; set; }
    public int totalNote { get; set; }
    public int? imageSkinId { get; set; }
    public int? imageFrameId { get; set; }
    public SkinsDTO[] skinsPurchased { get; set; }
    public FramesDTO[] framesPurchased { get; set; }
}