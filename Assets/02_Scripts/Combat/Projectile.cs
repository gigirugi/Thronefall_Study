using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	Combat owner;
	Combat target;
	int damage;
	AttackType attackType;
	[SerializeField] float speed = 10f;

	bool hasStarted = false;

	[SerializeField] float alertRadius = 5f;

	public void Initialize(Combat owner, Combat target, int damage, AttackType attackType)
	{
		this.owner = owner;
		this.target = target;
		this.damage = damage;
		this.attackType = attackType;

		hasStarted = false;
	}
	private IEnumerator Start()
	{
		hasStarted = true;

		Vector3 startPos = transform.position;
		Vector3 targetPos = target.transform.position;

		float initialDistance = Vector3.Distance(startPos, target.transform.position);
		float traveledDistance = 0f;

		float archHeight = initialDistance / 2f;

		while (traveledDistance < initialDistance - .01f)
		{
			traveledDistance += speed * Time.deltaTime;
			float archOffset = Mathf.PingPong(traveledDistance / initialDistance, 1f) * archHeight;
			targetPos = target ? target.transform.position : targetPos;
			Vector3 currentPos = Vector3.Lerp(startPos + Vector3.up * archOffset, targetPos, traveledDistance / initialDistance);
			transform.position = currentPos;

			yield return null;
		}
		if (target && owner)
			target.TakeDamage(damage, owner, attackType);

		GainAggro();

		ProjectileObjectPool.Instance.ReturnPooledObject(this);
	}
	void GainAggro()
	{
		if (!owner)
			return;

		var results = Physics.OverlapSphere(transform.position, alertRadius);
		foreach (var collider in results)
		{
			if (collider.TryGetComponent(out Combat combat))
			{
				if (owner.CanAttack(combat))
				{
					combat.TakeDamage(0, owner, 0);
				}
			}
		}
	}
	private void Update()
	{
		if (!hasStarted)
			StartCoroutine(Start());
	}
}
