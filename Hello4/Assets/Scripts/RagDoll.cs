using UnityEngine;

public class RagDoll : MonoBehaviour
{
    public GameObject mainColliderGo;
    public GameObject ragdollRootGo;

    Animator animator;
    Collider mainCollider;
    Collider[] allCollider;
    Rigidbody[] rigidbodies;

    bool isRagdoll = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mainCollider = mainColliderGo.GetComponent<Collider>();
        allCollider = ragdollRootGo.GetComponentsInChildren<Collider>(true);
        rigidbodies = ragdollRootGo.GetComponentsInChildren<Rigidbody>(true);
    }

    private void OnEnable()
    {
        //DoRagdoll(isRagdoll);
    }
    // Start is called before the first frame update
    void Start()
    {
        //animator.keepAnimatorControllerStateOnDisable = true;
        
    }

    // Update is called once per frame
    public void DoRagdoll(bool isRagdoll)
    {
        //Debug.Log("DoRagdoll");
        foreach (var col in allCollider)
        {
            col.enabled = isRagdoll;
        }

        foreach (var rb in rigidbodies)
        {
            //Debug.Log(rb.name);
            rb.useGravity = isRagdoll;
            rb.isKinematic = !isRagdoll;
        }

        mainCollider.enabled = !isRagdoll;
        //GetComponent<Rigidbody>().useGravity = !isRagdoll;     
        animator.enabled = !isRagdoll;
    }


}
