using System;


[Serializable]
public class LoginDTO
{
    public string email { get; set; }
    public string password { get; set; }
}


[Serializable]
public class RegisterDTO
{
    public string email { get; set; }
    public string password { get; set; }
}
