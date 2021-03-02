using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    public LayerMask groundLayer = default;
    public IKFootSolver otherFoot = default;
    public Transform body;
    public float modelScale = default;

    public Vector3 footOffset = default;
   
    public float stepDistance;
    public float stepLength;
    public float stepHeight;

    public float speed = 1f;
    RaycastHit hitinfo;
    RaycastHit hitinfo2;


    float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;

    float lerp;

    public Transform temp;
    private void Start()
    {
        footSpacing = transform.localPosition.x * modelScale;
        footOffset = footOffset * modelScale;

        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = temp.up;

        //transform.up;
        //transform.right = Quaternion.Euler(0, 0, 90) * transform.right;
        //transform.right = Quaternion.Euler(90, 0, 0) * transform.right;
        //transform.right = Quaternion.Euler(0, 180, 0) * transform.right;
        //transform.right = Quaternion.Euler(0, 0, 90) * transform.right;

        //transform.rotation = Quaternion.LookRotation(Vector3.right, transform.up) ;
        //transform.rotation = Quaternion.LookRotation(Vector3.right, transform.up);

        //transform.up = transform.right;
        
        lerp = 1;

    }

    private void Update()
    {
        //transform.up = temp.rotation.eulerAngles;
        /*
        Ray ray = new Ray(body.position + body.right * footSpacing, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, 5, groundLayer.value))
        {
            hitinfo = info;
            temp.up = info.normal;
            transform.up = temp.eulerAngles;
        }

        Ray ray2 = new Ray(body.position + body.right * footSpacing + body.forward * stepDistance, Vector3.down);
        if (Physics.Raycast(ray2, out RaycastHit info2, 5, groundLayer.value))
        {
            hitinfo2 = info2;
        }

        if (info.point.y > info2.point.y)
        {
            Debug.Log("slump down");
        }
        else if (info.point.y == info2.point.y)
        {
            Debug.Log("flat");
        }
        else
        {
            Debug.Log("slump up");
        }
        */

        /*
        Ray ray = new Ray(body.position + body.right * footSpacing, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, 5, groundLayer.value))
        {
            hitinfo = info;
            temp.up = info.normal;
            transform.up = temp.eulerAngles;
        }
        */
        
        temp.position = currentPosition;
        temp.up = currentNormal;

        Ray ray = new Ray(body.position + body.right * footSpacing, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit info, 5, groundLayer.value))
        {
            hitinfo = info;
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            if( Vector3.Distance(currentPosition, info.point) > stepDistance && lerp >= 1 && !otherFoot.IsMoving()) 
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                Vector3 raise = Vector3.zero;
                Ray slumpRay = new Ray(body.position + direction * body.right * footSpacing + body.forward * stepDistance, Vector3.down);
                if (Physics.Raycast(slumpRay, out RaycastHit info2, 5, groundLayer.value))
                {
                    hitinfo2 = info2;
                    raise.y = info.point.y - info2.point.y;
                }


                newPosition = info.point + direction * stepLength * body.forward + footOffset - raise;
                newNormal = info2.normal;
            }

        } 

        if(lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            currentPosition = tempPosition;

            Vector3 tempNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            currentNormal = tempNormal;

            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
        
    }

    private void OnDrawGizmos()
    {
        if(hitinfo.transform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(newPosition, 0.05f);

        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hitinfo.point, 0.07f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hitinfo2.point, 0.07f);


    }
    public bool IsMoving()
    {
        return lerp < 1;
    }


}


/*
Ray ray = new Ray(body.position + body.right * footSpacing, Vector3.down);
if (Physics.Raycast(ray, out RaycastHit info, 5, groundLayer.value))
{
    //transform.up = info.normal;

    hitinfo = info;
    /*
    temp.transform.up = hitinfo.normal;

    temp.transform.Rotate(Vector3.right, 180f, Space.Self);

    temp.transform.Rotate(Vector3.up, 90f, Space.Self);
    temp.transform.Rotate(Vector3.forward, -90f, Space.Self);

    transform.rotation = Quaternion.Euler( temp.rotation.eulerAngles);
    */
    /*
    transform.rotation = Quaternion.LookRotation(Vector3.up, info.normal);
    transform.Rotate(Vector3.right, 180f, Space.Self);
    transform.Rotate(Vector3.up, 90f, Space.Self);
    */

//}

