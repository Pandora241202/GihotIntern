using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{
    Dictionary<int, GameObject> effectDict = new Dictionary<int, GameObject>();

    public EffectManager() { }

    public void AddEffect(GameObject effect)
    {
        effectDict.Add(effect.GetInstanceID(), effect);
    }

    public void RemoveEffectById(int id)
    {
        effectDict.Remove(id);
    }

    private void PauseAll()
    {
        foreach (GameObject effect in effectDict.Values)
        {
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps)
            {
                ps.Pause();
            }
        }
    }

    private void PlayAll()
    {
        foreach (GameObject effect in effectDict.Values)
        {
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps)
            {
                if (ps.isPlaying) return;
                ps.Play();
            }
        }
    }

    public void UpdateEffectsState()
    {
        if (AllManager.Instance().isPause)
        {
           PauseAll();
        } 
        else
        {
            PlayAll();
        }
    }
}
