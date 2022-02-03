using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float initialSpawnTime;
    [SerializeField] Vector2 spawnDirection;
    [SerializeField] GameObject objectToSpawn;
    GameObject lastSpawnedObject;

    float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = initialSpawnTime;
    }

    private void Update()
    {
        if (GameManager.GameTime >= nextSpawnTime)
        {
            SpawnObject();
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime += GameManager.EnemySpawnRate;
    }

    void SpawnObject()
    {
        lastSpawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        StartCoroutine(co_SpawnObject());
    }

    IEnumerator co_SpawnObject()
    {
        yield return null;
        EventManager.RaiseOnSpawnObject(lastSpawnedObject.transform, spawnDirection.normalized);
    }
}
