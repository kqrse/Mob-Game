using System;
using Globals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlMangement {
    public class GamepadController : PlayerController {
        private Vector2 _currentMoveVector = Vector2.zero;
        private float _cursorMoveSpeed = 10f;

        public void OnMovement(InputAction.CallbackContext context) {
            _currentMoveVector = context.ReadValue<Vector2>();
            Debug.Log(_currentMoveVector);
        }

        public void OnWakeMinions(InputAction.CallbackContext context) {
            foreach (var m in minionsList)
                if (!m.animator.GetBool(AnimParams.MinionIsActive))
                    m.IdleWakeUp();
        }

        private void Awake() {
        }

        private void Update() {
            SetMinionDirections();
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

        private void SetMinionDirections() {
            var cursorPos = new Vector2(transform.position.x, transform.position.y);

            foreach (var m in minionsList) {
                var minionPos = new Vector2(m.transform.position.x, m.transform.position.y);
                m.direction = (cursorPos - minionPos).normalized;
            }
        }
    }
}