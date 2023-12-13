using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustController : MonoBehaviour
{
    ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void StartDust()
    {
        particles.Play();
    }
    
    public void StopDust()
    {
        particles.Stop();
    }
}
