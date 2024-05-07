using System;
using System.Collections.Generic;




[Serializable]
public class StoreDTO
{
   
    public List<SkinDTO> skins {get;set;}
    public List<FrameDTO> frames {get;set;}

    public List<TestDTO> tests {get; set;}


}


[Serializable]
public class SkinDTO
{
   
    public int skin_id {get; set; }

    public string mode_url { get; set;}

     public string image_url { get; set;}

     public int price {get;set;}

     public string skin_name {get;set;}

     public bool is_purchased { get; set;}

}

//[Serializable]
//public class TestDTO
//{
   
//    public int test_id {get; set; }
//    public int semester {get;set;}
//    public int time {get;set;}

//     public int price {get;set;}

//     public string test_name {get;set;}
//     public bool is_purchased { get; set;}

//}

[Serializable]
public class FrameDTO
{
   
  
    public int frame_id {get; set; }

     public string image_url { get; set;}

     public int price {get;set;}

     public string frame_name {get;set;}
     public bool is_purchased { get; set;}

}