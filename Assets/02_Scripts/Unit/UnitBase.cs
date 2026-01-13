using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Combat))]
public abstract class UnitBase : MonoBehaviour
{
	NavMeshAgent agent;
	protected Combat combat;
	[SerializeField] Vector3 destination;
	void Awake()
	{
		TryGetComponent(out agent);
		TryGetComponent(out combat);

		agent.speed = combat.StatData.MoveSpeed;
		agent.stoppingDistance = combat.StatData.AttackRange;
		SetDestination(destination);
	}

	protected void SetDestination(Vector3 destination)
	{
		this.destination = destination;
	}
	private void OnEnable()
	{
		if(!combat.enabled)
			combat.enabled = true;

		combat.OnDie += Combat_OnDie;
	}
	private void OnDisable()
	{
		combat.OnDie -= Combat_OnDie;
	}
	private void Update()
	{
		if (combat.IsInRange())
		{
			agent.isStopped = true;
		}
		else
		{
			agent.isStopped = false;

			if (combat.TargetCombat)
			{
				Vector3 destination = combat.TargetCombat.transform.position;
				destination.y = transform.position.y;
				agent.SetDestination(destination);
			}
			else
				agent.SetDestination(destination);
		}
	}
	protected virtual void Combat_OnDie()
	{

	}
}
