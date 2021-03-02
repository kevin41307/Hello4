
using UnityEngine;
[RequireComponent(typeof( Cinemachine.CinemachineImpulseSource))]
public class ShakeCinemachine : MonoBehaviour
{

    Cinemachine.CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.O))
        {
            GenerateImpulseBig();
        }
        */
    }
    public void GenerateImpulseBig()
    {
        float temp = impulseSource.m_ImpulseDefinition.m_AmplitudeGain;
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = 7f;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 0.2f;
        impulseSource.GenerateImpulse();
        //impulseSource.m_ImpulseDefinition.m_AmplitudeGain = temp;
    }

    public void GenerateImpulseNormal()
    {
        float temp = impulseSource.m_ImpulseDefinition.m_AmplitudeGain;
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = 4f;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 0.15f;
        impulseSource.GenerateImpulse();
        //impulseSource.m_ImpulseDefinition.m_AmplitudeGain = temp;
    }

    public void GenerateImpulseSmall()
    {
        float temp = impulseSource.m_ImpulseDefinition.m_AmplitudeGain;
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = 1.2f;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 0.1f;
        impulseSource.GenerateImpulse();
        //impulseSource.m_ImpulseDefinition.m_AmplitudeGain = temp;

    }

}
