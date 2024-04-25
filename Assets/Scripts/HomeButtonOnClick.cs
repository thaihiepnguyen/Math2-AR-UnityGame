using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtonOnClick : MonoBehaviour
{
   public void OnClickButton(){
   //  SceneManager.LoadScene("Main");

   SceneHistory.GetInstance().OnClickHomeButton();
   }
}
