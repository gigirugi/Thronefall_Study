using UnityEngine;

public class BuildingCastle : BuildingBase
{
	[SerializeField] BuildingSpot[] buildingSpots;
	protected override void Combat_OnDie()
	{
		base.Combat_OnDie();
		Debug.Log("Game Over!");
	}
	protected override void Upgrade(int index)
	{
		base.Upgrade(index);

		foreach (var spot in buildingSpots)
		{
			spot.gameObject.SetActive(true);
		}

		GameManager.Instance.IncreasePlayerLevel();
	}
}
