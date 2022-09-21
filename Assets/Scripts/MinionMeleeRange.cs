using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMeleeRange : MonoBehaviour {
    public event EventHandler OnMeleeRangeEntered;
    private Transform _minionMeleePoint;
    private CircleCollider2D _rangeCollider;

    private void Start() {
        BeginGetComponents();
        BeginAsserts();
    }

    // Update is called once per frame
    private void Update() {
    }

    //todo: fix initial collision vs. bow's range collider
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.isTrigger) Physics2D.IgnoreCollision(_rangeCollider, col);

        _minionMeleePoint.position = col.gameObject.transform.position;
        OnMeleeRangeEntered?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (col.isTrigger) Physics2D.IgnoreCollision(_rangeCollider, col);

        _minionMeleePoint.position = col.gameObject.transform.position;
        OnMeleeRangeEntered?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerExit2D(Collider2D col) {
        _minionMeleePoint.position = transform.position;
    }

    private void BeginAsserts() {
    }

    private void BeginGetComponents() {
        _minionMeleePoint = GetComponentInChildren<MinionMeleePoint>().transform;
        _rangeCollider = GetComponent<CircleCollider2D>();
    }
}