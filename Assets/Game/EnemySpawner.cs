using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    private int enemiesPerWave = 1;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(enemyPrefab, point.position, point.rotation);
            }

            enemiesPerWave++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
