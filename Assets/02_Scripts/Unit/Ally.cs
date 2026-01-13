using UnityEngine;

public class Ally : UnitBase
{
    BuildingTrainingCenter trainingCenter;

    [SerializeField] GameObject[] equipments;
	protected override void Combat_OnDie()
	{
		base.Combat_OnDie();

		combat.enabled = false;

		AllyObjectPool.Instance.ReturnPooledObject(this);
		trainingCenter.currentAllyCount--;
	}
	public void Init(StatDataSO statData, AllyType allyType, BuildingTrainingCenter trainingCenter, Vector3 unitDestination)
	{
		this.trainingCenter = trainingCenter;

		for (int i = 0; i < equipments.Length; i++)
		{
			equipments[i].SetActive(i == (int)allyType);
		}
		combat.Init(statData);
		SetDestination(unitDestination);
	}
}
