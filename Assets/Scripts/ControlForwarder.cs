using System;
using System.Collections;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;

public class ControlForwarder : MonoBehaviour {
    private List<Minion> _player1Minions = new List<Minion>();
    private List<Minion> _player2Minions = new List<Minion>();
    private List<Minion> _player3Minions = new List<Minion>();
    private List<Minion> _player4Minions = new List<Minion>();

    // Start is called before the first frame update
    void Start() {
        MinionSpawner.OnMinionSpawn += UpdateMinionList;
    }

    // Update is called once per frame
    void Update() {
        // MoveMinions();
    }

    private void MoveMinions() {
        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(
        //     new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        //
        // foreach (var m in _player1Minions) {
        //     Vector2 minionPos = new Vector2(m.transform.position.x, m.transform.position.y);
        //     Vector2 direction = mousePos - minionPos;
        //     direction.Normalize();
        //     m.Move(direction);
        // }
    }

    private void UpdateMinionList(object sender, EventArgs e) {
        List<GameObject> allMinions = new List<GameObject>();
        allMinions.AddRange(GameObject.FindGameObjectsWithTag("Minion"));
        
        foreach (GameObject m in allMinions) {
            Minion minionScript = m.GetComponent<Minion>();
            
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
}