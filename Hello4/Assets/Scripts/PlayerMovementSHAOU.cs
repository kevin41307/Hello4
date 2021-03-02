using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSHAOU : MonoBehaviour
{
    private CharacterController controller;

    private Vector3 moveDir;

    private bool running;
    public bool isGround;

    public float walkSpeed = 5f,runSpeed = 15f;
    private float gravity = -20f;
    public float jumpForce = 10f;
    private float velocityY;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        moveDir = transform.TransformDirection(moveDir);
        running = Input.GetKey(KeyCode.LeftShift);
        Gravity();
        moveDir = ((running) ? runSpeed : walkSpeed) * moveDir + Vector3.up * velocityY;
        

        controller.Move(moveDir * Time.deltaTime);
        if (controller.isGrounded)
        {
            velocityY = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (controller.isGrounded)
            isGround = true;
        else
            isGround = false;
    }


    void Jump()
    {
        if (controller.isGrounded)
        {
            velocityY = Mathf.Sqrt(-2 * gravity * jumpForce);
        }
    }

    void Gravity()
    {
        velocityY += Time.deltaTime * gravity;       
    }
}
