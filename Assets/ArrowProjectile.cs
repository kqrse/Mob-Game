using System;
using System.Collections;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;

public class ArrowProjectile : CollisionCheckValidTarget {
    [SerializeField] private GameObject arrowHitAnimation;

    private BaseMinion _minion;
    private Rigidbody2D _rb;

    private Vector3 _targetPos;
    private float _speed = 6;
    private float _arcHeight = 1;
    private Vector3 _startPos;
    private bool _reachedDestination;

    public void Init(BaseMinion minion, Vector3 targetPosition, float speed, float arcHeight) {
        _minion = minion;
        _rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
        _targetPos = targetPosition;
        _speed = speed;
        _arcHeight = arcHeight;
    }

    private void Update() {
        if (!_reachedDestination) UpdateArrow();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("MapBoundary")) {
            Destroy(gameObject);
            return;
        }

        if (IsInvalidAttackTarget(col)) return;

        var enemyMinion = col.GetComponent<BaseMinion>();
        if (enemyMinion == null) return;

        if (enemyMinion.playerNumber == _minion.playerNumber) return;

        var enemyHealth = col.gameObject.GetComponent<Health>();
        Assert.IsNotNull(enemyHealth);
        enemyHealth.Damage(_minion.GetDamageValue());

        Instantiate(arrowHitAnimation, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override bool IsInvalidAttackTarget(Collider2D targetCollider) {
        if (targetCollider.gameObject.layer != LayerMask.NameToLayer("Attackable")) return true;

        if (targetCollider.CompareTag("Minion"))
            return _minion.playerNumber == targetCollider.GetComponent<BaseMinion>().playerNumber;

        if (targetCollider.CompareTag("MinionSpawner"))
            return _minion.playerNumber == targetCollider.GetComponent<MinionSpawner>().playerNumber;

        return true;
    }


    private void Start() {
        _startPos = transform.position;
    }

    private void UpdateArrow() {
        // Compute the next position, with arc added in
        var x0 = _startPos.x;
        var x1 = _targetPos.x;
        var dist = x1 - x0;
        var nextX = Mathf.MoveTowards(transform.position.x, x1, _speed * Time.deltaTime);
        var baseY = Mathf.Lerp(_startPos.y, _targetPos.y, (nextX - x0) / dist);
        var arc = _arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
        var nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

        // Rotate to face the next position, and then move there
        transform.rotation = LookAt2D(nextPos - transform.position);
        transform.position = nextPos;

        if (nextPos == _targetPos) {
            _reachedDestination = true;
            _rb.velocity = VectorHelper.RotationToVector(transform.rotation.z) * _speed;
        }
    }

    private static Quaternion LookAt2D(Vector2 forward) {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
}