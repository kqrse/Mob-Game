using UnityEngine;

public class MinionMoveState : StateMachineBehaviour {
    private BaseMinion _baseMinionScript;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _baseMinionScript = animator.GetComponent<BaseMinion>();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _baseMinionScript.Move();
    }
}