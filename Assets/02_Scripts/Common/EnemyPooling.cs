using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    public static EnemyPooling instance;
    private int enemyMaxCount = 10;
    private Transform tr;
    public List<GameObject> enemyPool;
    private GameObject enemy;
    public List<Transform> spawnPointList = new();

    void Awake()
    {
        instance = this;
        tr = transform;
        enemyPool = new List<GameObject>();
        enemy = Resources.Load<GameObject>("Enemy");
        SetEnemyPool();
        FindSpawnPoint();
    }

    void FindSpawnPoint()
    {
        var spawnpoints = GameObject.Find("Points");
        if (spawnpoints != null)
            spawnpoints.GetComponentsInChildren(spawnPointList);
        spawnPointList.RemoveAt(0);
        if (spawnPointList.Count > 0)
            StartCoroutine(CreateEnemy());
    }
    
    IEnumerator CreateEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            foreach (GameObject enemy in enemyPool)     
            {
                if (!enemy.activeSelf)
                {
                    int index = Random.Range(0, spawnPointList.Count);
                    enemy.transform.position = spawnPointList[index].position;
                    enemy.transform.rotation = spawnPointList[index].rotation;
                    enemy.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private void SetEnemyPool()
    {
        for (int i = 0; i <= enemyMaxCount; i++)
        {
            var enemies = Instantiate(enemy, tr);
            enemies.name = $"enemy_{i}";
            enemies.SetActive(false);
            enemyPool.Add(enemies);
        }
    }

    public GameObject GetEnemyPool()
    {
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].activeSelf)
                return enemyPool[i];
        }
        return null;
    }



}