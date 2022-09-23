using System;
using System.Collections.Generic;
using Globals;
using UnityEngine;

namespace ControlManagement {
    public class MinionUpdater : MonoBehaviour {
        [SerializeField] private PlayerController playerOneController;
        [SerializeField] private PlayerController playerTwoController;
        [SerializeField] private PlayerController playerThreeController;
        [SerializeField] private PlayerController playerFourController;

        private void Start() {
            // MinionSpawner.OnMinionSpawn += UpdateMinionList;
            UpdateMinionList(this, EventArgs.Empty);
        }

        public void RemoveMinionFromList(BaseMinion minion) {
            switch (minion.playerNumber) {
                case PlayerNumber.One:
                    playerOneController.minionsList.Remove(minion);
                    break;
                case PlayerNumber.Two:
                    playerTwoController.minionsList.Remove(minion);
                    break;
                case PlayerNumber.Three:
                    playerThreeController.minionsList.Remove(minion);
                    break;
                case PlayerNumber.Four:
                    playerFourController.minionsList.Remove(minion);
                    break;
            }
        }

        private void UpdateMinionList(object sender, EventArgs e) {
            var allMinions = new List<GameObject>();
            allMinions.AddRange(GameObject.FindGameObjectsWithTag("Minion"));

            foreach (var m in allMinions) {
                var minionScript = m.GetComponent<BaseMinion>();

                switch (minionScript.playerNumber) {
                    case PlayerNumber.One:
                        if (!playerOneController.minionsList.Contains(minionScript))
                            playerOneController.minionsList.Add(minionScript);
                        break;
                    case PlayerNumber.Two:
                        if (!playerTwoController.minionsList.Contains(minionScript))
                            playerTwoController.minionsList.Add(minionScript);
                        break;
                    case PlayerNumber.Three:
                        if (!playerThreeController.minionsList.Contains(minionScript))
                            playerThreeController.minionsList.Add(minionScript);
                        break;
                    case PlayerNumber.Four:
                        if (!playerFourController.minionsList.Contains(minionScript))
                            playerFourController.minionsList.Add(minionScript);
                        break;
                    default:
                        Debug.LogError("minion does not have assigned player number");
                        break;
                }
            }
        }
    }
}