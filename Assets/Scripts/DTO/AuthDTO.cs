using System;


[Serializable]
public class LoginEmailDTO
{
    public string email { get; set; }
    public string password { get; set; }
}


[Serializable]
public class LoginExternalDTO
{
    public string email { get; set; }
    public string uid { get; set; }
    public string token { get; set; }
    public string platform { get; set; }

}


[Serializable]
public class RegisterDTO
{
    public string email { get; set; }
    public string password { get; set; }
}
