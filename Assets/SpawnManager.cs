using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] _jewels;
    [SerializeField] Transform[] _spawnPoints;

    int numberOfJewels = 5;

    private void Start()
    {
        for(int i = 0; i < numberOfJewels; i++)
        {
            int randomJewelIndex = Random.Range(0, _jewels.Length);
            int randomSpawnIndex = Random.Range(0, _spawnPoints.Length);
            var position = _spawnPoints[randomSpawnIndex].position;

            Instantiate(_jewels[randomJewelIndex], position, Quaternion.identity);
        }
    }
}
