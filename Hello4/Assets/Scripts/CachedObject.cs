using System.Collections;
using UnityEngine;

public class CachedObject : MonoBehaviour
{
    public bool inUse { set; get; }
    [HideInInspector]
    public float rentalPeriod;
    [HideInInspector]
    public CachedObjectIDCard cachedObjectIDCard;

    [HideInInspector]
    public GameObject _asset = null;
    ParticleSystem[] _ps;

    public static CachedObject Create(string assetname, GameObject prefab)
    {
        GameObject go = new GameObject();
        go.name = "cache_" + assetname;
        CachedObject co = go.AddComponent<CachedObject>();
        co.Init(assetname, prefab);
        return co;
    }
    public static CachedObject Create(string assetname, GameObject prefab, CacheObjectController.ObjType _objType, int _ID)
    {
        GameObject go = new GameObject();
        go.name = "cache_" + assetname;
        CachedObject co = go.AddComponent<CachedObject>();
        co.Init(assetname, prefab, _objType, _ID);
        return co;
    }


    void Init(string assetname, GameObject prefab)
    {
        inUse = false;
        Vector3 pos = new Vector3(10000, 10000, 10000);
        _asset = (GameObject)GameObject.Instantiate(prefab, pos, Quaternion.identity);
        //cachedObjectIDCard = _asset.AddComponent<CachedObjectIDCard>();
        _ps = _asset.GetComponentsInChildren<ParticleSystem>();
        _asset.SetActive(false);
    }

    void Init(string assetname, GameObject prefab, CacheObjectController.ObjType _objType, int _ID)
    {
        inUse = false;
        Vector3 pos = new Vector3(10000, 10000, 10000);
        _asset = (GameObject)GameObject.Instantiate(prefab, pos, Quaternion.identity);
        cachedObjectIDCard = _asset.AddComponent<CachedObjectIDCard>();
        cachedObjectIDCard.cachedObject = this;
        cachedObjectIDCard.ID = _ID;
        cachedObjectIDCard.objType = _objType;
        _ps = _asset.GetComponentsInChildren<ParticleSystem>();
        _asset.SetActive(false);
    }
    public GameObject CreateObject(Vector3 pos, Vector3 euler)
    {
        if(_asset == null )
        {
            return null;
        }
        inUse = true;
        _asset.transform.position = pos;
        _asset.transform.eulerAngles = euler;
        _asset.SetActive(true);
        foreach(ParticleSystem ps in _ps)
        {
            ps.Play(true);
        }
        return _asset;
    }
    public GameObject CreateObject(Vector3 pos, Vector3 euler, float _rentalPeriod)
    {
        if (_asset == null)
        {
            return null;
        }
        inUse = true;
        _asset.transform.position = pos;
        _asset.transform.eulerAngles = euler;
        rentalPeriod = _rentalPeriod;
        _asset.SetActive(true);
        foreach (ParticleSystem ps in _ps)
        {
            ps.Play(true);
        }
        StartCoroutine(StartAutoGiveBackAsset());
        return _asset;
    }

    public void MyParent(Transform parent)
    {
        _asset.transform.SetParent(parent);
    }
    public GameObject DestroyAsset()
    {
        if(_asset == null )
        {
            return null;
        }
        inUse = false;
        _asset.transform.position = new Vector3(10000, 10000, 10000);
        _asset.transform.eulerAngles = Vector3.zero;
        MyParent(null);
        _asset.SetActive(false);
        foreach( ParticleSystem ps in _ps)
        {
            ps.Stop(true);
        }
        return _asset;
    }


    IEnumerator StartAutoGiveBackAsset()
    {
        yield return new WaitForSeconds(rentalPeriod);
        
        DestroyAsset();
    }
}
