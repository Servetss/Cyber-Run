using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem SpeedParticle;

    // Start is called before the first frame update
    void Start()
    {
        SpeedParticle.Pause(true);
    }

    public void SpeedParticleActivate(bool Activate)
    {
        if(Activate)
            SpeedParticle.Play(true);
        else
            SpeedParticle.Pause(true);
    }
}
