using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource audioSource_BGM;
    [SerializeField] AudioSource audioSource_Footstep;
    [SerializeField] AudioSource audioSource_Hit;

    [Header("Day/Night Cycle")]
    [SerializeField] AudioClip dayBGM;
    [SerializeField] AudioClip nightBGM;
    [SerializeField] AudioClip timeChangeSFX;

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
        AudioClip newBGM = isDay ? dayBGM : nightBGM;
        ChangeBGM(newBGM);
        PlayOneShot(timeChangeSFX);
    }

    public void ChangeBGM(AudioClip newBGM)
    {
        audioSource_BGM.Stop();
        audioSource_BGM.clip = newBGM;
        audioSource_BGM.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSource_BGM.PlayOneShot(clip);
    }

    public void PlayFootstep()
    {

        audioSource_Footstep.Play();
    }

    public void PlayHit()
    {
        audioSource_Hit.Play();
    }
}
