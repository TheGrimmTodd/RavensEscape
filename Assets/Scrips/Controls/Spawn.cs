using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelText))]
public class Spawn : MonoBehaviour {

    [Range(0,1)]
    public float postSpawnSleepTime = 0.7f;
    public Vector3 spawnLocation;
    public LevelText levelText;
    private bool isSpawned = false;
    private bool isSpawning = false;
    private float spawnStartTime;

    public void PlayerSeen()
    {
        //TODO anything special for when seen
        levelText.SetLevelText("You've been Seen. Lets Reset", LevelText.TextType.Long);
        ResetLevel();
    }

    public void PlayerFallen()
    {
        //TODO anything special for when fallen
        levelText.SetLevelText("That was not the right way togo, lets reset.", LevelText.TextType.Long);
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

    internal void SpawnPlayer(Player player)
    {
        if (!isSpawning)
        {
            transform.position = spawnLocation;
            player.velocity = Vector3.zero;
            isSpawning = true;
            spawnStartTime = Time.time;
        }
    }

    internal void FinishedLevel()
    {
        //TODO anything special for when Level is Finished.
        //move to UI screen &| next level
        //for now, just reset again.
        levelText.SetLevelText("You've escaped to the next Area!", LevelText.TextType.Long);
        ResetLevel();
    }
}
