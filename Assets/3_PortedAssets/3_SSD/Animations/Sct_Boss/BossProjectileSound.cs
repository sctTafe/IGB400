using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileSound : MonoBehaviour
{
    AudioSource bossProjectileSound;
    public AudioClip bossProjectileLaunch;
    public AudioClip bossProjectileHit;

    private void Awake()
    {
        bossProjectileSound = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //bossProjectileSound.PlayOneShot(bossProjectileLaunch);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        bossProjectileSound.PlayOneShot(bossProjectileHit);
    }
}
