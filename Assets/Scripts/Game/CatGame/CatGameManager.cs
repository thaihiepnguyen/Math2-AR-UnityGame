using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] private int amount;
    [SerializeField] private float scaler;
    [SerializeField] private Camera mainCam;

    private GameObject spawnedObject;

    GameObject GetRandomObject()
    {
        return spawnObjects[Random.Range(0, spawnObjects.Count)];
    }
    void InstantiateObject(GameObject obj)
    {
        
        spawnedObject = Instantiate(obj, GetRandomVector(), Quaternion.identity);
        spawnedObject.transform.localScale *= scaler;
    }
    Vector3 GetRandomVector()
    {
        Vector3 randomVector = new Vector3(
            mainCam.transform.position.x + Random.Range(-2f, 2f), 
            mainCam.transform.position.y + Random.Range(-2f, -0.5f), 
            mainCam.transform.position.z + Random.Range(0.5f, 2f)
        );
        while(IsCreated(randomVector))
            randomVector = new Vector3(
                mainCam.transform.position.x + Random.Range(-2f, 2f), 
                mainCam.transform.position.y + Random.Range(-2f, -0.5f), 
                mainCam.transform.position.z + Random.Range(-2f, 2f)
            );
        return randomVector;
    }
    private bool IsCreated(Vector3 position){
        float raycastDistance = 0.1f;
        if (Physics.Raycast(position, Vector3.forward, raycastDistance))
            return true;
        return false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariable.currentAmount = 0;
        for(int i = 0; i < amount; i++){
            InstantiateObject(GetRandomObject());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
