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
    private float _minionMeleeRangeAOE = 0.1f;
    private MinionAttackPoint _minionAttackPoint;
    [SerializeField] private GameObject meleeAttackAnim;

    protected float MovementSpeed = 2f;
    protected float AttackCooldownTime = 0.45f;
    protected float AttackRecoveryTime = 0.3f;

    [SerializeField] protected Transform _attackPoint;
    protected CircleCollider2D _attackRangeCollider;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private CircleCollider2D _footprintCollider;

    protected Health MinionHealth;
    protected float MaxHealth = 3;
    protected float BaseDamage = 1;
    protected bool IsAttackAOE = false;

    protected virtual void Start() {
        BeginGetBaseComponents();
        BeginBaseAsserts();
        BeginGetComponents();
        BeginAsserts();
        MinionHealth.Init(MaxHealth);
        MinionHealth.OnHealthDepleted += StartDeath;
        _minionMeleeRange.OnMeleeRangeEntered += EnterAttack;
    }

    private void Update() {
    }

    protected virtual void EnterAttack(object sender, EventArgs e) {
        if (animator.GetBool(AnimParams.MinionIsStunned) || animator.GetBool(AnimParams.MinionIsAttack) ||
            !animator.GetBool(AnimParams.MinionCanAttack))
            return;

        StartCoroutine(StartAttack());
    }

    protected virtual IEnumerator StartAttack() {
        animator.SetBool(AnimParams.MinionIsAttack, true);
        animator.SetBool(AnimParams.MinionCanAttack, false);
        _rb.velocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        var attackPointPos = _attackPoint.transform.position;
        _sr.flipX = attackPointPos.x - transform.position.x > 0;

        AttemptAttack(attackPointPos);
        Instantiate(meleeAttackAnim, attackPointPos, Quaternion.identity);

        yield return new WaitForSeconds(AttackRecoveryTime);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        animator.SetBool(AnimParams.MinionIsAttack, false);

        yield return new WaitForSeconds(AttackCooldownTime);
        animator.SetBool(AnimParams.MinionCanAttack, true);
    }

    private void AttemptAttack(Vector3 attackPointPos) {
        var enemiesHit = Physics2D.OverlapCircleAll(attackPointPos, _minionMeleeRangeAOE,
            LayerMask.GetMask("Attackable"));

        if (IsAttackAOE) {
            foreach (var enemyCollider in enemiesHit) {
                if (enemyCollider.GetComponent<BaseMinion>().playerNumber == playerNumber) continue;
                var enemyHealth = enemyCollider.GetComponent<Health>();
                enemyHealth.Damage(GetDamageValue());
            }
        }
        else {
            var closestCollider = enemiesHit[0];

            var closestDistance = Vector2.Distance(attackPointPos, closestCollider.transform.position);
            foreach (var currentCollider in enemiesHit) {
                var enemyDistance = Vector2.Distance(attackPointPos, currentCollider.transform.position);
                if (enemyDistance < closestDistance) closestCollider = currentCollider;
            }

            Debug.Log("ATTACKER: " + gameObject.name + " <> " + "TARGET: " + closestCollider.gameObject.name);
            if (closestCollider.GetComponent<BaseMinion>().playerNumber != playerNumber) {
                var enemyHealth = closestCollider.gameObject.GetComponent<Health>();
                Assert.IsNotNull(enemyHealth);
                enemyHealth.Damage(GetDamageValue());
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_attackPoint.position, _minionMeleeRangeAOE);
    }

    protected virtual float GetDamageValue() {
        return BaseDamage;
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

    protected void StartDeath(object e, EventArgs eventArgs) {
        MinionHealth.OnHealthDepleted -= StartDeath;
        _minionMeleeRange.OnMeleeRangeEntered -= EnterAttack;
        _rb.simulated = false;
        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation() {
        yield return new WaitForSeconds(1f);
        FinishDeath();
    }

    private void FinishDeath() {
        Destroy(gameObject);
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