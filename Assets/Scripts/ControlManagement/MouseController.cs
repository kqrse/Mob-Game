using System.Collections;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MouseController : PlayerController {
    private Camera _cam;

    private void Start() {
        _cam = Camera.main;
    }

    private void Update() {
        SetMinionDirections();
        if (Input.GetKeyDown(KeyCode.Space))
            foreach (var m in minionsList)
                if (!m.animator.GetBool(AnimParams.MinionIsAwake))
                    m.IdleWakeUp();
    }

    private void SetMinionDirections() {
        var rawMousePos = Input.mousePosition;
        Vector2 mousePos = _cam.ScreenToWorldPoint(new Vector2(rawMousePos.x,
            rawMousePos.y));

        foreach (var m in minionsList) {
            var minionPos = new Vector2(m.transform.position.x, m.transform.position.y);
            var direction = mousePos - minionPos;
            if (Mathf.Abs(direction.x) < MovementTolerance) direction.x = 0;
            if (Mathf.Abs(direction.y) < MovementTolerance) direction.y = 0;
            m.animator.SetBool(AnimParams.MinionIsMoving, m.direction.normalized != Vector2.zero);
            m.direction = direction.normalized;
        }
    }
}