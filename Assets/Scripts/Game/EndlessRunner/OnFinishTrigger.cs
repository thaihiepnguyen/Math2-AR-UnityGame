using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFinishTrigger : MonoBehaviour
{
  
    void OnTriggerEnter(Collider other)
    {
            if (other.transform.tag == "Player"){
                Debug.Log("hello");

            StartCoroutine(End());
          
          
        }
    }

    private IEnumerator End(){
       
        yield return new WaitForSeconds(1.2f);
        RunnerManager.gameCompleted = true;
    }
}
