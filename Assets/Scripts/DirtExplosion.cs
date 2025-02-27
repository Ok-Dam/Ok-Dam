using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtExplosion : MonoBehaviour
{
    public ParticleSystem dirtParticles; // 흙 튀는 입자 시스템

    void Start()
    {
        // 파헤쳐진 곳에서 흙이 튀는 효과 발생
        PlayDirtParticles(transform.position);
    }

    void PlayDirtParticles(Vector3 position)
    {
        dirtParticles.transform.position = position;
        dirtParticles.Play();
    }
}
