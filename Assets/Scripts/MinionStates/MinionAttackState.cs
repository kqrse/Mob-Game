using Globals;
using UnityEngine;

public class MinionAttackState : StateMachineBehaviour {
    private BaseMinion _baseMinionScript;
    private SpriteRenderer _sr;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _baseMinionScript = animator.GetComponent<BaseMinion>();
        _sr = animator.GetComponent<SpriteRenderer>();
        _sr.color = Color.red;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // animator.SetBool(AnimParams.MinionIsAttack, false);
        _sr.color = Color.white;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }
}