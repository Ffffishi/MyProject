using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerMgr : MonoBehaviour
{
    public static EnemySpawnerMgr Instance;

    void Awake()
    {
        Instance = this;
    }
    [Header("敌人刷新点")]
    public Transform[] spawnPoint;

    public GameObject bossPrefab;
    private bool isBossSpawned = false;
    public GameObject[] enemyPrefabs;//敌人预制体
    public float timeToSpawn;//间隔时间
    private float spawnCounter=0;//计时器
    public int enemyCount;//敌人数量
    //[Header("敌人刷新时间点")]
    //public float countA, countB, countC, countD;
    //private Animator animator;

    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (LevelMgr.instance.timer < 70f)
        {
            enemyCount = 5;
        }else 
        {
            enemyCount = 8;
        }
        if (spawnCounter > 0)
        {
            spawnCounter -= Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < enemyCount; i++)
            {

                SpawnEnemy();
            }
            spawnCounter = timeToSpawn;
        }
    }

    public void SpawnEnemy()
    {
        if (LevelMgr.instance.timer < 20f)
        {
            GameObject enemy = Instantiate(enemyPrefabs[0], GetSpawnPoint().position, Quaternion.identity);
            enemy.transform.parent = transform;
        }
        else if (LevelMgr.instance.timer >= 20f && LevelMgr.instance.timer < 40f)
        {
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, 2)], GetSpawnPoint().position, Quaternion.identity);
            enemy.GetComponent<Character>().maxHealth *= 2;
            enemy.transform.parent = transform;
        }
        else if (LevelMgr.instance.timer >= 40f && LevelMgr.instance.timer < 60f)
        {
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, 3)], GetSpawnPoint().position, Quaternion.identity);
            if (enemy.GetComponent<Character>()!=null)
            {
                enemy.GetComponent<Character>().maxHealth *= 3;
                //enemy.GetComponent<EnergyPlus>().speed *= 1.5f;
                if (enemy.GetComponent<Enemy>() != null)
                {
                    enemy.GetComponent<Enemy>().expToGive=5;
                }
            }
            else
            {
                enemy.GetComponent<EPlus>().maxHealth *= 3;
                enemy.GetComponent<EPlus>().expToGive = 5;
            }
            enemy.transform.parent = transform;
        }else
        {
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, 3)], GetSpawnPoint().position, Quaternion.identity);
            int index= (int)(LevelMgr.instance.timer) / 20;
            if (enemy.GetComponent<Character>() != null)
            {
                enemy.GetComponent<Character>().maxHealth *= 3;
                if (enemy.GetComponent<Enemy>() != null)
                {
                    enemy.GetComponent<Enemy>().expToGive = 5;
                }
            }
            else
            {
                enemy.GetComponent<EPlus>().maxHealth *= 3;
                enemy.GetComponent<EPlus>().expToGive = 5;

            }
            enemy.transform.parent = transform;
        }

        if (LevelMgr.instance.timer >= 180f && isBossSpawned == false)
        {
            GameObject enemy = Instantiate(bossPrefab, GetSpawnPoint().position, Quaternion.identity);
            enemy.GetComponent<BossA>().maxHealth *= 4;
            enemy.transform.parent = transform;
            isBossSpawned = true;
        }
        
    }

    //获取随机刷新点
    public Transform GetSpawnPoint()
    {
        int index = Random.Range(0, spawnPoint.Length);
        return spawnPoint[index];
    }
}
//[System.Serializable]
//public class EnemyData
//{
//    public GameObject enemyPrefab;//敌人预制体
//    public float timeToSpawn;//刷新时间
//    public float enemyCounte;//刷新数量
//}