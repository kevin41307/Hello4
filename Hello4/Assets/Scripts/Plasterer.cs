using UnityEngine;

public class Plasterer : MonoBehaviour
{
    public GameObject[] carpet;
    CacheObjectController CoController;
    private void Awake()
    {
        CoController = Game.cacheObjectControllerSingle;
    }

    // Start is called before the first frame update
    public void Construction(Vector3 place)
    {
        CoController.BorrowAsset(CacheObjectController.ObjType.blood_carpet, place, Quaternion.identity.eulerAngles);
    }
}
