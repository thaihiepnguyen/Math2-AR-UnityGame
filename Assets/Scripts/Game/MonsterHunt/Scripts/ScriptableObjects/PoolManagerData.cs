using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CreatePoolMode { Start, FirstGet, Default };

[System.Serializable]
public struct PoolData
{
    public string name;
    public Component prefab;
 
    public int defaultCapacity;
    public int maxSize;
    public CreatePoolMode createPoolMode;
    public bool createParent;
}


[CreateAssetMenu(fileName = "New Pool Data", menuName = "Planet Force/Pool Data")]
public class PoolManagerData : ScriptableObject
{
    public PoolData[] poolData;

    private void OnValidate()
    {
        for (int i = 0; i < poolData.Length; i++)
        {
            if (poolData[i].prefab)
            {
                poolData[i].name = poolData[i].prefab.name;
            }
        }
    }

}

