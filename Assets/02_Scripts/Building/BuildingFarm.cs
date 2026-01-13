using UnityEngine;

public class BuildingFarm : BuildingBase
{
    [SerializeField] int goldPerDay = 2;

	protected override void DayNightCycle_OnTimeChanged(bool isDay)
	{
		if (!isDestroyed && isDay)
			ResourceManager.Instance.AddOrSpendGold(goldPerDay);

		base.DayNightCycle_OnTimeChanged(isDay);
	}
}
