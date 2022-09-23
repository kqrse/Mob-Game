using System;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlManagement {
    public class GamepadController : PlayerController {
        private Vector2 _currentMoveVector = Vector2.zero;
        private float _cursorMoveSpeed = 10f;
        private Rigidbody2D _rb;

        public void OnMovement(InputAction.CallbackContext context) {
            _currentMoveVector = context.ReadValue<Vector2>();
        }

        public void OnWakeMinions(InputAction.CallbackContext context) {
            foreach (var m in minionsList)
                if (!m.animator.GetBool(AnimParams.MinionIsAwake))
                    m.IdleWakeUp();
        }

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            SetMinionDirections();
        }

        private void FixedUpdate() {
            MoveCursor();
        }

        private void MoveCursor() {
            // var moveVelocity = new Vector2(_currentMoveVector.x * _cursorMoveSpeed,
            //     _currentMoveVector.y * _cursorMoveSpeed);
            // var movementInFrame = Time.deltaTime * moveVelocity;
            // transform.position += new Vector3(movementInFrame.x, movementInFrame.y, 0);
            if (_currentMoveVector != Vector2.zero) _rb.velocity = _currentMoveVector * _cursorMoveSpeed;
            else _rb.velocity = Vector2.zero;
        }

        private void SetMinionDirections() {
            var cursorPos = new Vector2(transform.position.x, transform.position.y);

            foreach (var m in minionsList) {
                var minionPos = new Vector2(m.transform.position.x, m.transform.position.y);
                var direction = cursorPos - minionPos;
                if (Mathf.Abs(direction.x) < MovementTolerance) direction.x = 0;
                if (Mathf.Abs(direction.y) < MovementTolerance) direction.y = 0;
                m.animator.SetBool(AnimParams.MinionIsMoving, m.direction.normalized != Vector2.zero);
                m.direction = direction.normalized;
            }
        }
    }
}