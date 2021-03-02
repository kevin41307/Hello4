using UnityEngine;


public class SpawnFracture : MonoBehaviour
{
    public GameObject fracturedObject_prefab;
    GameObject fracObf;
    // Start is called before the first frame update

    private void Start()
    {
        fracObf = Instantiate(fracturedObject_prefab, transform.position, Quaternion.identity) as GameObject;
        fracObf.transform.localScale = this.transform.localScale;
        fracObf.SetActive(false);
    }


    public void SpawnFractureObject()
    {
        fracObf.SetActive(true);
        fracObf.GetComponent<ExplodeTut>().Explode();
        Destroy(this.gameObject);
    }
}
