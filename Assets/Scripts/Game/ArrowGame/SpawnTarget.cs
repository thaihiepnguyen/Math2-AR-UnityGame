using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTarget : MonoBehaviour
{
    // Start is called before the first frame update
   public List<GameObject> _groundPrefabs;
  public List<GameObject> _wallPrefabs;
  public float _groundNormalTolerance = 0.01f;
  public float _wallNormalTolerance = 0.001f;
  public int _spawnMinVertexCount = 100;
  public int _despawnMaxVertexCount = 50;
  public float _growthDuration = 4.0f;
  private MeshFilter _filter;
  private GameObject _plant;

  private void Start()
  {
    _filter = GetComponent<MeshFilter>();

    // don't update the object if there's no mesh filter
    enabled = (bool)_filter;
  }

  private void Update()
  {
    int vertexCount = _filter.sharedMesh.vertexCount;
    if (vertexCount >= _spawnMinVertexCount && !(bool)_plant)
    {
      // plant a plant! (might not succeed)
      _plant = InstantiatePlant(_filter.sharedMesh);
    }
    else if (vertexCount <= _despawnMaxVertexCount && (bool)_plant)
    {
      // pull a plant!
      StopCoroutine(GrowPlant());
      Destroy(_plant);
      _plant = null;
    }
  }

  private GameObject InstantiatePlant(Mesh mesh)
  {
    bool wall;
    Vector3 position;
    Vector3 normal;
    // if we find a suitable vertex, plop a plant at that location
    if (FindVertex(_filter.sharedMesh, out wall, out position, out normal))
    {
      GameObject prefab = wall
        ? _wallPrefabs[Random.Range(0, _wallPrefabs.Count)]
        : _groundPrefabs[Random.Range(0, _groundPrefabs.Count)];

      Quaternion rotation = wall
        ? Quaternion.LookRotation(normal, Vector3.zero)
        : Quaternion.Euler(0, 0, 0);

      // use local position/rotation because of the different coordinate system
      GameObject plant = Instantiate(prefab, transform, false);
      plant.transform.localPosition = position;
      plant.transform.localRotation = rotation;
      plant.transform.localScale = Vector3.zero;
      StartCoroutine(GrowPlant());
      return plant;
    }

    return null;
  }

  private bool FindVertex(Mesh mesh, out bool wall, out Vector3 position, out Vector3 normal)
  {
    int v = Random.Range(0, mesh.vertexCount);
    position = mesh.vertices[v];
    normal = mesh.normals[v];
    bool ground = normal.y > 1.0f - _groundNormalTolerance && _groundPrefabs.Count > 0;
    wall = Mathf.Abs(normal.y) < _wallNormalTolerance && _wallPrefabs.Count > 0;
    return wall || ground;
  }

  private IEnumerator GrowPlant()
  {
    yield return null;

    float progress = 0.0f;
    // end scale has Y inverted because of the transform on the mesh root
    Vector3 endScale = new Vector3(1.0f, -1.0f, 1.0f);
    while (progress < 1.0f && (bool)_plant)
    {
      progress = Mathf.Min(1.0f, progress + Time.deltaTime / _growthDuration);
      _plant.transform.localScale = Vector3.Lerp(Vector3.zero, endScale, progress);
      yield return null;
    }
  }
}
