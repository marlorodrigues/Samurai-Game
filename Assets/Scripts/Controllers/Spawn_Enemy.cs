using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemy : MonoBehaviour
{
    public float minTime;
    public float maxTime;
    private float spawnTime;
    private float timeCount;

    public GameObject enemyPreFab;
    void Start()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount > spawnTime)
        {
            timeCount = 0f;

            Vector3 enemyPosition = new Vector3(transform.position.x, transform.position.y, 0f);

            Instantiate(enemyPreFab, enemyPosition, transform.rotation);

            spawnTime = Random.Range(minTime, maxTime);
        }
    }
}
