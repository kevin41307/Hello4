using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MpBar : MonoBehaviour
{
    Slider manaPoint;
    public Slider fadeOut;
    PlayerAttribute pla = Game.playerAttrSingle;
    float delayDescendingTimer = 3.5f;
    bool descending = false;


    private void Awake()
    {
        manaPoint = GetComponent<Slider>();
        fadeOut = fadeOut.GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        manaPoint.value = pla.currentMp / pla.maxMp;
        fadeOut.value = manaPoint.value;
    }


    // Start is called before the first frame update
    public void ChangeBarValue(ChangeInfo info)
    {
        pla.currentMp = pla.currentMp + info.delta;
        manaPoint.value = pla.currentMp / pla.maxMp;
        if( !descending )
            StartCoroutine( StartDescending());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            //接受傷害
            ChangeBarValue(new ChangeInfo(-10f, true));
        }
    }
    IEnumerator StartDescending()
    {
        descending = true;
        yield return new WaitForSeconds(delayDescendingTimer);
        float t = 0f;
        while (Mathf.Abs(fadeOut.value - manaPoint.value) >= 0.001f)
        {
            t += 0.5f * Time.deltaTime;
            fadeOut.value = Mathf.Lerp(fadeOut.value, manaPoint.value, t);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        descending = false;
    }
}
