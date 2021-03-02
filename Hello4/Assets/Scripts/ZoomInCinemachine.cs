using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInCinemachine : MonoBehaviour
{
    public Cinemachine.CinemachineFreeLook freeLookCamera;
    public Cinemachine.CinemachineFreeLook lockOnCamera;
    Cinemachine.CinemachineFreeLook currentCamera;

    bool isZoomIn = false;
    bool isZoomOut = false;
    const float defaultFOV = 45;
    const float zoomInFOV = 25;


    void ChooseCinemachine()
    {
        bool isLockOn = (Game.Singleton.nearbyEnemy != null) ? true : false;
        currentCamera = (isLockOn) ? lockOnCamera : freeLookCamera;
    }

    IEnumerator StartZoomIn()
    {
        ChooseCinemachine();
        currentCamera.m_Lens.FieldOfView = defaultFOV;
        while (Mathf.Abs(currentCamera.m_Lens.FieldOfView - zoomInFOV) > 0.1f)
        {
            currentCamera.m_Lens.FieldOfView = Mathf.Lerp(currentCamera.m_Lens.FieldOfView, zoomInFOV, 50f * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        currentCamera.m_Lens.FieldOfView = zoomInFOV;

    }

    IEnumerator StartZoomOut()
    {
        ChooseCinemachine();
        currentCamera.m_Lens.FieldOfView = zoomInFOV;
        while (Mathf.Abs(currentCamera.m_Lens.FieldOfView - defaultFOV) > 0.1f)
        {
            currentCamera.m_Lens.FieldOfView = Mathf.Lerp(currentCamera.m_Lens.FieldOfView, defaultFOV, 0.75f * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        currentCamera.m_Lens.FieldOfView = defaultFOV;

    }

    IEnumerator StartZoomInAndZoomOut()
    {
        ChooseCinemachine();
        currentCamera.m_Lens.FieldOfView = defaultFOV;
        while (Mathf.Abs(currentCamera.m_Lens.FieldOfView - zoomInFOV) > 0.1f)
        {
            currentCamera.m_Lens.FieldOfView = Mathf.Lerp(currentCamera.m_Lens.FieldOfView, zoomInFOV, 40f * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(1f);

        currentCamera.m_Lens.FieldOfView = zoomInFOV;
        while (Mathf.Abs(currentCamera.m_Lens.FieldOfView - defaultFOV) > 0.1f)
        {
            currentCamera.m_Lens.FieldOfView = Mathf.Lerp(currentCamera.m_Lens.FieldOfView, defaultFOV, 0.75f * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        currentCamera.m_Lens.FieldOfView = defaultFOV;
    }
}
