public class HitEffectPool : ObjectPoolBase<HitEffectObject>
{
    public static HitEffectPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}