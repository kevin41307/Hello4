
using System.Collections;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    public float time = 3f;
    // Start is called before the first frame update

    void Start()
    {
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject, time);
        StartCoroutine(StartDisable());
    }

    IEnumerator StartDisable()
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }

}
