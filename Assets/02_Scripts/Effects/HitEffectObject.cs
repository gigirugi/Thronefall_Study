public class HitEffectObject : EffectObjectBase
{
    // 타격 효과 재생
    public override void Play()
    {
        base.Play();
        AudioManager.Instance.PlayHit();
    }

    protected override void OnDisable()
    {
        HitEffectPool.Instance.ReturnPooledObject(this);
    }
}
