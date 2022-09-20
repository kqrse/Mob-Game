using System.Collections;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;

public class Minion : MonoBehaviour {
    public PlayerNumber playerNumber;
    private float _movementSpeed = 2f;
    private Rigidbody2D _rb;
    void Start() {
        BeginAsserts();
        BeginGetComponents();
    }

    void Update() {
        
    }

    public void Move(Vector2 direction) {
        _rb.velocity = direction * _movementSpeed;
    }

    private void BeginAsserts() {
        Assert.IsFalse(playerNumber == PlayerNumber.Unassigned);
    }

    private void BeginGetComponents() {
        _rb = GetComponent<Rigidbody2D>();
    }
}
