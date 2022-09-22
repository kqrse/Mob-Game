using System;
using System.Collections;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class MinionSpawner : MonoBehaviour {
    [SerializeField] private GameObject baseMinionPrefab;
    [SerializeField] private GameObject bowMinionPrefab;
    [SerializeField] private bool isSpawning;
    [SerializeField] private PlayerController playerController;
    public PlayerNumber playerNumber;

    private float _spawnCooldown = 0.5f;
    private int _rangedSpawnDelayCurrent = 3;
    private int _rangedSpawnDelayMax = 3;
    private int _maxMinions = 10;


    // private int _magicSpawnDelayCurrent = 3;
    // private int _magicSpawnDelayMax = 3;
    // private int _tankSpawnDelayCurrent = 3;
    // private int _tankSpawnDelayMax = 3;
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
        if (!isSpawning || playerController.minionsList.Count >= _maxMinions) return;

        var offset = 0.5f;
        var offsetX = Random.Range(-offset, offset);
        var offsetY = Random.Range(-offset, offset);

        var spawnCoordinate = _spawnBounds.center + new Vector3(offsetX, offsetY, 0);
        BaseMinion minion = null;
        if (_rangedSpawnDelayCurrent == 0) {
            minion = Instantiate(bowMinionPrefab, spawnCoordinate, Quaternion.identity)
                .GetComponent<BaseMinion>();
            _rangedSpawnDelayCurrent = _rangedSpawnDelayMax;
        }
        else {
            minion = Instantiate(baseMinionPrefab, spawnCoordinate, Quaternion.identity)
                .GetComponent<BaseMinion>();
            _rangedSpawnDelayCurrent--;
        }

        Assert.IsNotNull(minion);
        minion.playerNumber = playerNumber;
        playerController.minionsList.Add(minion);
    }

    private void BeginAsserts() {
        Assert.IsFalse(playerNumber == PlayerNumber.Unassigned);
        Assert.IsNotNull(baseMinionPrefab);
        Assert.IsNotNull(bowMinionPrefab);
        Assert.IsNotNull(playerController);
    }

    private void BeginGetComponents() {
        _spawnBounds = GetComponentInChildren<BoxCollider2D>().bounds;
    }
}