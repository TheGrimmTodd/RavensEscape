using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    [Range(0,1)]
    public float postSpawnSleepTime = 0.7f;
    public Vector3 spawnLocation;
    [HideInInspector]
    private bool isSpawned = false;
    private bool isSpawning = false;
    private float spawnStartTime;

    public void SpawnPlayer()
    {
        if (!isSpawning)
        {
            transform.position = spawnLocation;
            isSpawning = true;
            spawnStartTime = Time.time;
        }
    }

    public void PlayerSeen()
    {
        //TODO anything special for when seen
        ResetLevel();
    }

    public void PlayerFallen()
    {
        //TODO anything special for when fallen
        ResetLevel();
    }

    public void ResetLevel()
    {
        //do anything that is needed to reset level.
        isSpawned = false;
    }

    public bool IsSpawned()
    {
        return isSpawned;
    }

    void Update()
    {
        if (spawnStartTime + postSpawnSleepTime > Time.time && isSpawning)
        {
            isSpawned = true;
            isSpawning = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        float size = .3f;
        
        Gizmos.DrawLine(spawnLocation - Vector3.up * size, spawnLocation + Vector3.up * size);
        Gizmos.DrawLine(spawnLocation - Vector3.right * size, spawnLocation + Vector3.right * size);
    }
}
