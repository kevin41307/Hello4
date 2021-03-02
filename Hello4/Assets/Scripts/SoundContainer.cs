using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundContainer : MonoBehaviour
{
    public AudioSource playerAudioSource;
    public List<AudioClip> footsteps;
    public List<AudioClip> swordSwing;
    public List<AudioClip> swordHit;
    public List<AudioClip> rolling;
    public List<AudioClip> gunShoot;
    public List<AudioClip> bloodExplosion_More;
    public List<AudioClip> bloodExplosion_Less;
    public AudioClip postEffect;
    public AudioClip void_low1;
    public AudioClip void_high1;
    public AudioClip bassDrop1_1;
    public AudioClip bassDrop1_2;

    public Database_AudioCollection playerAudioCollection;

    public AudioEvent m_GuardAudioEvent1;

    AudioSourcePool audioSourcePool;


    private void Awake()
    {
        playerAudioSource = this.GetComponent<AudioSource>();
        audioSourcePool = this.GetComponent<AudioSourcePool>();
    }

    private void Start()
    {

    }


    public void PlaySound(AudioClip sound)
    {
        playerAudioSource.PlayOneShot(sound);
    }


    public void PlayGuardSound()
    {
        m_GuardAudioEvent1.Play(audioSourcePool.GetAudioSource());
    }



    public void PlayFootsteps()
    {
        //playerAudioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Count - 1)], 0.6f);
        audioSourcePool.Play(playerAudioCollection.footsteps[Random.Range(0, playerAudioCollection.footsteps.Count - 1)], 0.6f);

    }
    public void PlaySwordSwingSound()
    {
        //playerAudioSource.PlayOneShot(swordSwing[Random.Range(0, swordSwing.Count - 1)], 0.6f);
        audioSourcePool.Play(playerAudioCollection.swordSwing[Random.Range(0, playerAudioCollection.swordSwing.Count - 1)], 0.6f);
    }

    public void PlaySwordHitSound()
    {
        playerAudioSource.PlayOneShot(swordHit[Random.Range(0, swordHit.Count - 1)]);
    }



    public void PlayRollingSound()
    {
        playerAudioSource.PlayOneShot(rolling[Random.Range(0, rolling.Count - 1)]);
    }

    public void PlayGunShootSound()
    {
        playerAudioSource.PlayOneShot(gunShoot[Random.Range(0, gunShoot.Count - 1)]);
    }
    public void PlayBloodExplosionMoreSound()
    {
        playerAudioSource.PlayOneShot(bloodExplosion_More[Random.Range(0, bloodExplosion_More.Count - 1)], 1.2f);
    }

    public void PlayBloodExplosionLessSound()
    {
        playerAudioSource.PlayOneShot(bloodExplosion_Less[Random.Range(0, bloodExplosion_Less.Count - 1)]);
    }

    public void PlayPostEffectSound()
    {
        playerAudioSource.PlayOneShot(postEffect);
    }

    public void PlayVoidLow1Sound()
    {
        playerAudioSource.PlayOneShot(void_low1);
    }
    public void PlayVoidHigh1Sound()
    {
        playerAudioSource.PlayOneShot(void_high1);
    }

    public void PlayBassDrop1_1Sound()
    {
        playerAudioSource.PlayOneShot(bassDrop1_1);
    }

    public void PlayBassDrop1_2Sound()
    {
        playerAudioSource.PlayOneShot(bassDrop1_2);
    }


}
