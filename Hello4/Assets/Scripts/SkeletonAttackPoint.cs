using UnityEngine;

public class SkeletonAttackPoint : EnemyAttackPoint
{

    Animator animator;
    SkeletonAttribute skeletonAttribute;
    float attackFieldOfView = 160f;
    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        skeletonAttribute = GetComponentInParent<SkeletonAttribute>();
    }

    private void Start()
    {
        sphereDetection = true;
        sphereRadius = 1.9f;
    }

    void OnDrawGizmos()
    {

        //Gizmos.color = new Color(0, 1, 0, 1f);
        //Gizmos.DrawSphere(transform.position, radius);
        //Gizmos.DrawLine(this.transform.position - this.transform.up * 1f, this.transform.position + this.transform.up * 0.7f);
        //Gizmos.DrawLine(this.transform.position + this.transform.right * 0.2f, this.transform.position - this.transform.right * 0.2f);
        Gizmos.color = new Color(0, 1, 0, 0.5f);

        Gizmos.DrawSphere(transform.position, sphereRadius);

        //Gizmos.DrawSphere(transform.position, sphereRadius);
    }

    public override void HandleCollision(Collider[] hit)
    {
        base.HandleCollision(hit);
        Vector3 direction = hit[0].transform.position - this.transform.position;
        float degree = Vector3.Angle(direction, this.transform.forward);
        if (degree < attackFieldOfView / 2 && degree > -attackFieldOfView / 2)
        {

            ChangeInfo info = new ChangeInfo(-skeletonAttribute.attackDamage * animator.GetFloat("motionRate"));
            info.murderer = animator.transform;
            hit[0].SendMessage("TakeDamage", info);
        }
    }


}
