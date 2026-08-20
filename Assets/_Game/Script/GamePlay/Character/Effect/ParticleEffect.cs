using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : GameUnit
{
    [SerializeField] protected ParticleSystem mainParticle;
    [SerializeField] protected List<ParticleSystem> childParticle;

    public void Play(ColorType colorType)
    {
        TF.localScale = Vector3.one;
        mainParticle.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = mainParticle.main;
        main.startColor = GetEffectColor(colorType);
        foreach(var p in childParticle)
        {
            main = p.main;
            main.startColor = GetEffectColor(colorType);
        }
        mainParticle.Play();
        Invoke(nameof(DeSpawnVFX), 1f);
    }
    public void DeSpawnVFX()
    {
        SimplePool.DeSpawn(this);
    }
    //TODO sửa list<enum>
    private Color GetEffectColor(ColorType type)
    {
        return DataManager.Instance.GetMaterial(type).color;
    }
}