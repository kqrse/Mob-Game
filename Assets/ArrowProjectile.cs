using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    private bool _useStraightTrajectory;
    private float _lifespan = 0.4f;

    public void Init(BaseMinion minion, Vector3 targetPosition, float speed, float arcHeight) {
        _minion = minion;
        _rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
        _targetPos = targetPosition;
        _speed = speed;
        _arcHeight = arcHeight * Math.Abs(_startPos.x - _targetPos.x) / 2.5f;

        var straightShotRange = 0.8f;
        if (Math.Abs(_startPos.x - _targetPos.x) < straightShotRange) {
            _useStraightTrajectory = true;

            var arrowDirection = VectorHelper.GetVectorToPoint(_startPos, _targetPos);
            var arrowAngle = Vector2.SignedAngle(Vector2.left, arrowDirection);

            _rb.velocity = arrowDirection.normalized * (_speed * 1.5f);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0,arrowAngle));
            StartCoroutine(DestroyOnLifespan());
        }
    }

    private void FixedUpdate() {
        if (!_reachedDestination && !_useStraightTrajectory) UpdateArcArrow();
    }

    protected override bool IsInvalidAttackTarget(Collider2D targetCollider) {
        if (targetCollider.gameObject.layer != LayerMask.NameToLayer("Attackable")) return true;

        if (targetCollider.CompareTag("Minion"))
            return _minion.playerNumber == targetCollider.GetComponent<BaseMinion>().playerNumber;

        if (targetCollider.CompareTag("MinionSpawner"))
            return _minion.playerNumber == targetCollider.GetComponent<MinionSpawner>().playerNumber;

        return true;
    }

    private void UpdateArcArrow() {
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
            StartCoroutine(DelayedDestroy());
        }
    }

    private IEnumerator DelayedDestroy() {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private IEnumerator DestroyOnLifespan() {
        yield return new WaitForSeconds(_lifespan);
        Destroy(gameObject);
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

    private static Quaternion LookAt2D(Vector2 forward) {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg + 180);
    }
}