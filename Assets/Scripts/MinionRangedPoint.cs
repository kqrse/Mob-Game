using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionRangedPoint : MonoBehaviour {
    public event EventHandler OnRangedRangeEntered;
    private Transform _minionRangedPoint;

    private void Start() {
        BeginGetComponents();
        BeginAsserts();
    }

    // Update is called once per frame
    private void Update() {
    }

    private void OnTriggerEnter2D(Collider2D col) {
        _minionRangedPoint.position = col.gameObject.transform.position;
        OnRangedRangeEntered?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerStay2D(Collider2D col) {
        _minionRangedPoint.position = col.gameObject.transform.position;
        OnRangedRangeEntered?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerExit2D(Collider2D col) {
        _minionRangedPoint.position = transform.position;
    }

    private void BeginAsserts() {
    }

    private void BeginGetComponents() {
        _minionRangedPoint = GetComponentInChildren<MinionRangedPoint>().transform;
    }
}