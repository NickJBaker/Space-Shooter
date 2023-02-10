using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    private UIManager _bestScore;

    private bool _stopSpawning = false;

    void Start()
    {
        _bestScore = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, SpawnPos(), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            int randomPowerUp = Random.Range(0, 3);
            yield return new WaitForSeconds(Random.Range(7.0f, 15.0f));
            Instantiate(powerups[randomPowerUp], SpawnPos(), Quaternion.identity);
        }
    }

    private Vector3 SpawnPos()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-9.0f, 9.0f), 7, 0);
        return spawnPos;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
