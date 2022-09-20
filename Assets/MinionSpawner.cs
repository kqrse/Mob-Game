using System;
using System.Collections;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class MinionSpawner : MonoBehaviour {
    private float _spawnCooldown = 1f;
    private Bounds _spawnBounds;

    [SerializeField] private GameObject minion;

    public static event EventHandler OnMinionSpawn;
    public PlayerNumber playerNumber;
    
    void Start() {
        BeginAsserts();
        BeginGetComponents();
        StartCoroutine(StartSpawnMinion());
    }

    void Update() { }

    private IEnumerator StartSpawnMinion() {
        yield return new WaitForSeconds(_spawnCooldown);
        StartCoroutine(StartSpawnMinion());
        SpawnMinion();
        OnMinionSpawn?.Invoke(this, EventArgs.Empty);
    }

    private void SpawnMinion() {
        Vector2 spawnCoordinate = new Vector2(
            Random.Range(_spawnBounds.min.x, _spawnBounds.max.x),
            Random.Range(_spawnBounds.min.y, _spawnBounds.max.y));
        Instantiate(minion, spawnCoordinate, Quaternion.identity);
    }

    private void BeginAsserts() {
        Assert.IsFalse(playerNumber == PlayerNumber.Unassigned);
        Assert.IsNotNull(minion);
    }

    private void BeginGetComponents() {
        _spawnBounds = GetComponentInChildren<BoxCollider2D>().bounds;
    }
}