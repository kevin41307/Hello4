using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    float walkSpeed = 2.5f;
    float runSpeed = 4.5f;
    bool isWalkSlowlying = false;
    float shootingWalkSpeed = 1f;
    const float rotateSpeedMore = 9f;
    const float rotateSpeed_original = 1.15f;
    float rotateSpeed = 1.25f;
    float moveSpeed = 1f;
    Animator _animator;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    public float gravity = -12;
    public float jumpHeight = 1;
    float speedSmoothTime = 0.08f;
    float velocityY;
    float currentSpeed;
    float rotateSmoothTime = 0.1f;
    Vector2 input = Vector2.zero;
    Vector2 inputDir = Vector2.zero;
    [Range(0, 1)]
    public float airControlPercent;
    float speedSmoothVelocity;
    CharacterController controller;
    AnimatorStateInfo aniStateInfo;
    float turnSmoothVelocity;
    RaycastHit hitGroundRay;

    //外力推擠
    bool isPushing = false;


    [Header("Cameras")]
    [HideInInspector]
    public bool isLockOn = false;
    public Cinemachine.CinemachineFreeLook freeLookCamera;
    public Cinemachine.CinemachineFreeLook lockOnCamera; //lockOnCamera
    public Cinemachine.CinemachineTargetGroup targetGroup;

    float topRigRadius_Run = 7f;
    float middleRigRadius_Run = 5.75f;
    float bottomRigRadius_Run = 4.75f;
    float topRigRadius_Walk = 5.25f;
    float middleRigRadius_Walk = 4f;
    float bottomRigRadius_Walk = 3f;

    //區域變數外移
    bool running;
    float targetRotation;
    float targetSpeed;
    float animationSpeedPercent;
    RaycastHit landingHit;
    Vector3 targetDir;
    Camera mainCamera;

    //public Cinemachine.CinemachineVirtualCamera virtualCamara;
    [HideInInspector]
    public Transform lookAtTarget = null;
    Transform nearbyEnemy = null;
    float senseRange = 15f;

    VolumeController volumeController;


    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        targetGroup = targetGroup.GetComponent<Cinemachine.CinemachineTargetGroup>();
        mainCamera = Camera.main;
        volumeController = GameObject.FindGameObjectWithTag("GlobalVolume").GetComponent<VolumeController>();
    }
    private void Start()
    {
        isLockOn = false;
    }

    private void Update()
    {
        input = Vector2.zero;
        inputDir = Vector2.zero;

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputDir = input.normalized;
        running = Input.GetKey(KeyCode.LeftShift);
        aniStateInfo = _animator.GetCurrentAnimatorStateInfo(0);


        //Debug.Log("v=" + controller.velocity);    

        if (CanMove())
        {
            Move(inputDir, running);

            if (running && inputDir.magnitude > 0f)
            {
                Game.spBarSingle.ChangeBarValue(new ChangeInfo(-10f * Time.deltaTime));
            }
            //Debug.Log("move");
        }

        if (isLockOn)
        {
            HandleRotation();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isLockOn)
            {
                OnAssignLookOverride();
            }
            else
            {
                OnClearLookOverride();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded && aniStateInfo.IsName("Move"))
        {
            Jump();
        }

           

    }

    private void LateUpdate()
    {
        //ChangeCameraOrbits();
    }

    private void FixedUpdate()
    {
        IsLanding();
    }

    public void Move(Vector2 inputDir, bool running)
    {
        float direction = Mathf.Clamp(inputDir.magnitude, 0, 1) * inputDir.magnitude;

        if (!isLockOn)
        {
            AnimatorStateInfo stateInfo1 = _animator.GetCurrentAnimatorStateInfo(1);
            if (inputDir != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(0.1f));
            }


            float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

            if (stateInfo1.IsTag("blocking"))
            {
                targetSpeed *= 6f;
            }
            if (isWalkSlowlying)
                targetSpeed = shootingWalkSpeed;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, .1f);

            velocityY += Time.deltaTime * gravity;

            // 暫時水平移動用root motion 取代 targetSpeed * new Vector3( inputDir.x, inputDir.y) +
            Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
            currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

            if (controller.isGrounded)
            {
                velocityY *= 0.9f;
            }

            //Debug.Log("vY" + velocityY + "," + controller.isGrounded);
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
            if (isWalkSlowlying)
                targetSpeed = shootingWalkSpeed;
            velocityY += Time.deltaTime * gravity;
            Vector3 velocity2 = Vector3.zero;
            velocity2 += targetSpeed * (transform.forward * inputDir.y + transform.right * inputDir.x) + Vector3.up * velocityY;
            //velocity2 = transform.forward * currentSpeed + Vector3.up * velocityY;
            //currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
            if (controller.isGrounded)
            {
                velocityY *= 0.9f;
            }
            //旋轉移到外面
            controller.Move(velocity2 * Time.deltaTime);

        }


        //緩慢開始 緩慢結束
        //Debug.Log("isground=" + controller.isGrounded);
        if (inputDir.magnitude > 0.01f)
        {
            _animator.SetFloat("X", inputDir.x, speedSmoothTime, Time.deltaTime);
            _animator.SetFloat("Y", inputDir.y, speedSmoothTime, Time.deltaTime);
            _animator.SetFloat("direction", direction, speedSmoothTime, Time.deltaTime);
            animationSpeedPercent = ((running) ? runSpeed * inputDir.magnitude : 0);
            _animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat("X", 0, speedSmoothTime / 2, Time.deltaTime);
            _animator.SetFloat("Y", 0, speedSmoothTime / 2, Time.deltaTime);
            _animator.SetFloat("direction", 0, speedSmoothTime / 2, Time.deltaTime);
            _animator.SetFloat("speedPercent", 0, speedSmoothTime / 2, Time.deltaTime);
        }


        _animator.SetBool("isGrounded", controller.isGrounded);




        if( isPushing )
        {
           
            controller.SimpleMove(-transform.forward * 3f);
        }


        //var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        //HandleRotation(x, y);


        /*
        if ( inputDir.magnitude > 0.01f && !isLockOn)
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0), ref rotationSmoothVelocity, .12f);
            transform.eulerAngles = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);  
        */


        //transform.position += Vector3.forward * inputDir.y * Time.deltaTime;
        //transform.position += Vector3.right * inputDir.x * Time.deltaTime;



        //this.transform.rotation = Quaternion.Euler( 0, Camera.main.transform.rotation.eulerAngles.y, 0);

        //Debug.Log("move" + direction);

        //Debug.Log("move* scaledMoveSpeed" +  move * scaledMoveSpeed);

        //transform.position += Vector3.forward * y * scaledMoveSpeed;
        //transform.position += Vector3.right * x * scaledMoveSpeed;

    }

    void ChangeCameraOrbits()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isLockOn)
        {
            for (int i = 0; i < freeLookCamera.m_Orbits.Length; i++)
            {
                float radius = 0f;
                if (i == 0) radius = topRigRadius_Run;
                if (i == 1) radius = middleRigRadius_Run;
                if (i == 2) radius = bottomRigRadius_Run;

                freeLookCamera.m_Orbits[i].m_Radius = Mathf.Lerp(freeLookCamera.m_Orbits[i].m_Radius, radius, 7f * Time.deltaTime);
            }

        }

        if (!Input.GetKey(KeyCode.LeftShift) && !isLockOn)
        {

            float radius = 0f;
            for (int i = 0; i < freeLookCamera.m_Orbits.Length; i++)
            {
                //if ((Mathf.Abs(freeLookCamera.m_Orbits[0].m_Radius - topRigRadius_Walk) < 0.001f)) break;
                if (i == 0) radius = topRigRadius_Walk;
                if (i == 1) radius = middleRigRadius_Walk;
                if (i == 2) radius = bottomRigRadius_Walk;

                freeLookCamera.m_Orbits[i].m_Radius = Mathf.Lerp(freeLookCamera.m_Orbits[i].m_Radius, radius, 7f * Time.deltaTime);
            }
        }
    }
    Transform GetNearbyEnemy()
    {
        return nearbyEnemy;
    }
    Transform SenseNearbyEnemy()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, senseRange, 1 << 9);
        if (hit.Length > 0)
        {
            float minDistance = 9999f;
            Transform tempEnemy = null;
            foreach (var enemy in hit)
            {
                float distance = Vector3.Distance(this.transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    tempEnemy = enemy.transform;
                    
                    nearbyEnemy = tempEnemy.gameObject.GetComponentInChildren<CamPos>().GetCamPos();


                }
            }
            /*
            Ray ray = new Ray();
            ray.origin = this.transform.position;
            ray.direction = tempEnemy.position - this.transform.position;
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, minDistance))
            {
                if (hitInfo.transform == this.transform)
                {
                    nearbyEnemy = tempEnemy.transform;
                }
            }
            */
        }
        else
        {
            nearbyEnemy = null;
        }

        return nearbyEnemy;
    }

    void IsLanding()
    {

        if (aniStateInfo.IsName("Air") && Physics.Raycast(this.transform.position, Vector3.down, out landingHit, 1.5f, 1 << 0))
        {
            Debug.DrawLine(this.transform.position, landingHit.point, Color.red);
            _animator.SetBool("Landing", true);
        }

    }

    void Jump()
    {
        _animator.SetTrigger("Jump");
        _animator.SetBool("Landing", false);
        _animator.SetBool("CanDodge", false);
        velocityY = 0f;
        if (controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }
    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    bool CanMove()
    {
        return _animator.GetBool("CanMove");
    }

    void OnAssignLookOverride()
    {
        if (SenseNearbyEnemy() == null) return;
        lookAtTarget = nearbyEnemy;
        Game.Singleton.nearbyEnemy = nearbyEnemy;
        isLockOn = true;
        Game.Singleton.isLockOn = isLockOn;

        targetGroup.m_Targets[1].target = lookAtTarget;
        freeLookCamera.gameObject.SetActive(false);
        lockOnCamera.gameObject.SetActive(true);

        _animator.SetFloat("LockOn", 1f);
        this.transform.position += new Vector3(0.001f, 0, 0);
        this.transform.position -= new Vector3(0.001f, 0, 0);

    }
    public void OnClearLookOverride()
    {
        lookAtTarget = null;
        Game.Singleton.nearbyEnemy = null;
        isLockOn = false;
        Game.Singleton.isLockOn = isLockOn;
        //freeLookCamera.gameObject.transform.position = lockOnCamera.gameObject.transform.position;     
        freeLookCamera.gameObject.SetActive(true);
        lockOnCamera.gameObject.SetActive(false);
        _animator.SetFloat("LockOn", 0f);
    }

    void HandleRotation()
    {
        //Debug.Log("LockOn" + isLockOn);
        if (isLockOn)
        {

            targetDir = Vector3.zero;
            if (isLockOn)
            {
                targetDir = lookAtTarget.transform.position - this.transform.position;
            }
            targetDir = targetDir.normalized;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = this.transform.forward;

            float rs = rotateSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(this.transform.rotation, tr, rs * Time.deltaTime);
            this.transform.rotation = targetRotation;

        }
    }

    void HandleRotation(Vector2 inputDir)
    {
        if (inputDir != Vector2.zero)
        {
            targetRotation = 0f;
            targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(0.08f));
        }
    }

    IEnumerator StartChangeRotateSpeed()
    {
        rotateSpeed = rotateSpeedMore;
        //Debug.Log("StartChangeRotateSpeed");
        yield return new WaitForSeconds(0.8f);
        rotateSpeed = rotateSpeed_original;
    }

    IEnumerator StarWalkSlowly()
    {
        isWalkSlowlying = true;
        AnimatorStateInfo stateInfo;
        stateInfo = _animator.GetCurrentAnimatorStateInfo(2);
        while (aniStateInfo.IsName("Shoot") || aniStateInfo.IsName("Eat") || aniStateInfo.IsName("NoBullet"))
        {
            stateInfo = _animator.GetCurrentAnimatorStateInfo(2);
            yield return new WaitForSeconds(0.2f);
        }
        isWalkSlowlying = false;
    }

    IEnumerator AddMove_Backward()
    {
        //controller.SimpleMove(-transform.forward * 40f );
        isPushing = true;
        StartCoroutine(volumeController.StartMotionBlurIn2ms());
        Game.cacheObjectControllerSingle.BorrowAsset(CacheObjectController.ObjType.smokeSheet1, transform.position, Quaternion.identity.eulerAngles);
        yield return new WaitForSeconds(0.2f);
        
        isPushing = false;
    }

    RaycastHit Raycast(Vector3 offset, Vector3 rayDirection, float length, LayerMask layer)
    {
        Vector3 pos = transform.position;
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(pos + offset, rayDirection, out hitInfo, length, layer) ;

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDirection, color);
        return hitInfo;
    }


    /*
    void OnAnimatorIK(int layerIndex)
    {
        _animator.SetLookAtWeight(0.7f);
        if (_animator)
        {
            if (nearbyEnemy != null)
            {
                _animator.SetLookAtPosition(nearbyEnemy.position);
            }
        }

    }
    */
    /*
    bool IsGround()
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out hitGroundRay, 0.1f, 1 << 0))
        {
            Debug.DrawLine(this.transform.position, hitGroundRay.point, Color.blue);
            _animator.SetBool("isGrounded", true);
            isGrounded = true;

        }
        else
        {
            _animator.SetBool("isGrounded", false);
            isGrounded = false;
        }
        return isGrounded;
    }
    */
}
