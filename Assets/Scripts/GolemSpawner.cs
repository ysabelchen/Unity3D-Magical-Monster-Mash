using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject golemPrefab;

    private float maxWaitTime = 10f;
    private float currentWaitTime = 3f;

    private int spawnAmount = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAmount > 0) {
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime <= 0) {
                Instantiate(golemPrefab);
                spawnAmount -= 1;
                currentWaitTime = maxWaitTime;
            }
        }
    }
}
