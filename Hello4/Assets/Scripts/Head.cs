using UnityEngine;

public class Head : MonoBehaviour
{
    private Transform _head;
    [HideInInspector]
    public Transform head 
    { get 
        {
            if (_head == null )
                _head = this.GetComponent<Transform>();
            return _head;
        } 
    }
    
    private void Awake()
    {
        _head = this.GetComponent<Transform>();
    }

}
