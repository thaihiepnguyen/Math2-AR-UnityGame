using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    // Start is called before the first frame update

    TileManager tileManager;
    void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         if (!RunnerManager.isGameStarted || RunnerManager.gameCompleted || RunnerManager.gameOver)
        return;
        // if (other.transform.tag == "Player"){
            transform.localPosition -= new Vector3(0,0,5f)* Time.fixedDeltaTime;
        // }
    }

       void OnTriggerExit(Collider other)
    {

          tileManager.SpawnTile(Random.Range(0,tileManager.tilePrefabs.Length-2),transform.localPosition + transform.GetChild(transform.childCount-1).transform.localPosition*2,transform.parent.gameObject);
           Destroy(gameObject,2f);
    }

  
    // void OnTriggerStay(Collider other)
    // {
        
    //     if (!RunnerManager.isGameStarted || RunnerManager.gameCompleted || RunnerManager.gameOver)
    //     return;
    //     if (other.transform.tag == "Player"){
    //         transform.position-= new Vector3(0,0,5f)* Time.fixedDeltaTime;
    //     }
    // }



   
}
