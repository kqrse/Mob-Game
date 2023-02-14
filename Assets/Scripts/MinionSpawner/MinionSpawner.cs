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

    private float _spawnCooldown = 1.25f;
    private float _initialSpawnCooldown = 0.3f;
    private int _initialMinionsSpawned = 0;
    private int _initialMinionsMax = 7;
    private int _rangedSpawnDelayCurrent = 3;
    private int _rangedSpawnDelayMax = 3;
    private int _maxMinions = 15;

    private Health _baseHealth;
    private float _maxHealth = 50;
    [SerializeField] private GameObject healthBarPrefab;
    private HealthBar _healthBar;

    private SpriteRenderer _sr;
    private Rigidbody2D _rb;


    // private int _magicSpawnDelayCurrent = 3;
    // private int _magicSpawnDelayMax = 3;
    // private int _tankSpawnDelayCurrent = 3;
    // private int _tankSpawnDelayMax = 3;
    private Bounds _spawnBounds;
    public static event EventHandler OnMinionSpawn;

    private void Start() {
        BeginGetComponents();
        BeginAsserts();
        StartCoroutine(StartSpawnInitialMinion());
    }

    private IEnumerator StartSpawnMinion() {
        if (_healthBar == null) yield break;
        yield return new WaitForSeconds(_spawnCooldown);
        StartCoroutine(StartSpawnMinion());
        SpawnMinion();
        OnMinionSpawn?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator StartSpawnInitialMinion() {
        yield return new WaitForSeconds(_initialSpawnCooldown);
        if (_initialMinionsSpawned == _initialMinionsMax) StartCoroutine(StartSpawnMinion());
        else StartCoroutine(StartSpawnInitialMinion());
        _initialMinionsSpawned++;
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

    private void StartDeath(object e, EventArgs eventArgs) {
        _baseHealth.OnHealthDepleted -= StartDeath;
        StartCoroutine(DeathAnimation());
        StartCoroutine(DelayedHealthbarDeletion());
    }

    private IEnumerator DelayedHealthbarDeletion() {
        yield return new WaitForSeconds(0.5f);
        Destroy(_healthBar.gameObject);
    }

    private IEnumerator DeathAnimation() {
        _rb.simulated = false;
        var newRotation = Quaternion.Euler(0, 0, 90);
        _sr.sortingLayerName = "Background";

        while (_sr.color.a > 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 7.5f);
            var color = _sr.color;
            var newAlpha = color.a -= 0.004f;
            _sr.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void BeginAsserts() {
        Assert.IsFalse(playerNumber == PlayerNumber.Unassigned);
        Assert.IsNotNull(baseMinionPrefab);
        Assert.IsNotNull(bowMinionPrefab);
        Assert.IsNotNull(playerController);
        Assert.IsNotNull(_rb);
        Assert.IsNotNull(_sr);
    }

    private void BeginGetComponents() {
        _spawnBounds = GetComponentInChildren<BoxCollider2D>().bounds;
        _baseHealth = GetComponent<Health>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        SetPlayerColor();
        _baseHealth.Init(_maxHealth);
        _healthBar = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity).GetComponent<HealthBar>();
        _healthBar.Init(transform, _baseHealth, 36f);
        _baseHealth.OnHealthDepleted += StartDeath;
    }

    private void SetPlayerColor() {
        switch (playerNumber) {
            case PlayerNumber.One:
                _sr.color = PlayerColor.PlayerOneActive;
                break;
            case PlayerNumber.Two:
                _sr.color = PlayerColor.PlayerTwoActive;
                break;
            case PlayerNumber.Three:
                _sr.color = PlayerColor.PlayerThreeActive;
                break;
            case PlayerNumber.Four:
                _sr.color = PlayerColor.PlayerFourActive;
                break;
        }
    }
}