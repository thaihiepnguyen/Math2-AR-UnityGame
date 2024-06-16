using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    
    private static PoolManager instance = null;
    public static PoolManager Instance => instance;

    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 100;
    [SerializeField] private bool defaultCreateObjects = false;
    [SerializeField] private bool collectionCheck = true; 
    [SerializeField] private bool forceDestroy = true; 
    [SerializeField] private Transform defaultParent;
    [SerializeField] private int creationOnStartWaitFrameAfter = 10; 
    [SerializeField] private PoolManagerData data;

          
    Dictionary<int, ObjectPool<Component>> pools = new();
    Dictionary<int, ObjectPool<Component>> objectPoolLookup = new();

   
    Dictionary<int, Component> componentLookup = new();

    
    Dictionary<int, PoolData> poolDataLookup = new();

   
    Dictionary<int, Transform> parentLookup = new();

   
    Component prefabTemp;
    Transform parentTemp;
    void SetTempVars(Component prefab, Transform parent)
    {
        prefabTemp = prefab;
        parentTemp = parent;
    }

    private void Awake()
    {
        if (!SingletonAwakeValidation())
            return;
        FillPoolDataLookup();
    }

    private void Start()
    {
        StartCoroutine(CreatePoolsModeStartRoutine());
    }

    public T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        var parent = GetParentOrCreate(GetPrefabID(prefab));
        var obj = GetFromPool(prefab, parent);
        obj.transform.SetPositionAndRotation(position, rotation);
        return (T)obj;
    }

    public T Get<T>(T prefab, Transform parent) where T : Component
    {
        var obj = GetFromPool(prefab, parent);
        return (T)obj;
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Transform prefabTransform = prefab.transform;
        var comp = Get(prefabTransform, position, rotation);
        return comp.gameObject;
    }

    public bool Release(GameObject obj)
    {
        if (!obj)
            return false; 

      
        if (collectionCheck && !obj.activeInHierarchy)
            return false; 

        var gameObjectID = obj.GetInstanceID();
        if (objectPoolLookup.TryGetValue(gameObjectID, out var pool))
        {
            var component = componentLookup[gameObjectID];
            pool.Release(component);
            return true;
        }
        else
        {
            if (forceDestroy)
                Destroy(obj);
            return false; 
        }
    }

    bool SingletonAwakeValidation()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return false;
        }
        return true;
    }

    void FillPoolDataLookup()
    {
        if (!data) return;
        for (int i = 0; i < data.poolData.Length; i++)
        {
            var poolDataItem = data.poolData[i];
            poolDataLookup[GetPrefabID(poolDataItem.prefab)] = poolDataItem;
        }
    }

    int GetPrefabID(Component prefab) => prefab.gameObject.GetInstanceID();

    IEnumerator CreatePoolsModeStartRoutine()
    {
        if (!data) yield break;
        
        for (int i = 0; i < data.poolData.Length; i++)
        {
            var poolDataItem = data.poolData[i];
            if (poolDataItem.createPoolMode == CreatePoolMode.Start)
            {
                Transform parent = GetParentOrCreate(GetPrefabID(poolDataItem.prefab));
                yield return StartCoroutine(CreatePoolRoutine(poolDataItem.prefab, poolDataItem.defaultCapacity, 
                    poolDataItem.maxSize, true, parent));
            }
        }
    }

    Transform GetParentOrCreate(int prefabID)
    {
        if (parentLookup.TryGetValue(prefabID, out var parent))
            return parent;
        else if (!poolDataLookup.TryGetValue(prefabID, out var poolDataItem))
            return defaultParent;
        else if (poolDataItem.createParent)
        {
            GameObject parentObject = new GameObject("Object Pool - " + poolDataItem.prefab.name);
            parent = parentObject.transform;
            parentLookup[prefabID] = parent;
            return parent;
        }
        else
            return defaultParent;
    }

  
    Component GetFromPool(Component prefab, Transform parent)
    {
        var pool = GetPoolOrCreate(prefab);
        SetTempVars(prefab, parent);
        var obj = pool.Get(); 
        obj.gameObject.SetActive(true);
        obj.transform.parent = parent; 
        return obj;
    }
    
    ObjectPool<Component> GetPoolOrCreate(Component prefab)
    {
        int prefabID = GetPrefabID(prefab);

        if (!pools.TryGetValue(prefabID, out var pool))
        {
            var parent = GetParentOrCreate(prefabID);

            if (poolDataLookup.TryGetValue(prefabID, out var poolDataItem)) 
            {
                bool createObjects = poolDataItem.createPoolMode == CreatePoolMode.FirstGet
                   
                    || poolDataItem.createPoolMode == CreatePoolMode.Start 
                    || defaultCreateObjects;
                pool = CreatePool(prefab, poolDataItem.defaultCapacity, poolDataItem.maxSize, createObjects, parent);
            }
            else
                pool = CreatePool(prefab, defaultCapacity, maxSize, defaultCreateObjects, parent);
        }

        return pool;
    }

    ObjectPool<Component> CreatePool(Component prefab, int defaultCapacity, int maxSize, bool createObjects, Transform parent)
    {
        int prefabID = GetPrefabID(prefab);
        if (pools.TryGetValue(prefabID, out var pool))
            return pool;
        
        pool = new ObjectPool<Component>(CreateFunc, null, OnReturnedToPool, OnDestroyPoolObject, collectionCheck, 
            defaultCapacity, maxSize); 
        pools[prefabID] = pool;

        prefab.gameObject.SetActive(false); 

        if (createObjects)
            CreateObjectsInPool(prefab, parent, pool, defaultCapacity);

        return pool;
    }

    IEnumerator CreatePoolRoutine(Component prefab, int defaultCapacity, int maxSize, bool createObjects, Transform parent)
    {
        var pool = CreatePool(prefab, defaultCapacity, maxSize, false, parent); // Solo crear el pool, sin ning�n clon

        if (createObjects)
            yield return StartCoroutine(CreateObjectsInPoolRoutine(prefab, parent, pool, defaultCapacity));
    }

    void CreateObjectsInPool(Component prefab, Transform parent, ObjectPool<Component> pool, int defaultCapacity)
    {
        SetTempVars(prefab, parent);
        var objectsCreated = new Component[defaultCapacity];

        for (int i = 0; i < objectsCreated.Length; i++) 
            objectsCreated[i] = pool.Get();

        for (int i = 0; i < objectsCreated.Length; i++)
            pool.Release(objectsCreated[i]);
    }

    IEnumerator CreateObjectsInPoolRoutine(Component prefab, Transform parent, ObjectPool<Component> pool, int defaultCapacity)
    {        
        var objectsCreated = new List<Component>(defaultCapacity);

        for (int i = 0; pool.CountAll < defaultCapacity; i++)
        {            
            SetTempVars(prefab, parent); 
            objectsCreated.Add(pool.Get()); 

            if (i % creationOnStartWaitFrameAfter == 0)
                yield return null;
        }

        for (int i = 0; i < objectsCreated.Count; i++)
            pool.Release(objectsCreated[i]);
    }
   
    Component CreateFunc()
    {
        var component = Instantiate(prefabTemp, parentTemp);
        int gameObjectID = component.gameObject.GetInstanceID();
        int prefabID = GetPrefabID(prefabTemp);

        objectPoolLookup[gameObjectID] = pools[prefabID]; 
        componentLookup[gameObjectID] = component; 

        return component;
    }

    void OnReturnedToPool(Component obj) => obj.gameObject.SetActive(false);

    void OnDestroyPoolObject(Component obj) => Destroy(obj.gameObject);
}