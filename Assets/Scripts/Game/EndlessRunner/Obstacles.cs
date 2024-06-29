using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
     
        void OnCollisionEnter(Collision other)
        {
              if (other.transform.tag == "Player"){
                RunnerManager.gameOver = true;
        }
        } 
}
