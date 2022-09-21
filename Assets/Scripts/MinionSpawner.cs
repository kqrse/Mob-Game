using System;
using System.Collections;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class MinionSpawner : MonoBehaviour {
    [SerializeField] private GameObject minionPrefab;
    public PlayerNumber playerNumber;
    private readonly float _spawnCooldown = 1f;
    private Bounds _spawnBounds;

    private void Start() {
        BeginGetComponents();
        BeginAsserts();
        StartCoroutine(StartSpawnMinion());
    }

    private void Update() {
    }

    public static event EventHandler OnMinionSpawn;

    private IEnumerator StartSpawnMinion() {
        yield return new WaitForSeconds(_spawnCooldown);
        StartCoroutine(StartSpawnMinion());
        SpawnMinion();
        OnMinionSpawn?.Invoke(this, EventArgs.Empty);
    }

    private void SpawnMinion() {
        var offset = 0.5f;
        var offsetX = Random.Range(-offset, offset);
        var offsetY = Random.Range(-offset, offset);

        var spawnCoordinate = _spawnBounds.center + new Vector3(offsetX, offsetY, 0);
        // Random.Range(_spawnBounds.min.x, _spawnBounds.max.x),
        // Random.Range(_spawnBounds.min.y, _spawnBounds.max.y));

        // Debug.Log(_spawnBounds.center);
        // Debug.Log(_spawnBounds.extents);
        // Debug.Log(spawnCoordinate);

        var minion = Instantiate(minionPrefab, spawnCoordinate, Quaternion.identity)
            .GetComponent<BaseMinion>();
        minion.playerNumber = playerNumber;
    }

    private void BeginAsserts() {
        Assert.IsFalse(playerNumber == PlayerNumber.Unassigned);
        Assert.IsNotNull(minionPrefab);
    }

    private void BeginGetComponents() {
        _spawnBounds = GetComponentInChildren<BoxCollider2D>().bounds;
    }
}