using System;


[Serializable]
public class LoginEmailDTO
{
    public string email { get; set; }
    public string password { get; set; }
    public bool rememberMe { get; set; }
}


[Serializable]
public class LoginExternalDTO
{
    public string email { get; set; }
    public string uid { get; set; }
    public string token { get; set; }
    public string platform { get; set; }
    public bool rememberMe { get; set; }
}


[Serializable]
public class RegisterEmailDTO
{
    public string email { get; set; }
    public string password { get; set; }
}


[Serializable]
public class RegisterPhoneDTO
{
    public string phone { get; set; }
    public string password { get; set; }
}
