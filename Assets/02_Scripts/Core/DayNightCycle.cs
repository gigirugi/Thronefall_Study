using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class DayNightCycle : MonoBehaviour
{ 
    public static DayNightCycle Instance { get; private set; }

    public bool IsDay { get; private set; } = true;

    [Header("Day Night Settings")]
    [SerializeField] Light directionalLight;
    [SerializeField] Vector3 dayLightDirection = new Vector3(50f, 0f, 0f);
    [SerializeField] Color dayLightColor = Color.white;
    [SerializeField] Vector3 nightLightDirection = new Vector3(30f, 60f, 0f);
    [SerializeField] Color nightLightColor = Color.gray4;

    [SerializeField] float changeTime = 2f;
    float timer = 0f;

    bool isBusy = false;

    public Action<bool> OnTimeChanged;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TryChangeTime()
    {
        if (!IsDay)
        {
            ResetTimer();
            return;
        }
        timer += Time.deltaTime;
        if (timer >= changeTime)
        {
            ChangeTime();
        }
    }
    public void ResetTimer()
    {
        timer = 0f;
    }
    [ContextMenu("Change Time")]
    public void ChangeTime()
    {
        StartCoroutine(CoChangeTime());
    }
    IEnumerator CoChangeTime()
    {
        if (isBusy) yield break;
        isBusy = true;
        float duration = 1f;
        float elapsed = 0f;
        Color startColor = IsDay ? dayLightColor : nightLightColor;
        Color endColor = IsDay ? nightLightColor : dayLightColor;
        Vector3 startDir = IsDay ? dayLightDirection : nightLightDirection;
        Vector3 endDir = IsDay ? nightLightDirection : dayLightDirection;

        IsDay = !IsDay;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            directionalLight.color = Color.Lerp(startColor, endColor, t);
            directionalLight.transform.rotation =
            Quaternion.Euler(Vector3.Lerp(startDir, endDir, t));
            yield return null;
        }
        directionalLight.color = endColor;
        directionalLight.transform.rotation = Quaternion.Euler(endDir);
        isBusy = false;
        OnTimeChanged?.Invoke(IsDay);
    }
    public float GetTimeNormalized()
    {
        return Mathf.Clamp01(timer / changeTime);
    }
}