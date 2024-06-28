using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
   private List<GameObject> activeTiles;
    public GameObject[] tilePrefabs;

    public float tileLength = 30;
    public int numberOfTiles = 3;
    public int totalNumOfTiles = 8;

    public float zSpawn = 0;

    public Transform playerTransform;

    public QuizAnswerManager quizManager;

    // void Start()
    // {
    //     // activeTiles = new List<GameObject>();
    //     // for (int i = 0; i < numberOfTiles; i++)
    //     // {
    //     //     if(i==0)
    //     //         SpawnTile(0);
    //     //     else if (i == 2)    
    //     //         {
    //     //         SpawnTile(tilePrefabs.Length-2);
            
    //     //         }
    //     //     else
    //     //         SpawnTile(Random.Range(0, tilePrefabs.Length-2));
    //     // }

    //     // playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    //     SpawnTile(0);

    // }

    private int cur = 0;

    public GameObject parent;

    private bool DoneSpawn = false;
    // void Update()
    // {   
    //     if (RunnerManager.gameCompleted || RunnerManager.gameOver || DoneSpawn)
    //     return;
    //       if (quizManager.checkEnd && !DoneSpawn){
    //              cur = 0;
    //               SpawnTile(tilePrefabs.Length-1);
    //               DoneSpawn = true;
    //               return;
    //         }


    //     if(playerTransform.position.z - 30 >= zSpawn - (numberOfTiles * tileLength))
    //     {
    //         // int index = Random.Range(0, totalNumOfTiles);
    //         // while(index == previousIndex)
    //         //     index = Random.Range(0, totalNumOfTiles);
          
    //         if (cur == 2)
    //         {
    //             SpawnTile(tilePrefabs.Length-2);
            

    //             cur = 0;
    //         }
    //         else{
    //          SpawnTile(Random.Range(0,tilePrefabs.Length-2));
    //           cur+=1;
    //         }
    //          DeleteTile();
             
    //         // SpawnTile(index);
    //     }

            
    // }

    public void SpawnTile(int index, Vector3 position, GameObject parent)
    {
        // GameObject tile = tilePrefabs[index];
        // if (tile.activeInHierarchy)
        //     tile = tilePrefabs[index + 8];

        // if(tile.activeInHierarchy)
        //     tile = tilePrefabs[index + 16];

        // tile.transform.position = Vector3.forward * zSpawn;
        // tile.transform.rotation = Quaternion.identity;
        // tile.SetActive(true);

        // activeTiles.Add(tile);
        // zSpawn += tileLength;
        // previousIndex = index;

        // GameObject go = Instantiate(tilePrefabs[index],transform.forward * zSpawn, transform.rotation);

        if (RunnerManager.gameCompleted || RunnerManager.gameOver || DoneSpawn)
        return;

            GameObject go;
          if (quizManager.checkEnd && !DoneSpawn){
                   go = Instantiate(tilePrefabs[tilePrefabs.Length-1]);
                   go.transform.localPosition = position;
                     go.transform.SetParent(parent.transform);
                  DoneSpawn = true;
                  return;
            }

      
        if (cur == 2)
        {
          
            go = Instantiate(tilePrefabs[tilePrefabs.Length-2]);
        
            cur = 0;
        }
        else {
            go = Instantiate(tilePrefabs[index]);
         
            cur+=1;
        }

            go.transform.localPosition = position;
            // go.transform.localScale = new Vector3(1,1,1);
            go.transform.SetParent(parent.transform,false);
        if (go!= null && go.gameObject.CompareTag("TileQuiz")){
               quizManager.QuizStart(go);
        }
        //  activeTiles.Add(go);
         
        //  zSpawn += tileLength;
        //  nextSpawnPoint = go.transform.GetChild(go.transform.childCount-1).transform.localPosition;
    }

    
    
    
   

    // public void SpawnTile(int index = 0)
    // {
    //     GameObject tile = Instantiate(tilePrefabs[index]);
      

    //     tile.transform.position = Vector3.forward * zSpawn;
    //     tile.transform.rotation = Quaternion.identity;
    //     tile.transform.SetParent(parent.transform,false);

    //       if (tile!= null && tile.gameObject.CompareTag("TileQuiz")){
    //            quizManager.QuizStart(tile);
    //     }


    //     activeTiles.Add(tile);
    //     zSpawn += tileLength;
    // }
    
    
    // private void DeleteTile()
    // {
    //     activeTiles[0].SetActive(false);
    //     activeTiles.RemoveAt(0);
    // }
}
