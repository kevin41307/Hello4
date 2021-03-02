using UnityEngine;
using UnityEngine.UI;

public class BeLockedUI : MonoBehaviour
{
    Image image;

    private void Awake()
    {
        image = this.GetComponent<Image>();
    }



    private void Update()
    {
        if( Game.Singleton.isLockOn && Game.Singleton.nearbyEnemy != null)
        {
            Vector3 enemyPos = Game.Singleton.nearbyEnemy.transform.position;
            enemyPos.y -= 0.5f;
            Vector3 point = Camera.main.WorldToScreenPoint(enemyPos);
            this.transform.position = point;

            if (image.color.a == 1) return;
            image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            if (image.color.a == 0) return;
            image.color = new Color(1, 1, 1, 0);
        }
    }
}
