using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSapwn;
    public float timeToSpawn;
    public Transform maxSpawnPoint;
    public Transform minSpawnPoint;
    public int checkPerFrame;
    public List<WaveInfo> waves;

    private float waveCounter;
    private int currentWave;
    private int enemyToCheck;
    private Transform target;
    private float spawnCounter;
    private float despawnDistance;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        //spawnCounter = timeToSpawn;
        target=PlayerHealthController.instance.transform;
        despawnDistance=Vector3.Distance(transform.position, maxSpawnPoint.position)+4f;

        currentWave = -1;
        GoToNextWave();
    }
    void Update()
    {
        //spawnCounter -= Time.deltaTime;
        //if (spawnCounter <= 0)
        //{
        //    spawnCounter = timeToSpawn;
        //    GameObject enemy = Instantiate(enemyToSapwn, GetRandomSpawnPosition(), transform.rotation);
        //    spawnedEnemies.Add(enemy);
        //} 
        if (PlayerHealthController.instance.gameObject.activeSelf)
        {
            if (currentWave < waves.Count)
            {
                waveCounter-=Time.deltaTime;
                if(waveCounter<=0)
                {
                    GoToNextWave();
                }
                spawnCounter -= Time.deltaTime;
                if (spawnCounter <= 0)
                {
                    spawnCounter = timeToSpawn;
                    GameObject enemy = Instantiate(waves[currentWave].enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
                    spawnedEnemies.Add(enemy);
                }
            }
            else
            {
                currentWave = 0;
                waveCounter = 0;
            }
        }



        transform.position = target.position;

        int checkTarget = checkPerFrame+enemyToCheck;
        while (enemyToCheck < checkTarget)
        {
            if (enemyToCheck < spawnedEnemies.Count)
            {
                if(spawnedEnemies[enemyToCheck]!= null)
                {
                    if (Vector3.Distance(spawnedEnemies[enemyToCheck].transform.position, transform.position) > despawnDistance)
                    {
                        Destroy(spawnedEnemies[enemyToCheck]);
                        spawnedEnemies.RemoveAt(enemyToCheck);
                        checkTarget--;
                    }
                    else
                    {
                        enemyToCheck++;
                    }
                }
                else
                {
                    spawnedEnemies.RemoveAt(enemyToCheck);
                    checkTarget--;
                }
            }
            else
            {
                enemyToCheck = 0;
                checkTarget = 0;
            }
        }
    }

    public Vector3 GetRandomSpawnPosition()
    {
        Vector3 spwanPosition = Vector3.zero;
        bool isSpawnPositionValid = Random.Range(0f, 1f) >.5f;
        if (isSpawnPositionValid)
        {
            spwanPosition.y= Random.Range(minSpawnPoint.position.y, maxSpawnPoint.position.y);
            if (Random.Range(0f, 1f) > .5f)
            {
                spwanPosition.x=maxSpawnPoint.position.x;
            }
            else
            {
                spwanPosition.x=minSpawnPoint.position.x;
            }
        }
        else
        {
            spwanPosition.x = Random.Range(minSpawnPoint.position.x, maxSpawnPoint.position.x);
            if (Random.Range(0f, 1f) > .5f)
            {
                spwanPosition.y = maxSpawnPoint.position.y;
            }
            else
            {
                spwanPosition.y = minSpawnPoint.position.y;
            }
        }

        return spwanPosition;
    }
    public void GoToNextWave()
    {
        currentWave++;
        if (currentWave >= waves.Count)
        {
            currentWave = waves.Count - 1;
        }
        waveCounter = waves[currentWave].waveLength;
        spawnCounter = waves[currentWave].timeBetweenSpawn;
    }

}
[System.Serializable]
public class WaveInfo
{
    public GameObject enemyPrefab;
    public float timeBetweenSpawn=1f;
    public float waveLength=10f;
}