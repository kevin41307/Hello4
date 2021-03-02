using UnityEngine;

public class AttackToEnv : MonoBehaviour
{
    Vector3 swordPoint;
    float radius = 0.02f;
    float length = 1.65f;
    RaycastHit hitInfo;
    CacheObjectController CoController;
    const float pauseTime = 0.5f;
    float pauseTimer = 0f;
    private void Awake()
    {
        CoController = Game.cacheObjectControllerSingle;
    }


    private void Start()
    {
        swordPoint = this.transform.position - this.transform.forward * 1f;
        pauseTimer = 0f;
    }

    private void LateUpdate()
    {
        swordPoint = this.transform.position - this.transform.forward * 1f;
        pauseTimer -= Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DetectCollision();

    }

    void DetectCollision()
    {
        if (Physics.SphereCast(swordPoint, radius, this.transform.forward, out hitInfo, length, Game.Singleton.dafultLayer))
        {
            if( pauseTimer <= 0 )
            {
                GenerateCollisionVFX();
                pauseTimer = pauseTime;
            }
            
        }
    }


    void GenerateCollisionVFX()
    {
        CoController.BorrowAsset(CacheObjectController.ObjType.hitSparks, hitInfo.point, Quaternion.identity.eulerAngles);
    }
}
