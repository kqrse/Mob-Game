using System;
using System.Collections;
using System.Collections.Generic;
using ControlManagement;
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
    protected Rigidbody2D _rb;
    protected SpriteRenderer _sr;
    protected CircleCollider2D _footprintCollider;

    public Health minionHealth;
    public HealthBar healthBar;
    [SerializeField] private GameObject healthBarPrefab;
    protected float MaxHealth = 3;
    protected float BaseDamage = 1;
    protected bool IsAttackAOE = false;

    protected virtual void Start() {
        BeginGetBaseComponents();
        BeginBaseAsserts();
        BeginGetComponents();
        BeginAsserts();
        // minionHealth.Init(MaxHealth);
        // minionHealth.OnHealthDepleted += StartDeath;
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

        var filteredEnemiesHit = new List<Collider2D>();
        foreach (var enemy in enemiesHit) {
            if (enemy.CompareTag("Minion")) {
                if (enemy.GetComponent<BaseMinion>().playerNumber != playerNumber) filteredEnemiesHit.Add(enemy);
            } else if (enemy.CompareTag("MinionSpawner")) {
                if (enemy.GetComponent<MinionSpawner>().playerNumber != playerNumber) filteredEnemiesHit.Add(enemy);
            }
        }

        if (IsAttackAOE) {
            foreach (var enemyCollider in filteredEnemiesHit) {
                var enemyHealth = enemyCollider.GetComponent<Health>();
                enemyHealth.Damage(GetDamageValue());
            }
        }
        else {
            if (filteredEnemiesHit.Count == 0) return;
            var closestCollider = filteredEnemiesHit[0];

            var closestDistance = Vector2.Distance(attackPointPos, closestCollider.transform.position);
            foreach (var currentCollider in filteredEnemiesHit) {
                var enemyDistance = Vector2.Distance(attackPointPos, currentCollider.transform.position);
                if (enemyDistance < closestDistance) closestCollider = currentCollider;
            }

            // Debug.Log("ATTACKER: " + gameObject.name + " <> " + "TARGET: " + closestCollider.gameObject.name);
            var enemyHealth = closestCollider.GetComponent<Health>();
            if (enemyHealth != null) enemyHealth.Damage(GetDamageValue());
            else Debug.Log("Enemy was hit, but there is no health?");
        }
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_attackPoint.position, _minionMeleeRangeAOE);
    }

    public virtual float GetDamageValue() {
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

    protected virtual void StartDeath(object e, EventArgs eventArgs) {
        minionHealth.OnHealthDepleted -= StartDeath;
        _minionMeleeRange.OnMeleeRangeEntered -= EnterAttack;
        StartCoroutine(DeathAnimation());
        StartCoroutine(DelayedHealthbarDeletion());
    }

    private IEnumerator DelayedHealthbarDeletion() {
        yield return new WaitForSeconds(0.5f);
        Destroy(healthBar.gameObject);
    }

    protected IEnumerator DeathAnimation() {
        _rb.simulated = false;
        var newRotation = Quaternion.Euler(0, 0, 90);
        _sr.sortingLayerName = "Background";

        while (_sr.color.a > 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 7.5f);
            var color = _sr.color;
            var newAlpha = color.a -= 0.004f;
            _sr.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        FinishDeath();
    }

    private void FinishDeath() {
        GameObject.FindGameObjectWithTag("MinionManager")
            .GetComponent<MinionUpdater>()
            .RemoveMinionFromList(this);
        Destroy(gameObject);
    }

    protected virtual void BeginAsserts() {
        Assert.IsNotNull(_minionMeleeRange);
        Assert.IsNotNull(_minionAttackPoint);
        Assert.IsNotNull(_attackRangeCollider);
        Assert.IsNotNull(_attackPoint);
        Assert.IsNotNull(meleeAttackAnim);
        Assert.IsNotNull(healthBar);
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
        Assert.IsNotNull(minionHealth);
    }

    protected void BeginGetBaseComponents() {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        _footprintCollider = GetComponentInChildren<Footprint>().GetFootprintCollider();
        minionHealth = GetComponent<Health>();
        minionHealth.Init(MaxHealth);
        healthBar = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity).GetComponent<HealthBar>();
        healthBar.Init(transform, minionHealth, 10f);
        minionHealth.OnHealthDepleted += StartDeath;
    }
}