using UnityEngine;

public class MinionIdleState : StateMachineBehaviour {
    private BaseMinion _baseMinionScript;
    private SpriteRenderer _sr;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _baseMinionScript = animator.GetComponent<BaseMinion>();
        _sr = animator.GetComponent<SpriteRenderer>();
        _sr.color = Color.yellow;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _sr.color = Color.white;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }
}