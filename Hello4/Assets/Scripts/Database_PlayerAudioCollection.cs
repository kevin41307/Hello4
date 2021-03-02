using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/AudioCollections/PlayerAudioCollection", fileName = "PlayerAudioCollection")]
[System.Serializable]
public class Database_PlayerAudioCollection : ScriptableObject
{
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
    public AudioEvent m_GuardAudioEvent1;
}
