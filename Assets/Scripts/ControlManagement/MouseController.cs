using System.Collections;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MouseController : PlayerController {
    private Camera _cam;
    private Vector2 _currentMoveVector = Vector2.zero;
    private float _cursorMoveSpeed = 30f;
    private Rigidbody2D _rb;

    private void Start() {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        MoveMouseCursor();
        SetMinionDirections();
        if (Input.GetKeyDown(KeyCode.Space))
            foreach (var m in minionsList)
                if (!m.animator.GetBool(AnimParams.MinionIsAwake))
                    m.IdleWakeUp();
    }

    private void MoveMouseCursor() {
        var rawMousePos = Input.mousePosition;
        Vector2 mousePos = _cam.ScreenToWorldPoint(new Vector2(rawMousePos.x,
            rawMousePos.y));

        Vector2 currPos = transform.position;
        _currentMoveVector = (mousePos - currPos).normalized;

        var dist = Vector2.Distance(mousePos, currPos);

        switch (dist) {
            case < 0.1f:
                _rb.velocity = Vector2.zero;
                break;
            case < 2f: {
                var closeness = dist / 2f;
                _rb.velocity = _currentMoveVector * (_cursorMoveSpeed * closeness);
                break;
            }
            default:
                _rb.velocity = _currentMoveVector * _cursorMoveSpeed;
                break;
        }
    }

    private void SetMinionDirections() {
        foreach (var m in minionsList) {
            var direction = (Vector2)(transform.position - m.transform.position);
            if (Mathf.Abs(direction.x) < MovementTolerance) direction.x = 0;
            if (Mathf.Abs(direction.y) < MovementTolerance) direction.y = 0;
            m.animator.SetBool(AnimParams.MinionIsMoving, m.direction.normalized != Vector2.zero);
            m.direction = direction.normalized;
        }
    }
}