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
    private MinionAttackPoint _minionAttackPoint;

    protected float MovementSpeed = 2f;
    protected float AttackCooldown = 0.75f;
    protected float AttackRecoverySpeed = 0.5f;

    protected Transform _attackPoint;
    protected CircleCollider2D _attackRangeCollider;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private CircleCollider2D _footprintCollider;

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

    public void IdleWakeUp() {
        animator.SetBool(AnimParams.MinionIsActive, true);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _footprintCollider.isTrigger = false;
    }

    protected virtual void BeginAsserts() {
        Assert.IsFalse(playerNumber == PlayerNumber.Unassigned);
        Assert.IsNotNull(_minionMeleeRange);
        Assert.IsNotNull(_minionAttackPoint);
        Assert.IsNotNull(_attackRangeCollider);
        Assert.IsNotNull(_attackPoint);
    }

    protected virtual void BeginGetComponents() {
        _minionMeleeRange = GetComponentInChildren<MinionMeleeRange>();
        _minionAttackPoint = _minionMeleeRange.GetComponentInChildren<MinionAttackPoint>();
        _attackRangeCollider = _minionMeleeRange.GetComponent<CircleCollider2D>();
        _attackPoint = _minionAttackPoint.transform;
    }

    protected void BeginGetBaseComponents() {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        _footprintCollider = GetComponentInChildren<Footprint>().GetFootprintCollider();
    }
}