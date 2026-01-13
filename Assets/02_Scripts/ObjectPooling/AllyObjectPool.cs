using UnityEngine;

public class AllyObjectPool : ObjectPoolBase<Ally>
{
	public static AllyObjectPool Instance { get; private set; }

	[SerializeField] StatDataSO[] statDataSOs;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}
	public Ally GetPooledAlly(AllyType allyType, BuildingTrainingCenter trainingCenter)
	{
		var ally = GetPooledObject();
		ally.Init(statDataSOs[(int)allyType], allyType, trainingCenter, trainingCenter.GetUnitDestination());
		return ally;
	}
}

public enum AllyType
{
	Swordman = 0,
	Spearman,
	Bowman,
	Crossbowman
}
