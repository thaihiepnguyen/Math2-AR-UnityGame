using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButtonOnClick : MonoBehaviour
{
   public void OnClickButton(){
    SceneHistory.GetInstance().PreviousScene();
   }
}
