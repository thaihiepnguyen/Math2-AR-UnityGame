using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using static UnityEngine.GraphicsBuffer;

public class SpawnCollectibleOnStartGame : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ARMeshManager meshManager;
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private GameObject collectibles;
    List<Vector3> spawnPositions= new List<Vector3>();
    [SerializeField] private int spawnCount = 0;
    private float minDistance = 0.5f;
    
    void Start()
    {
        
    }
    public override void OnNetworkSpawn()
    {
        StartGameAR.OnStartHost += OnStartGame;
    }
    public override void OnNetworkDespawn()
    {
        StartGameAR.OnStartHost -= OnStartGame;
    }

    private void OnStartGame()
    {
        spawnCount = 4;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCount > 0 && IsHost)
        {
            Debug.Log("Spawn");
            SpawnTarget();
        }
    }
    void SpawnTarget()
    {
        // Lấy tất cả các lưới hiện tại
        IList<MeshFilter> meshes = meshManager.meshes;

        if (meshes.Count == 0) return;

        bool validPositionFound = false;
        int attempt = 0;
        const int maxAttempts = 100; // Giới hạn số lần thử

        while (!validPositionFound && attempt < maxAttempts)
        {
            attempt++;
            MeshFilter selectedMeshFilter = meshes[UnityEngine.Random.Range(0, meshes.Count)];
            if (selectedMeshFilter == null)
            {
                return;
            }
            Mesh selectedMesh = selectedMeshFilter.mesh;
            var meshAnalyser = selectedMeshFilter.gameObject.GetComponent<MeshAnalyser>();
            // Find the highest vertex in the selected mesh
            if (!meshAnalyser || !meshAnalyser.IsGround)
            {
                continue;
            }
            Vector3 highestVertexPosition = selectedMesh.vertices[0];
            foreach (Vector3 vertex in selectedMesh.vertices)
            {
                if (vertex.y > highestVertexPosition.y)
                {
                    highestVertexPosition = vertex;
                }
            }
            // Chuyển đổi vị trí từ không gian local sang không gian world
            Vector3 worldPosition = selectedMeshFilter.transform.TransformPoint(highestVertexPosition);

            // Kiểm tra khoảng cách với các vị trí đã spawn
            bool isValid = true;
            foreach (Vector3 spawnPos in spawnPositions)
            {
                if (Vector3.Distance(worldPosition, spawnPos) < minDistance)
                {
                    isValid = false;
                    break;
                }
            }

            // Nếu vị trí hợp lệ, spawn đối tượng mục tiêu
            if (isValid)
            {
                spawnPositions.Add(worldPosition);
                SpawnCollectServerRpc(worldPosition, Quaternion.identity, NetworkManager.Singleton.LocalClientId);
                //var target = Instantiate(collectiblePrefab, worldPosition, Quaternion.identity,collectibles.transform);
               
                //target.transform.SetParent(collectibles.transform, true);
                validPositionFound = true;
                spawnCount--;
                if (spawnCount == 0)
                {
                    updateUI();
                }
            }
        }

        if (!validPositionFound)
        {
            Debug.LogWarning("Không thể tìm thấy vị trí hợp lệ để spawn đối tượng mục tiêu.");
        }


    }
    [ServerRpc(RequireOwnership = false)]
    void SpawnCollectServerRpc(Vector3 positon, Quaternion rotation, ulong callerID)
    {
        GameObject character = Instantiate(collectiblePrefab, positon, rotation, collectibles.transform);

        var arCamera = Camera.main;
        if (arCamera != null)
        {
            Vector3 directionToCamera = arCamera.transform.position - character.transform.position;
            directionToCamera.y = 0; // Keep the target upright
            Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera);
            character.transform.rotation = lookRotation;
        }
        NetworkObject characterNetworkObject = character.GetComponent<NetworkObject>();

        characterNetworkObject.SpawnWithOwnership(callerID);

        
    }

void updateUI()
    {
        Debug.Log("update ui");
    }

}
