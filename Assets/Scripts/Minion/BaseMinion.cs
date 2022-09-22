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
    private float _minionMeleeRangeAOE = 1f;
    private MinionAttackPoint _minionAttackPoint;
    [SerializeField] private GameObject meleeAttackAnim;

    protected float MovementSpeed = 2f;
    protected float AttackCooldown = 0.75f;
    protected float AttackRecoverySpeed = 0.5f;

    protected Transform _attackPoint;
    protected CircleCollider2D _attackRangeCollider;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private CircleCollider2D _footprintCollider;

    protected Health MinionHealth;
    protected int MaxHealth = 3;

    protected virtual void Start() {
        BeginGetBaseComponents();
        BeginBaseAsserts();
        BeginGetComponents();
        BeginAsserts();
        MinionHealth.Init(MaxHealth);
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
        // _rb.velocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        var attackPointPos = _attackPoint.transform.position;
        _sr.flipX = attackPointPos.x - transform.position.x > 0;

        Physics2D.OverlapCircleAll(_attackPoint.position, _minionMeleeRangeAOE,
            LayerMask.NameToLayer("Attackable"));
        Instantiate(meleeAttackAnim, attackPointPos, Quaternion.identity);

        yield return new WaitForSeconds(AttackCooldown * AttackRecoverySpeed);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        animator.SetBool(AnimParams.MinionIsAttack, false);

        yield return new WaitForSeconds(AttackCooldown * AttackRecoverySpeed);
        animator.SetBool(AnimParams.MinionCanAttack, true);
    }

    public void Move() {
        if (!animator.GetBool(AnimParams.MinionIsActive)) return;
        _rb.velocity = direction * MovementSpeed;

        if (_rb.velocity != Vector2.zero)
            _sr.flipX = direction.x > 0;
    }

    public void IdleWakeUp() {
        animator.SetBool(AnimParams.MinionIsAwake, true);
        animator.SetBool(AnimParams.MinionIsActive, true);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _footprintCollider.isTrigger = false;
    }

    protected virtual void BeginAsserts() {
        Assert.IsNotNull(_minionMeleeRange);
        Assert.IsNotNull(_minionAttackPoint);
        Assert.IsNotNull(_attackRangeCollider);
        Assert.IsNotNull(_attackPoint);
        Assert.IsNotNull(meleeAttackAnim);
    }

    protected virtual void BeginGetComponents() {
        _minionMeleeRange = GetComponentInChildren<MinionMeleeRange>();
        _minionAttackPoint = _minionMeleeRange.GetComponentInChildren<MinionAttackPoint>();
        _attackRangeCollider = _minionMeleeRange.GetComponent<CircleCollider2D>();
        _attackPoint = _minionAttackPoint.transform;
    }

    protected void BeginBaseAsserts() {
        Assert.IsFalse(playerNumber == PlayerNumber.Unassigned);
        Assert.IsNotNull(_rb);
        Assert.IsNotNull(_sr);
        Assert.IsNotNull(animator);
        Assert.IsNotNull(_footprintCollider);
        Assert.IsNotNull(MinionHealth);
    }

    protected void BeginGetBaseComponents() {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        _footprintCollider = GetComponentInChildren<Footprint>().GetFootprintCollider();
        MinionHealth = GetComponent<Health>();
    }
}