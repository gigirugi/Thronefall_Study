using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StatDataSO", menuName = "Scriptable Objects/StatDataSO")]
public class StatDataSO : ScriptableObject
{
	[field: SerializeField] public int MaxHealth { get; private set; } = 100;
	[field: SerializeField] public int AttackDamage { get; private set; } = 10;
	[field: SerializeField] public float AttackRange { get; private set; } = 1.5f;
	[field: SerializeField] public float AttackCooldown { get; private set; } = 1.0f;
	[field: SerializeField] public float MoveSpeed { get; private set; } = 3.0f;
	[field: SerializeField, EnumButtons] public AttackType AttackTypes { get; private set; } = AttackType.Melee;
	[field: SerializeField, EnumButtons] public AttackType WeaknessTypes { get; private set; } = 0;
	[field: SerializeField, EnumButtons] public BodyType BodyType { get; private set; } = BodyType.Ground;
	[field: SerializeField] public bool IsEnemy { get; private set; } = false;
	[field: SerializeField] public float BodySize { get; private set; } = 0.5f;
	[field: SerializeField] public float FollowTime { get; private set; } = 5.0f;
}

public enum BodyType
{
	Ground,
	Air
}

[Flags]
public enum AttackType
{
	Melee = 1 << 0,
	Ranged = 1 << 1,
	Siege = 1 << 2,
}