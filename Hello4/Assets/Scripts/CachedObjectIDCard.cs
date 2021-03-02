using UnityEngine;

public class CachedObjectIDCard : MonoBehaviour
{
    [HideInInspector]
    public CacheObjectController.ObjType objType;
    [HideInInspector]
    public int ID = 0;
    [HideInInspector]
    public CachedObject cachedObject;

    public CachedObjectIDCard(CacheObjectController.ObjType _objType, int _ID, CachedObject _cachedObject)
    {
        objType = _objType;
        ID = _ID;
        cachedObject = _cachedObject;
    }


    public void GiveBackCachedObject()
    {
        cachedObject.DestroyAsset();
    }
}
