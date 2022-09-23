using System;
using System.Collections;
using System.Numerics;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BowMinion : BaseMinion {
    private MinionRangedRange _minionRangedRange;
    private MinionAttackPoint _minionRangedPoint;
    [SerializeField] private GameObject arrowPrefab;

    protected override void Start() {
        InitializeStats();
        BeginGetBaseComponents();
        BeginBaseAsserts();
        BeginGetComponents();
        BeginAsserts();
        // minionHealth.Init(MaxHealth);
        _minionRangedRange.OnRangedRangeEntered += EnterAttack;
    }

    private void InitializeStats() {
        MovementSpeed = 1.75f;
        AttackCooldownTime = 0.7f;
        AttackRecoveryTime = 0.3f;
        MaxHealth = 3;
    }

    public override float GetDamageValue() {
        return BaseDamage;
    }

    protected override void EnterAttack(object sender, EventArgs e) {
        if (animator.GetBool(AnimParams.MinionIsStunned) || animator.GetBool(AnimParams.MinionIsAttack) ||
            !animator.GetBool(AnimParams.MinionCanAttack))
            return;

        StartCoroutine(StartAttack());
    }

    protected override IEnumerator StartAttack() {
        animator.SetBool(AnimParams.MinionIsAttack, true);
        animator.SetBool(AnimParams.MinionCanAttack, false);
        _rb.velocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        var attackPointPos = _attackPoint.transform.position;
        _sr.flipX = attackPointPos.x - transform.position.x > 0;

        var arrowDirection = VectorHelper.GetVectorToPoint(transform.position,
            new Vector3(attackPointPos.x, attackPointPos.y + 3, attackPointPos.y));
        var arrowAngle = Vector2.SignedAngle(Vector2.left, arrowDirection);

        var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity)
            .GetComponent<ArrowProjectile>();
        arrow.Init(this, attackPointPos, 2f, 1f);
        // arrow.Init(this, arrowDirection, arrowAngle, 5f);

        yield return new WaitForSeconds(AttackRecoveryTime);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        animator.SetBool(AnimParams.MinionIsAttack, false);

        yield return new WaitForSeconds(AttackCooldownTime);
        animator.SetBool(AnimParams.MinionCanAttack, true);
    }

    protected override void StartDeath(object e, EventArgs eventArgs) {
        minionHealth.OnHealthDepleted -= StartDeath;
        _minionRangedRange.OnRangedRangeEntered += EnterAttack;
        StartCoroutine(DeathAnimation());
    }

    protected override void BeginAsserts() {
        Assert.IsFalse(playerNumber == PlayerNumber.Unassigned);
        Assert.IsNotNull(_minionRangedRange);
        Assert.IsNotNull(_minionRangedPoint);
        Assert.IsNotNull(_attackRangeCollider);
        Assert.IsNotNull(minionHealth);
    }

    protected override void BeginGetComponents() {
        _minionRangedRange = GetComponentInChildren<MinionRangedRange>();
        _minionRangedPoint = _minionRangedRange.GetComponentInChildren<MinionAttackPoint>();
        _attackRangeCollider = _minionRangedRange.GetComponent<CircleCollider2D>();
        _attackPoint = _minionRangedPoint.transform;
    }
}