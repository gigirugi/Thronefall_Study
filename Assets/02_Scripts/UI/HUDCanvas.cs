using UnityEngine;
using UnityEngine.UI;

public class HUDCanvas : MonoBehaviour
{
    [Header("Day/Night Cycle")]
    [SerializeField] Sprite[] dayNightSprites; // 0: Day, 1: Night
    [SerializeField] Image dayNightImage;
    [SerializeField] Image dayNightProgressImage;
    private void OnEnable()
    {
        DayNightCycle.Instance.OnTimeChanged += DayNightCycle_OnDayNightChanged;
    }

    private void OnDisable()
    {
        DayNightCycle.Instance.OnTimeChanged -= DayNightCycle_OnDayNightChanged;
    }
    private void DayNightCycle_OnDayNightChanged(bool isDay)
    {
        Sprite sprite = isDay ? dayNightSprites[0] : dayNightSprites[1];
        dayNightImage.sprite = sprite;
    }
    private void Update()
    {
        if (!DayNightCycle.Instance.IsDay)
        {
            dayNightProgressImage.fillAmount = 0;
            return;
        }

        dayNightProgressImage.fillAmount = DayNightCycle.Instance.GetTimeNormalized();
    }
}
