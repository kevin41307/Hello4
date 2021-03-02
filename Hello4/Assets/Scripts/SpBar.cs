using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpBar : MonoBehaviour
{
    Slider spPoint;
    public Slider fadeOut;
    PlayerAttribute pla = Game.playerAttrSingle;
    bool preDescending = false;
    bool descending = false;
    bool resuming = false;
    float descendingTimer ;
    float autoRecoveryTimer;
    float resumingTimer;
    float autoRecoverySpeed;
    const float descendingTime = 1f;
    const float autoRecoveryTime = 0.25f;
    const float resumingTime = 0.8f;
    private void Awake()
    {
        spPoint = GetComponent<Slider>();
        fadeOut = fadeOut.GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        spPoint.value = pla.currentSp / pla.maxSp;
        fadeOut.value = spPoint.value;
        autoRecoverySpeed = pla.autoRecoverySpeed;
    }
    public void Print()
    {
        //Debug.Log("TestSingle");
    }
    public void ChangeBarValue(ChangeInfo info)
    {
        float deltaInPercent = info.delta / pla.maxSp;
        pla.currentSp = Mathf.Clamp(pla.currentSp + info.delta, 0, pla.maxSp);
        spPoint.value = pla.currentSp / pla.maxSp;

        if (Mathf.Sign(info.delta) == -1)  //&& !descending && !resuming
        {          

            if (!preDescending)
            {
                //Debug.Log("preDesding");
                ResetDescendingTimer();
                StartCoroutine(StartDescendingBuff());
            }
            RestResumingTimer();
        }
        if (Mathf.Sign(info.delta) == 1)
        {
            //spPoint.value = pla.currentSp / pla.maxSp;
            fadeOut.value = spPoint.value;
        }

    }

    void ResetAutoRecoveryTimer()
    {
        autoRecoveryTimer = autoRecoveryTime;
    }
    void ResetDescendingTimer()
    {
        descendingTimer = descendingTime;
    }

    void RestResumingTimer()
    {
        resumingTimer = resumingTime;
    }    


    private void Update()
    {
        /*
        if (descendingTimer > -1f)
            descendingTimer -= Time.deltaTime;
        */
        if (resumingTimer > -1f)
            resumingTimer -= Time.deltaTime;
        /*
        if (resumingTimer <= 0f )
        {
            if( descendingTimer > resumingTimer && fadeOut.value - spPoint.value > 0.01f && !resuming)
            {
                //Debug.Log("resume");
                StopCoroutine(StartDescendingBuff());
                preDescending = false;
                StartCoroutine(StartResuming());
            }
        }
        
        //Debug.Log(descendingTimer + "," + resumingTimer);
        
        if ( descendingTimer <= 0f )
        {
            if (fadeOut.value - spPoint.value > 0.01f && !descending)
            {
                Debug.Log("descend");
                StartCoroutine(StartDescending());
            }
        }
        */
        bool running = Input.GetKey(KeyCode.LeftShift);
        if (running)
        {
            ResetAutoRecoveryTimer();
        }

        if (Mathf.Abs(fadeOut.value - spPoint.value) < 0.01f)
            autoRecoveryTimer -= Time.deltaTime;
        else
            ResetAutoRecoveryTimer();

        if (  autoRecoveryTimer < 0f )
        {
            ChangeBarValue(new ChangeInfo(autoRecoverySpeed * Time.deltaTime));
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeBarValue(new ChangeInfo(-30f, true));
        }


        
    }



    IEnumerator StartDescendingBuff()
    {
        preDescending = true;
        yield return new WaitForSeconds(descendingTimer);
        float t = 0f;
        while (Mathf.Abs(fadeOut.value - spPoint.value) >= 0.001f)
        {
            t += 0.75f * Time.deltaTime;
            fadeOut.value = Mathf.Lerp(fadeOut.value, spPoint.value, t);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        spPoint.value = fadeOut.value;
        preDescending = false;
    }

    IEnumerator StartDescending()
    {
        descending = true;
        float t = 0f;
        while (Mathf.Abs(fadeOut.value - spPoint.value) >= 0.001f)
        {
            t += 0.75f * Time.deltaTime;
            fadeOut.value = Mathf.Lerp(fadeOut.value, spPoint.value, t);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        spPoint.value = fadeOut.value;
        descending = false;
    }

    IEnumerator StartResuming()
    {
        resuming = true;
        float t = 0f;
        while (Mathf.Abs(fadeOut.value - spPoint.value) >= 0.001f)
        {
            t += 0.75f * Time.deltaTime;

            
            spPoint.value = Mathf.Lerp(spPoint.value, fadeOut.value, t);
            pla.currentSp = spPoint.value * pla.maxSp;
            
            /*
            ChangeInfo info = new ChangeInfo();
            info.delta = Mathf.Lerp(spPoint.value * pla.maxSp, fadeOut.value * pla.maxSp, t) - pla.currentSp;          
            ChangeBarValue(info);
            */
            yield return new WaitForSeconds(Time.deltaTime);

        }
        fadeOut.value = spPoint.value;
        resuming = false;
    }

}
