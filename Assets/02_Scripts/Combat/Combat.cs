using System;
using UnityEngine;

public class Combat : MonoBehaviour
{
	[field: SerializeField] public StatDataSO StatData { get; private set; }
	[field: SerializeField] public Transform FirePoint { get; private set; }
	public Combat TargetCombat { get; private set; }
	float followTimer = 0f;


    [SerializeField] HPCanvas hpCanvas;
    int currentHealth;
    float attackCooldownTimer = 0f;
	Collider[] results = new Collider[50];
	Collider selfCollider;

	[SerializeField] GameObject slashPrefab;

	public Action OnDie;
	private void Awake()
	{
		Init();
	}

	public void Init(StatDataSO statData = null)
	{
		if (statData) StatData = statData;

		currentHealth = StatData.MaxHealth;
		TryGetComponent(out selfCollider);

		if (!FirePoint)
			FirePoint = transform;
	}

	private void FixedUpdate()
	{
		attackCooldownTimer += Time.deltaTime;
		if (attackCooldownTimer >= StatData.AttackCooldown)
		{
			TryAttack();
		}
	}

	private void TryAttack()
	{
		if (!TargetCombat)
		{
			TargetCombat = GetNearestTarget();
		}

		if (!IsInRange())
		{
			followTimer += Time.deltaTime;

			if (followTimer >= StatData.FollowTime)
			{
				TargetCombat = null;
				followTimer = 0f;
			}

			return;
		}

		PerformAttack();
	}

    private void PerformAttack()
    {
		if (TargetCombat.enabled == false)
		{
			TargetCombat = null;
			return;
		}

		Action<Combat, AttackType> attackAction = StatData.AttackTypes.HasFlag(AttackType.Ranged) ? Shoot : Hit;
		attackAction(TargetCombat, StatData.AttackTypes);

		attackCooldownTimer = 0f;
		followTimer = 0f;
	}

    public bool IsInRange()
	{
		if (!TargetCombat)
			return false;

		Vector3 target = new(TargetCombat.transform.position.x, 0, TargetCombat.transform.position.z);
		Vector3 self = new(transform.position.x, 0, transform.position.z);
		Vector3 direction = target - self;

		return direction.magnitude - TargetCombat.StatData.BodySize <= StatData.AttackRange;
	}

	private Combat GetNearestTarget()
	{
		int length = Physics.OverlapSphereNonAlloc(transform.position, StatData.AttackRange, results);

		Combat nearest = null;
		float bestSqr = float.MaxValue;

		for (int i = 0; i < length; i++)
		{
			if (results[i] == selfCollider) continue;

			if (results[i].TryGetComponent(out Combat combat))
			{
				float sqr = (results[i].ClosestPoint(transform.position) - transform.position).sqrMagnitude;
				if (CanAttack(combat) && sqr < bestSqr)
				{
					bestSqr = sqr;
					nearest = combat;
				}
			}
		}
		return nearest;
	}

	private void Hit(Combat nearest, AttackType attackType)
	{
		Vector3 playerPos = FirePoint.position;
		playerPos.y = 0f;
		Vector3 targetPos = nearest.transform.position;
		targetPos.y = 0f;
		Vector3 direction = (targetPos - playerPos).normalized;
		slashPrefab.transform.position = FirePoint.position + direction * .5f;
		slashPrefab.transform.forward = direction;
		slashPrefab.SetActive(true);
		nearest.TakeDamage(StatData.AttackDamage, this, attackType);

		if (slashPrefab.transform.parent == transform)
			slashPrefab.transform.SetParent(null);

		Invoke(nameof(DisableSlash), 0.2f);
	}

	void DisableSlash()
	{
		slashPrefab.SetActive(false);
	}

	private void Shoot(Combat nearest, AttackType attackType)
	{
		var projectile = ProjectileObjectPool.Instance.GetPooledObject();
		projectile.Initialize(this, nearest, StatData.AttackDamage, attackType);
		projectile.transform.position = FirePoint ? FirePoint.position : transform.position;
		projectile.gameObject.SetActive(true);
	}

	public bool CanAttack(Combat combat)
	{
		bool isEnable = combat.enabled;
		bool isEnemyTarget = StatData.IsEnemy != combat.StatData.IsEnemy;
		bool canReach = combat.StatData.BodyType == BodyType.Ground || StatData.AttackTypes.HasFlag(AttackType.Ranged);
		return isEnable&& isEnemyTarget && canReach;
	}

	public void TakeDamage(int damage, Combat combat, AttackType attackType)
	{
		if (StatData.WeaknessTypes.HasFlag(attackType))
		{
			damage = (int)(damage * 1.5f);
		}

		currentHealth -= damage;

        if (damage > 0 && hpCanvas)
            hpCanvas.ShowSlider((float)currentHealth / StatData.MaxHealth);

        if (!IsInRange())
			TargetCombat = combat;

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		OnDie?.Invoke();

		if (OnDie == null)
			Destroy(gameObject);
	}

	private void OnDrawGizmosSelected()
	{
		if (!StatData) return;

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, StatData.AttackRange);
	}
}
