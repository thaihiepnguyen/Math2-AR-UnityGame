using System;


[Serializable]
public struct BaseDTO<T>
{
    public string message { get; set; }
    public T data { get; set; }
    public bool isSuccessful { get; set; }
}
