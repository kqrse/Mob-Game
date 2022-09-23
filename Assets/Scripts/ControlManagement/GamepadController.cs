using System;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlManagement {
    public class GamepadController : PlayerController {
        private Vector2 _currentMoveVector = Vector2.zero;
        private float _cursorMoveSpeed = 10f;

        public void OnMovement(InputAction.CallbackContext context) {
            _currentMoveVector = context.ReadValue<Vector2>();
            Debug.Log(_currentMoveVector);
        }

        public void OnWakeMinions(InputAction.CallbackContext context) {
            foreach (var m in minionsList)
                if (!m.animator.GetBool(AnimParams.MinionIsAwake))
                    m.IdleWakeUp();
        }

        private void Awake() {
        }

        private void Update() {
            UpdateMinions();
        }

        private void FixedUpdate() {
            MoveCursor();
        }

        private void MoveCursor() {
            var moveVelocity = new Vector2(_currentMoveVector.x * _cursorMoveSpeed,
                _currentMoveVector.y * _cursorMoveSpeed);
            var movementInFrame = Time.deltaTime * moveVelocity;
            transform.position += new Vector3(movementInFrame.x, movementInFrame.y, 0);
        }

        private void UpdateMinions() {
            var cursorPos = new Vector2(transform.position.x, transform.position.y);

            var liveMinions = new List<BaseMinion>();
            var hasMinionDied = false;
            foreach (var m in minionsList)
                if (m != null) {
                    var minionPos = new Vector2(m.transform.position.x, m.transform.position.y);
                    m.direction = (cursorPos - minionPos).normalized;
                }
                else {
                    liveMinions.Add(m);
                    hasMinionDied = true;
                }

            if (hasMinionDied) minionsList = liveMinions;
        }
    }
}