using UnityEngine;

public class FootIK : MonoBehaviour
{
    Vector3 rightFootPosition, leftFootPosition, rightFootIkPosition, leftFootIkPosition;
    Quaternion rightFootIkRotation, leftFootIkRotation;
    float lastPalvisPositionY, lastRightFootIkPositionY, lastLeftFootPositionY;

    [Header("Feet Grounder")]
    public bool enableFeetIk = true;
    [Range(0, 2)] [SerializeField] private float heightFromGroundRaycast = 1.14f;
    [Range(0, 2)] [SerializeField] private float raycastDownDistance =1.5f;
    [SerializeField] LayerMask environmentLayer;
    [SerializeField] float pelvisOffset = 0f;
    [Range(0, 1)] [SerializeField] float pelvisUpAndDownSpeed = 0.28f;
    [Range(0, 1)] [SerializeField] float feetToIkPositionSpeed = 0.5f;

    public string leftFootAnimVariableName = "LeftFootCurve";
    public string rightFootAnimVariableName = "RightFootCurve";

    public bool useProIkFeature = false;
    public bool showSolveDebug = true;

    Animator anim;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        
    }

    private void Start()
    {
        //Debug.Log(anim.GetBoneTransform(HumanBodyBones.LeftFoot).position);

    }

    #region FeetGrounding



    private void FixedUpdate()
    {
        Debug.Log(anim.GetBoneTransform(HumanBodyBones.LeftFoot).position);



        if (enableFeetIk == false) return;
        if (anim == null) return;

        AdjustFeetTarget(ref rightFootIkPosition, HumanBodyBones.RightFoot);
        AdjustFeetTarget(ref leftFootIkPosition, HumanBodyBones.LeftFoot);

        FeetPositionSolver(rightFootPosition, ref rightFootIkPosition, ref rightFootIkRotation);
        FeetPositionSolver(leftFootPosition, ref leftFootIkPosition, ref leftFootIkRotation);


    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (enableFeetIk == false) return;
        if (anim == null) return;
        MovePelvisHeight();

        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        if(useProIkFeature)
        {
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat(rightFootAnimVariableName));
        }

        MoveFeetToIkPoint(AvatarIKGoal.RightFoot, rightFootPosition, rightFootIkRotation, ref lastRightFootIkPositionY);


        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        if (useProIkFeature)
        {
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat(leftFootAnimVariableName));
        }

        MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, leftFootPosition, leftFootIkRotation, ref lastLeftFootPositionY);

    }

    #endregion

    #region FeetGroundingMethods

    void MoveFeetToIkPoint (AvatarIKGoal foot, Vector3 positionIkHolder, Quaternion rotationIkHolder, ref float lastFootPositionY )
    {
        Vector3 targetIkPosition = anim.GetIKPosition(foot);
        if(positionIkHolder != Vector3.zero )
        {
            
            targetIkPosition = transform.InverseTransformPoint(targetIkPosition);
            positionIkHolder = transform.InverseTransformPoint(positionIkHolder);

            float yVariable = Mathf.Lerp(lastFootPositionY, positionIkHolder.y, feetToIkPositionSpeed);
            targetIkPosition.y += yVariable;
            lastFootPositionY = yVariable;
            targetIkPosition = transform.TransformPoint(targetIkPosition);
            anim.SetIKRotation(foot, rotationIkHolder);
        }
        anim.SetIKPosition(foot, targetIkPosition);

    }

    void MovePelvisHeight()
    {
        if (rightFootIkPosition == Vector3.zero || leftFootIkPosition == Vector3.zero || lastLeftFootPositionY == 0)
        {
            lastLeftFootPositionY = anim.bodyPosition.y;
            return;
        }
        float lOffestPosition = leftFootIkPosition.y - transform.position.y;
        float rOffsetPostion = rightFootIkPosition.y - transform.position.y;

        float totalOffset = (lOffestPosition < rOffsetPostion) ? lOffestPosition : rOffsetPostion;

        Vector3 newPelvisPosition = anim.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(lastLeftFootPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

        anim.bodyPosition = newPelvisPosition;
        lastPalvisPositionY = anim.bodyPosition.y;

    }

    void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIkPositions, ref Quaternion feetIkRotations)
    {
        RaycastHit feetOutHit;

        if (showSolveDebug)
            Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);
        Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.red);
        //Debug.Log("fromSkyPosition" + fromSkyPosition + "feetIkPositions" + feetIkPositions);

        if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
        {
            feetIkPositions = fromSkyPosition;
            feetIkPositions.y = feetOutHit.point.y + pelvisOffset;
            feetIkRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;
            Debug.Log("feetIkRotations" + feetIkRotations);
            return;
        }
        feetIkPositions = Vector3.zero;
    }

    void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
    {
        
        feetPositions = anim.GetBoneTransform(foot).position;
        feetPositions.y = transform.position.y + heightFromGroundRaycast;

    }


    #endregion


    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(rightFootPosition, 2f);
    }
}
