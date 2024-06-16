using UnityEngine;
using System;


[CreateAssetMenu(fileName = "New Spawner Manager Data", menuName = "Monsters n Guns/Spawner Manager Data")]
public class SpawnerManagerData : ScriptableObject
{    
    public float spawnTimeBetweenMonsters = 0.2f;
    public MonstersByLevel[] monstersByLevels; 
}


[Serializable]
public struct MonsterToSpawn
{
    public MonsterController monsterPrefab;
    public int count;
}


[Serializable]
public struct MonstersByLevel
{
    public MonsterToSpawn[] initialMonsters;

}


