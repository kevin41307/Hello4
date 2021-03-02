using UnityEngine;
public class AudioSourcePool : MonoBehaviour
{

    AudioSource[] audioSources;
    AudioSource source;
    public int poolDepth = 5;
    [HideInInspector]
    [Range(0, 1f)]
    public float maxVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {
        SetupPool();
    }

    public void SetupPool()
    {
        audioSources = new AudioSource[poolDepth];

        for (int i = 0; i < poolDepth; i++)
        {
            audioSources[i] = this.gameObject.AddComponent<AudioSource>();
        }
    }

    public void SetupPool(int _poolDepth)
    {
        poolDepth = _poolDepth;
        SetupPool();
    }

    public AudioSource GetAudioSource()
    {
        source = null;
        for (int i = 0; i < poolDepth; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                source = audioSources[i];
                break;
            }
        }

        if(source == null )
        {
            source = audioSources[poolDepth-1]; // 先降
        }
        return source;

    }

    public void Play(AudioClip audioClip)
    {
        source = GetAudioSource();

        source.clip = audioClip;
        source.Play();
    }

    public void Play(AudioClip audioClip, float volume)
    {
        source = GetAudioSource();

        source.clip = audioClip;
        source.volume = volume * maxVolume;
        source.Play();
    }
    public void Play(AudioClip audioClip, RangedFloat volume)
    {
        source = GetAudioSource();

        source.clip = audioClip;
        source.volume = Random.Range(volume.minValue, volume.maxValue) * maxVolume;
        source.Play();
    }

    public void Play(SimpleAudioEvent audioEvent)
    {
        audioEvent.Play(GetAudioSource());
    }

    public void Play(SimpleAudioEvent audioEvent, float maxVolume)
    {
        audioEvent.Play(GetAudioSource(), maxVolume);
    }

}
