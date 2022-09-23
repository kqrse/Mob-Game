using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MinionMeleeRange : CollisionCheckValidTarget {
    public event EventHandler OnMeleeRangeEntered;
    private Transform _attackPoint;
    private CircleCollider2D _rangeCollider;

    private void Start() {
        BeginGetComponents();
        BeginAsserts();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (IsInvalidAttackTarget(col)) return;

        _attackPoint.position = col.gameObject.transform.position;
        OnMeleeRangeEntered?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (IsInvalidAttackTarget(col)) return;

        _attackPoint.position = col.gameObject.transform.position;
        OnMeleeRangeEntered?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerExit2D(Collider2D col) {
        _attackPoint.position = transform.position;
    }

    private void BeginAsserts() {
        Assert.IsNotNull(_attackPoint);
        Assert.IsNotNull(_rangeCollider);
    }

    private void BeginGetComponents() {
        _attackPoint = GetComponentInChildren<MinionAttackPoint>().transform;
        _rangeCollider = GetComponent<CircleCollider2D>();
    }
}