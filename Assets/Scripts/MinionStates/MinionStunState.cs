using System.Collections;
using System.Collections.Generic;
using Globals;
using UnityEngine;

public class MinionStunState : StateMachineBehaviour {
    private BaseMinion _baseMinionScript;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _baseMinionScript = animator.GetComponent<BaseMinion>();
        animator.SetBool(AnimParams.MinionIsMoving, false);
        animator.SetBool(AnimParams.MinionIsAttack, false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }
}