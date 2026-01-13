using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	public static WaveManager Instance { get; private set; }

	[SerializeField] WaveDataSO[] waveDataSOs;
	int nightIndex = 0;
	int waveIndex = 0;

	int enemyCount = 0;

    [SerializeField] WaveInfoUI waveInfoUIPrefab;
    // 활성화된 웨이브 정보 UI 목록
    List<WaveInfoUI> activeWaveInfoUIs = new List<WaveInfoUI>();
    public void DecreaseEnemyCount()
	{
		enemyCount--;

		if (enemyCount <= 0)
			DayNightCycle.Instance.ChangeTime();
	}
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void OnEnable()
	{
		DayNightCycle.Instance.OnTimeChanged += DayNightCycle_OnTimeChanged;
	}
	private void OnDisable()
	{
		DayNightCycle.Instance.OnTimeChanged -= DayNightCycle_OnTimeChanged;
	}
	private void DayNightCycle_OnTimeChanged(bool isDay)
	{
		if (nightIndex >= waveDataSOs.Length)
		{
			print("모든 밤이 종료되었습니다. Game Won");
			return;
		}

		if (isDay)
		{
			print("낮이 되었습니다. 다음 밤을 준비하세요.");
			return;
		}

		StartWave();
	}
	void StartWave()
	{
		if (waveIndex == 0)
		{
			enemyCount = waveDataSOs[nightIndex].GetTotalEnemyCount();
		}

		EnemyData[] enemyDatas = waveDataSOs[nightIndex].GetWaveEnemyData(waveIndex, out Vector3 spawnPos);

		foreach (var enemyData in enemyDatas)
		{
			for (int i = 0; i < enemyData.quantity; i++)
			{
				Instantiate(enemyData.enemyPrefab, spawnPos, Quaternion.identity);
			}
		}

		waveIndex++;

		if (waveIndex >= waveDataSOs[nightIndex].GetWaveCount())
		{
			waveIndex = 0;
			nightIndex++;
		}
		else
		{
			float interval = waveDataSOs[nightIndex].GetWaveInterval(waveIndex);

			// 다음 웨이브 시작 예약
			Invoke(nameof(StartWave), interval);

			// 웨이브 정보 UI 갱신
			InitInfoUI(interval);
		}
	}
    private void InitInfoUI(float totalTime)
    {
        ClearPastUIs();

        // 현재 웨이브의 적 데이터 가져오기
        EnemyData[] enemyDatas = waveDataSOs[nightIndex].GetWaveEnemyData(waveIndex, out Vector3 spawnPos);
        for (int i = 0; i < enemyDatas.Length; i++)
        {
            Vector3 offset = new Vector3(i * 1.2f, 1.5f, i * 1.2f);

            var infoUI = Instantiate(waveInfoUIPrefab, spawnPos + offset, waveInfoUIPrefab.transform.rotation);
            infoUI.SetUI(enemyDatas[i].icon, enemyDatas[i].quantity, totalTime);
            activeWaveInfoUIs.Add(infoUI);
        }
    }
    private void ClearPastUIs()
    {
        // 기존 UI 제거
        if (activeWaveInfoUIs.Count > 0)
        {
            foreach (var ui in activeWaveInfoUIs)
            {
                Destroy(ui.gameObject);
            }
            activeWaveInfoUIs.Clear();
        }
    }
}
