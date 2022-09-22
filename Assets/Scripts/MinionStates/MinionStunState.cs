using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionStunState : StateMachineBehaviour {
    private BaseMinion _baseMinionScript;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _baseMinionScript = animator.GetComponent<BaseMinion>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }
}