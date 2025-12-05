using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class NoteEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject _PerfectParticle;
    [SerializeField]
    private GameObject _GreatParticle;

    private const float _WaitTime = 1.5f;

    public void PerfectEffect()
    {
        GameObject particle = Instantiate(_PerfectParticle, transform);
        particle.GetComponent<ParticleSystem>().Play();
    }

    public void GreatEffect()
    {
        GameObject particle = Instantiate(_GreatParticle, transform);
        particle.GetComponent<ParticleSystem>().Play();
    }
}
