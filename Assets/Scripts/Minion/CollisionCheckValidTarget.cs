using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionCheckValidTarget : MonoBehaviour {
    protected virtual bool IsInvalidAttackTarget(Collider2D targetCollider) {
        if (targetCollider.gameObject.layer != LayerMask.NameToLayer("Attackable")) return true;

        if (targetCollider.CompareTag("Minion"))
            return GetComponentInParent<BaseMinion>().playerNumber ==
                   targetCollider.GetComponent<BaseMinion>().playerNumber;

        if (targetCollider.CompareTag("MinionSpawner"))
            return GetComponentInParent<BaseMinion>().playerNumber ==
                   targetCollider.GetComponent<MinionSpawner>().playerNumber;

        return true;
    }
}