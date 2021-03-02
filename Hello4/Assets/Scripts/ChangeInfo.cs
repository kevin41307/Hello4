using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChangeInfo
{
    public ChangeInfo(string _fromWho, float _delta, bool _recovery) { fromWho = _fromWho; delta = _delta; recovery = _recovery; }
    public ChangeInfo(float _delta, bool _recovery) { delta = _delta; recovery = _recovery; }
    public ChangeInfo(float _delta) { delta = _delta; }

    public ChangeInfo(float _delta, float _motionRate) { delta = _delta; motionRate = _motionRate; }


    public ChangeInfo(float _delta, float _motionRate,  float _woundPower ) { delta = _delta; woundPower = _woundPower; motionRate = _motionRate; }

    public ChangeInfo() { delta = 0f; recovery = false; }

    public string fromWho;
    public float delta;
    public bool recovery = true;
    public bool percent;
    public float woundPower = 0f;
    public bool directDamage = false;
    public float hitFromRightSide = 0f;
    public bool hitByGun = false;
    public Vector3 hitFromPosition = Vector3.zero;
    public Vector3 attackDirection = Vector3.zero;
    public Vector3 contactPoint = Vector3.zero;
    public float motionRate = 1f;
    public Transform murderer;



}

