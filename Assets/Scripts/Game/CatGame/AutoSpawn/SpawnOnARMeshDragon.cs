using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Random = UnityEngine.Random;

public class SpawnOnARMeshDragon : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] private float minVertsForSpawn;
    [SerializeField] private float scaler;

    
    // [Range(0,100)]
    [SerializeField] private int amount = 5;
    private GameObject spawnedObject;

    private MeshAnalyserDragon meshAnalyser;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>(); 

    private Mesh arMesh;
    ARRaycastManager m_RaycastManager; 

    Camera arCam; 

    // Start is called before the first frame update
    void Start()
    {
        // currentAmount = 0;
        // if(spawnLikelyHood == 0) return;
        meshAnalyser = GetComponent<MeshAnalyserDragon>();
        meshAnalyser.analysisDone += StartSpawning;
    }


    private void OnDestroy()
    {
        meshAnalyser.analysisDone -= StartSpawning;
        // currentAmount = 0;
    }

    void StartSpawning()
    {
        // Debug.Log(currentAmount);
        if(CatGameManager.GetInstance().currentAmount == amount)  return;
        arMesh = GetComponent<MeshFilter>().sharedMesh;


        // int spawnLikely =  Random.Range(0, 100 / spawnLikelyHood);
        // Debug.Log("Spawnlikely => " + spawnLikely);

        // if (spawnLikely != 0)
        // {
        //     return;
        // }

        if (arMesh.vertexCount > minVertsForSpawn &&
            meshAnalyser.IsGround)
        {
            Debug.Log("currentAmount: " + CatGameManager.GetInstance().currentAmount);
            InstantiateObject(GetRandomObject());
        }
        
    }
    GameObject GetRandomObject()
    {
        return spawnObjects[Random.Range(0, spawnObjects.Count)];
    }

    void InstantiateObject(GameObject obj)
    {
        Vector3 randomVector = GetRandomVector();
        if(!IsCreated(randomVector)){
            CatGameManager.GetInstance().currentAmount++;
            spawnedObject = Instantiate(obj, randomVector, Quaternion.identity);
            spawnedObject.transform.localScale *= scaler;
        }
    }
    private bool IsCreated(Vector3 position){
        float raycastDistance = 0.1f;
        if (Physics.Raycast(position, Vector3.forward, raycastDistance))
            return true;
        return false;
    }
    Vector3 GetRandomVector()
    {
        Vector3 highestVert = Vector3.zero;
        float highestY = Mathf.NegativeInfinity;

        foreach (var vert in arMesh.vertices)
        {
            if (vert.y > highestY)
            {
                highestY = vert.y;
                highestVert = transform.TransformPoint(vert);
            }
        }
        
        return highestVert;
    }
}
