using UnityEngine;

public class BubbleGameManager : MonoBehaviour
{
    public GameObject[] prefabs; // Array to hold the prefabs to instantiate
    public float spawnInterval = 2.0f; // Interval in seconds between spawns
    public Vector3 spawnAreaMin; // Minimum bounds of the spawn area
    public Vector3 spawnAreaMax; // Maximum bounds of the spawn area

    void Start()
    {
        InvokeRepeating("SpawnPrefabs", 2.0f, spawnInterval);
    }

    void Update()
    {
        // Check if there are any touches
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Check if the touch began
            if (touch.phase == TouchPhase.Began)
            {
                // Convert touch position to a Ray
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // Perform a raycast
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Check if the hit object has a specific tag or component
                    if (hit.collider.CompareTag("Bubble"))
                    {
                        // Handle interaction with the object
                        InteractWithObject(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    void SpawnPrefabs()
    {
        int numPrefabsToSpawn = Random.Range(2, 4); // Randomly choose between 2 and 3 prefabs
        for (int i = 0; i < numPrefabsToSpawn; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            int randomPrefabIndex = Random.Range(0, prefabs.Length);
            Instantiate(prefabs[randomPrefabIndex], randomPosition, Quaternion.identity);
        }
    }

    void InteractWithObject(GameObject obj)
    {
        // Example interaction: log the object's name
        Debug.Log("Touched object: " + obj.name);

        // Example: Change the object's color to red
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }

        // Add more interaction logic here
    }
}
