using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class EffectObjectBase : MonoBehaviour
{
    ParticleSystem vfx;

    private void Awake()
    {
        if (!vfx)
            vfx = GetComponent<ParticleSystem>();
    }

    public virtual void Play()
    {
        gameObject.SetActive(true);
        vfx.Play();
    }

    protected abstract void OnDisable();
}
