using UnityEngine;
using UnityEngine.Events;

public class BuildingTrainingCenter : BuildingBase
{
    AllyType allyType;
    [SerializeField, EnumButtons] AllyType[] allyTypes;

    [SerializeField] int maxAllyCount = 4;
    public int currentAllyCount = 0;

    [SerializeField] float trainingInterval = 5f;
    float trainingTimer = 0f;

    [SerializeField] Vector3 unitDestination;
    public Vector3 GetUnitDestination() => unitDestination;
	protected override void OnEnable()
	{
		base.OnEnable();

		UnityAction[] actions = new UnityAction[allyTypes.Length];
		for (int i = 0; i < allyTypes.Length; i++)
		{
			int ii = i;
			actions[i] = () =>
			{
				allyType = allyTypes[ii];
				for (int j = currentAllyCount; j < maxAllyCount; j++)
				{
					SpawnAlly();
				}
			};
		}

		var names = new string[allyTypes.Length];
		for (int i = 0; i < allyTypes.Length; i++)
		{
			names[i] = allyTypes[i].ToString();
		}

		UpgradeCanvas.Instance.Init(allyTypes.Length, names, actions);
	}
	private void SpawnAlly()
    {
        var ally = AllyObjectPool.Instance.GetPooledAlly(allyType, this);
        ally.transform.position = transform.position;
        ally.gameObject.SetActive(true);
        currentAllyCount++;
    }
	private void Update()
	{
		if (DayNightCycle.Instance.IsDay) return;

		if (maxAllyCount > currentAllyCount)
		{
			trainingTimer += Time.deltaTime;
			if (trainingTimer >= trainingInterval)
			{
				trainingTimer = 0f;
				SpawnAlly();
			}
		}
		else
		{
			trainingTimer = 0f;
		}
	}
	protected override void DayNightCycle_OnTimeChanged(bool isDay)
	{
		base.DayNightCycle_OnTimeChanged(isDay);

		if (isDay)
		{
			enabled = true;

			for (int i = currentAllyCount; i < maxAllyCount; i++)
			{
				SpawnAlly();
			}
		}
	}
	protected override void Combat_OnDie()
	{
		base.Combat_OnDie();

		enabled = false;
	}
	private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(unitDestination, 0.5f);
    }
}
