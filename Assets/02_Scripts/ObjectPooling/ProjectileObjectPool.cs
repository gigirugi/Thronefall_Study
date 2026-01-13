using System.Collections.Generic;
using UnityEngine;

public class ProjectileObjectPool : ObjectPoolBase<Projectile>
{
    public static ProjectileObjectPool Instance { get; private set; }

	
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}
	
}
