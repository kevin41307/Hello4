using System.Text.RegularExpressions;
using UnityEngine;

public class InteractiveWithPlayer 
{
    Transform player;
    Transform other;
    public float rorateSpeed = 3.5f;
    [HideInInspector]
    public float attackRange = 1.54f;
    float stopDistance = 1.33f;
    float observeScope = 30f;
    int currentState = -1;
    bool stateChange = false;


    public InteractiveWithPlayer(Transform _other)
    {
        other = _other;
        player = Game.playerAttrSingle.player.transform;
    }

    public InteractiveWithPlayer(Transform _other, float _rorateSpeed)
    {
        other = _other;
        rorateSpeed = _rorateSpeed;
        player = Game.playerAttrSingle.player.transform;
    }

    public InteractiveWithPlayer(Transform _other, float _rorateSpeed, float _attackRange)
    {
        rorateSpeed = _rorateSpeed;
        attackRange = _attackRange;
        other = _other;
        player = Game.playerAttrSingle.player.transform;
        //Debug.Log("other.position" + other.position);
    }

    public bool PlayerInDistance()
    {
        //Debug.Log("other.position" + other.position);
        return Vector3.Distance(player.position, other.position) <= attackRange;
    }

    public bool PlayerInDistance(float distance)
    {
        //Debug.Log("other.position" + other.position);
        return Vector3.Distance(player.position, other.position) <= distance;
    }


    public float PlayerDistance()
    {
        //Debug.Log("other.position" + other.position);
        return Vector3.Distance(player.transform.position, other.position);
    }


    public Vector3 PlayerDirectionNormailized()
    {
        Vector3 direction = (player.position - other.position).normalized;
        direction.y = 0f;
        return direction;
    }

    public void LookAtTargetButNotAccuracy()
    {
        Vector3 direction = PlayerDirectionNormailized();
        float rs = rorateSpeed;
        Quaternion tr = Quaternion.LookRotation(direction);
        Quaternion targetRotation = Quaternion.Slerp(other.rotation, tr, rs * Time.deltaTime);
        other.rotation = targetRotation;
    }

    public void LookAtTarget( float _rotateSpeed )
    {
        Vector3 direction = PlayerDirectionNormailized();
        float rs = _rotateSpeed;
        Quaternion tr = Quaternion.LookRotation(direction);
        Quaternion targetRotation = Quaternion.Slerp(other.rotation, tr, rs * Time.deltaTime);
        other.rotation = targetRotation;
    }


    public float RandomSign()
    {
        return (Random.value > 0.5) ? 1 : -1;
    }

    public bool IsPlayerDistanceStateChanged()
    {
        PlayerDistanceState();
        return stateChange;
    }
    
    public int GetCurrenDistanceState()
    {
        PlayerDistanceState();
        return currentState;
    }



    void PlayerDistanceState()
    {
        float playerDistance = PlayerDistance();
        int state = -1;
        if (playerDistance < stopDistance)
        {
            // <1.33f 玩家距離過近
            //Debug.Log("玩家距離過近");
            state = 0;
        }
        if (playerDistance >= stopDistance && playerDistance < 2f)
        {
            // 1.33f~2f 玩家距離1.33f~2f
            //Debug.Log("玩家距離1.33f~2f");
            state = 1;
        }
        if (playerDistance >= 2f && playerDistance < observeScope / 6)
        {
            //2f ~ 5f
            //Debug.Log("玩家距離適中");
            state = 2;

        }
        if (playerDistance >= observeScope / 6 && playerDistance < observeScope / 3)
        {
            //5f~10f 
            state = 3;
        }
        if (playerDistance >= observeScope / 3 && playerDistance < observeScope)
        {
            //10f~30f 玩家距離過遠
            state = 4;
        }
        if (playerDistance > observeScope)
        {
            //>30f 玩家逃離戰場
            state = 5;
            Debug.Log("玩家逃離戰場");
        }

        if (currentState != state)
        {
            //Debug.Log("玩家改變距離");
            currentState = state;
            stateChange = true;
        }
        else
        {
            //Debug.Log("玩家沒有改變距離");
            stateChange = false;
        }
    }

    
}
