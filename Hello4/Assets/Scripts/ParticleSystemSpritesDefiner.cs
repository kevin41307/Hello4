using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSpritesDefiner : MonoBehaviour
{
    ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        var tex = ps.textureSheetAnimation;
        Vector2 xy = new Vector2(tex.numTilesX, tex.numTilesY);
        if (xy.magnitude != 0)
            tex.frameOverTime = new ParticleSystem.MinMaxCurve(Random.Range(0, xy.x * xy.y) / (xy.x * xy.y));
        else
        {
            Debug.Log("Please set gridX and gridY in " + transform.name);
        }
    }
    /*
    private void Start()
    {

    }
    */

}
