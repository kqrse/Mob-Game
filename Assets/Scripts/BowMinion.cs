﻿using System;
using System.Collections;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;

public class BowMinion : BaseMinion {
    private MinionRangedRange _minionRangedRange;
    private MinionRangedPoint _minionRangedPoint;

    protected override void Start() {
        BeginGetBaseComponents();
        BeginAsserts();
        InitializeStats();
    }

    private void Update() {
    }

    private void InitializeStats() {
        MovementSpeed = 2.25f;
        AttackCooldown = 1f;
        AttackRecoverySpeed = 0.4f;
    }

    // protected override void EnterAttack(object sender, EventArgs e) {
    //     if (animator.GetBool(AnimParams.MinionIsStunned) ||
    //         animator.GetBool(AnimParams.MinionIsAttack) ||
    //         !animator.GetBool(AnimParams.MinionCanAttack)) {
    //         return;
    //     }
    //
    //     StartCoroutine(StartAttack());
    // }
    //
    // protected override IEnumerator StartAttack() {
    //     animator.SetBool(AnimParams.MinionIsAttack, true);
    //     animator.SetBool(AnimParams.MinionCanAttack, false);
    //     _rb.velocity = Vector2.zero;
    //
    //     yield return new WaitForSeconds(AttackCooldown * AttackRecoverySpeed);
    //     animator.SetBool(AnimParams.MinionIsAttack, false);
    //     yield return new WaitForSeconds(AttackCooldown * AttackRecoverySpeed);
    //     animator.SetBool(AnimParams.MinionCanAttack, true);
    // }

    protected override void BeginAsserts() {
        Assert.IsFalse(playerNumber == PlayerNumber.Unassigned);
    }

    protected override void BeginGetComponents() {
        _minionRangedRange = GetComponentInChildren<MinionRangedRange>();
        _minionRangedPoint = _minionRangedRange.GetComponentInChildren<MinionRangedPoint>();
        _attackRangeCollider = _minionRangedRange.GetComponent<CircleCollider2D>();
        _attackPoint = _minionRangedPoint.transform;
    }
}