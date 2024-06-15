using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public class MeshChunkManager : MonoBehaviour
{
  public List<GameObject> _groundPrefabs;
  public float _groundNormalTolerance = 0.01f;

  public int _spawnMinVertexCount = 100;
  public int _despawnMaxVertexCount = 50;
  private MeshFilter _filter;
  private GameObject _number;

  private void Start()
  {
    _filter = GetComponent<MeshFilter>();

    // don't update the object if there's no mesh filter
    enabled = (bool)_filter;
  }

  private void Update()
  {
    int vertexCount = _filter.sharedMesh.vertexCount;
    if (vertexCount >= _spawnMinVertexCount && !(bool)_number)
    {
      // plant a plant! (might not succeed)
      _number = InstantiateObject(_filter.sharedMesh);
    }
  }

  private GameObject InstantiateObject(Mesh mesh)
  {
    Vector3 position;
    Vector3 normal;
    // if we find a suitable vertex, plop a plant at that location
    if (FindVertex(_filter.sharedMesh, out position, out normal))
    {
      GameObject prefab =  _groundPrefabs[Random.Range(0, _groundPrefabs.Count)];

      Quaternion rotation =  Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);

      // use local position/rotation because of the different coordinate system
      GameObject obj = Instantiate(prefab, transform, false);
      obj.transform.localPosition = position;
      obj.transform.localRotation = rotation;
      obj.transform.localScale = Vector3.zero;
   
      return obj;
    }

    return null;
  }

   private bool FindVertex(Mesh mesh, out Vector3 position, out Vector3 normal)
  {
    int v = Random.Range(0, mesh.vertexCount);
    position = mesh.vertices[v];
    normal = mesh.normals[v];
    bool ground = normal.y > 1.0f - _groundNormalTolerance && _groundPrefabs.Count > 0;

    return ground;
  }


}
