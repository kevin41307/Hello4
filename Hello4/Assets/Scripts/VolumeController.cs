using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using System.Collections;

public class VolumeController : MonoBehaviour
{
    private Bloom bloom = null;
    private MotionBlur motionBlur = null;
    const float motionBlurTime = 1f;
    bool isMotionBluring = false;

    public float chromaticAberrationIntensity;

    float vignetteIntensity = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Volume volume = GetComponent<Volume>();

        volume.sharedProfile.TryGet<Bloom>(out bloom);
        volume.sharedProfile.TryGet<MotionBlur>(out motionBlur);

        //motionBlur.active = false;

        //StartCoroutine(StartMotionBlurIn2ms());

        //vignette.intensity.SetValue(new NoInterpMinFloatParameter(vignetteIntensity, 0, true));
        //vignette.intensity.value = 1f;

        //volume.sharedProfile.TryGet<ChromaticAberration>(out chromaticAberration);
    }

    public IEnumerator StartMotionBlurIn2ms()
    {
        motionBlur.active = true;
        Game.playerAttrSingle.animator.SendMessage("GenerateImpulseSmall", SendMessageOptions.DontRequireReceiver);
        yield return new WaitForSeconds(motionBlurTime);
        motionBlur.active = false;
    }

    private void Update()
    {
        //Debug.Log(motionBlur.active);
        if(motionBlur.active)
        {
            Debug.Log(Time.time);
        }

    }
}
