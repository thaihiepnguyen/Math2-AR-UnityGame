using System;

[Serializable]
public class UserDTO
{
    public int user_id { get; set; }
    public string email { get; set; }
    public int eventpint { get; set; }
    public int star { get; set; }
    public int coin { get; set; }
    public bool is_valid { get; set; }
    public string name { get; set; }
}