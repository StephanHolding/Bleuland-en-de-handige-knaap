using System.Collections;
using UnityEngine;

public class ParticleEffectHandler : SingletonTemplateMono<ParticleEffectHandler>
{

    public ParticleSystem[] particleSystemPrefabs;

    public void PlayParticle(string particleName, Vector3 worldPosition, Quaternion rotation)
    {
        PlayParticle(FindParticleSystemByName(particleName), worldPosition, rotation);
    }

    public void PlayParticle(ParticleSystem particleSystem, Vector3 worldPositon, Quaternion rotation)
    {
        ParticleSystem systemInScene = Instantiate(particleSystem.gameObject, worldPositon, rotation).GetComponent<ParticleSystem>();
        systemInScene.Play();
        StartCoroutine(KillParticleAfterLifespan(systemInScene));
    }

    private ParticleSystem FindParticleSystemByName(string particleName)
    {
        foreach (var particle in particleSystemPrefabs)
        {
            if (particle.name == particleName)
            {
                return particle;
            }
        }
        return null;
    }

    private IEnumerator KillParticleAfterLifespan(ParticleSystem toKill)
    {
        yield return new WaitForSeconds(toKill.main.duration);
        Destroy(toKill.gameObject);
    }

}
