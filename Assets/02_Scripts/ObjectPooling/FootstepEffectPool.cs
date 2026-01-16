public class FootstepEffectPool : ObjectPoolBase<FootstepEffectObject>
{
    public static FootstepEffectPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}