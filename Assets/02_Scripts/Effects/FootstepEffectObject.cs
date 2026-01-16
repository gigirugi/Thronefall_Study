using UnityEngine;

public class FootstepEffectObject : EffectObjectBase
{
    public override void Play()
    {
        base.Play();
        AudioManager.Instance.PlayFootstep();
    }

    protected override void OnDisable()
    {
        FootstepEffectPool.Instance.ReturnPooledObject(this);
    }
}
