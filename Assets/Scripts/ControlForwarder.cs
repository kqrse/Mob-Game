using System;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;

public class ControlForwarder : MonoBehaviour {
    private readonly List<BaseMinion> _player1Minions = new List<BaseMinion>();
    private readonly List<BaseMinion> _player2Minions = new List<BaseMinion>();
    private readonly List<BaseMinion> _player3Minions = new List<BaseMinion>();
    private readonly List<BaseMinion> _player4Minions = new List<BaseMinion>();
    private Camera _cam;

    // Start is called before the first frame update
    private void Start() {
        BeginGetComponents();
        BeginAsserts();
        MinionSpawner.OnMinionSpawn += UpdateMinionList;
        UpdateMinionList(this, EventArgs.Empty);
    }

    // Update is called once per frame
    private void Update() {
        HandleInputs();
        SetMinionDirections();
    }

    private void HandleInputs() {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        foreach (var m in _player1Minions) m.animator.SetBool(AnimParams.MinionIsActive, true);
    }

    private void SetMinionDirections() {
        Vector2 mousePos = _cam.ScreenToWorldPoint(
            new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        foreach (var m in _player1Minions) {
            var minionPos = new Vector2(m.transform.position.x, m.transform.position.y);
            m.direction = (mousePos - minionPos).normalized;
        }
    }

    private void UpdateMinionList(object sender, EventArgs e) {
        var allMinions = new List<GameObject>();
        allMinions.AddRange(GameObject.FindGameObjectsWithTag("Minion"));

        foreach (var m in allMinions) {
            var minionScript = m.GetComponent<BaseMinion>();

            switch (minionScript.playerNumber) {
                case PlayerNumber.One:
                    if (!_player1Minions.Contains(minionScript)) _player1Minions.Add(minionScript);
                    break;
                case PlayerNumber.Two:
                    if (!_player2Minions.Contains(minionScript)) _player2Minions.Add(minionScript);
                    break;
                case PlayerNumber.Three:
                    if (!_player3Minions.Contains(minionScript)) _player3Minions.Add(minionScript);
                    break;
                case PlayerNumber.Four:
                    if (!_player4Minions.Contains(minionScript)) _player4Minions.Add(minionScript);
                    break;
                default:
                    Debug.LogError("minion does not have assigned player number");
                    break;
            }
        }
    }

    private void BeginAsserts() {
        Assert.IsNotNull(_cam);
    }

    private void BeginGetComponents() {
        _cam = Camera.main;
    }
}