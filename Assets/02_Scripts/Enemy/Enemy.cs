
public class Enemy : UnitBase
{
	protected override void Combat_OnDie()
	{
		base.Combat_OnDie();

		WaveManager.Instance.DecreaseEnemyCount();
		Destroy(gameObject);
	}
}
