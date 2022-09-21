using System;
using System.Collections;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;

public class BaseMinion : MonoBehaviour {
    public PlayerNumber playerNumber;
    [NonSerialized] public Animator animator;
    [NonSerialized] public Vector2 direction;

    private MinionMeleeRange _minionMeleeRange;
    private MinionMeleePoint _minionMeleePoint;

    protected float MovementSpeed = 2f;
    protected float AttackCooldown = 0.75f;
    protected float AttackRecoverySpeed = 0.5f;

    protected Transform _attackPoint;
    protected CircleCollider2D _attackRangeCollider;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    protected virtual void Start() {
        BeginGetBaseComponents();
        BeginGetComponents();
        BeginAsserts();
        _minionMeleeRange.OnMeleeRangeEntered += EnterAttack;
    }

    private void Update() {
    }

    protected virtual void EnterAttack(object sender, EventArgs e) {
        if (animator.GetBool(AnimParams.MinionIsStunned) ||
            animator.GetBool(AnimParams.MinionIsAttack) ||
            !animator.GetBool(AnimParams.MinionCanAttack))
            return;

        StartCoroutine(StartAttack());
    }

    protected virtual IEnumerator StartAttack() {
        animator.SetBool(AnimParams.MinionIsAttack, true);
        animator.SetBool(AnimParams.MinionCanAttack, false);
        _rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(AttackCooldown * AttackRecoverySpeed);
        animator.SetBool(AnimParams.MinionIsAttack, false);
        yield return new WaitForSeconds(AttackCooldown * AttackRecoverySpeed);
        animator.SetBool(AnimParams.MinionCanAttack, true);
    }

    public void Move() {
        _rb.velocity = direction * MovementSpeed;
        _sr.flipX = _rb.velocity.x > 0;
    }

    protected virtual void BeginAsserts() {
        Assert.IsFalse(playerNumber == PlayerNumber.Unassigned);
    }

    protected virtual void BeginGetComponents() {
        _minionMeleeRange = GetComponentInChildren<MinionMeleeRange>();
        _minionMeleePoint = _minionMeleeRange.GetComponentInChildren<MinionMeleePoint>();
        _attackRangeCollider = _minionMeleeRange.GetComponent<CircleCollider2D>();
        _attackPoint = _minionMeleePoint.transform;
    }

    protected void BeginGetBaseComponents() {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
}