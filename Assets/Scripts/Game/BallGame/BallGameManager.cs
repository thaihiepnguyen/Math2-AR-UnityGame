using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class BallGameManager : MonoBehaviour
{
    [SerializeField] ARMeshManager meshManager;
    private const float minDistance = 2f;
    GameObject trackables;
    [SerializeField] GameObject rimPrefab;
    [SerializeField] GameObject rims;
    int spawnCount = 4;
    private List<Vector3> spawnPositions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void SpawnTarget()
    {
        if (spawnCount <= 0)
        {
            return;
        }
        trackables = GameObject.Find("Trackables");

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
            if (!meshAnalyser.IsGround)
            {
                return;
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
                var target = Instantiate(rimPrefab, worldPosition, Quaternion.identity, trackables.transform);
                var arCamera = Camera.main;
                if (arCamera != null)
                {
                    Vector3 directionToCamera = arCamera.transform.position - target.transform.position;
                    directionToCamera.y = 0; // Keep the target upright
                    Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera);
                    target.transform.rotation = lookRotation;
                }
                target.transform.SetParent(rims.transform, true);
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
    // Update is called once per frame
    void Update()
    {
        
    }

    void updateUI() {
        
    }
}
