using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveDataSO", menuName = "Scriptable Objects/WaveDataSO")]
public class WaveDataSO : ScriptableObject
{
	[SerializeField] SpawnData[] enemySpawnDataArray;

	[Serializable]
	public struct SpawnData
	{
		public EnemyData[] enemyDatas;
		public int interval;
		public Vector3 spawnPos;
	}
	public int GetTotalEnemyCount()
	{
		int totalCount = 0;
		foreach (var waveData in enemySpawnDataArray)
		{
			foreach (var enemyData in waveData.enemyDatas)
			{
				totalCount += enemyData.quantity;
			}
		}
		return totalCount;
	}
	public int GetWaveCount()
	{
		return enemySpawnDataArray.Length;
	}
	public float GetWaveInterval(int waveCount)
	{
		if (waveCount >= enemySpawnDataArray.Length)
		{
			Debug.LogWarning("유효하지 않은 waveCount 입니다.");
			return 0f;
		}
		return enemySpawnDataArray[waveCount].interval;
	}
	public EnemyData[] GetWaveEnemyData(int waveCount, out Vector3 spawnPos)
	{
		spawnPos = enemySpawnDataArray[waveCount].spawnPos;
		return enemySpawnDataArray[waveCount].enemyDatas;
	}

}

[Serializable]
public struct EnemyData
{
	public Enemy enemyPrefab;
	public int quantity;
    public Sprite icon;
}