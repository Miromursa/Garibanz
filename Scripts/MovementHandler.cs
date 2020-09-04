using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    //load from gameobject
    public AnimationHandler animationHandler;
    public Animator anim;
    public CameraHandler cameraHandler;
    public Rigidbody rb;

    //jump
    public bool isFalling;
	public float fallMulti = 2.5f;
	public float lowJumpMulti = 2f;
    public int jumpVelocity = 6;

    //main cam
    public Transform cam;

    //speed
    public float normalSpeed = 10f;
    public float sprintSpeed;
    private float speed;

    //Camera and input variables
    public float turnSmooth = 0.1f;

    private float horizontal;
    private float vertical;
    private float turnSmoothVelocity;
    Vector3 direction;

    //wichtig für Kampf
    private bool isMovementEnabled = true;



    void Start()
    {
        animationHandler = GetComponentInChildren<AnimationHandler>();
        anim = GetComponentInChildren<Animator>();
        cameraHandler = GetComponent<CameraHandler>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
    }
    void FixedUpdate()
    {
        if (isMovementEnabled)
            Movement();
    }

    void Movement()
    {
        //Sprint with shift key
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = normalSpeed;
        }

	    //"groundcheck" alternative
        if (rb.velocity.y == 0f) isFalling = false;
        else isFalling = true;

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && !isFalling) rb.velocity = Vector3.up* jumpVelocity;

        ///normal grav for jumping up, higher grav for falling down
        //+ lowjump implementation
        if (rb.velocity.y < 0) 
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMulti - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) 
            rb.velocity += Vector3.up* Physics.gravity.y * (lowJumpMulti - 1) * Time.deltaTime;

        //if we are in lock on mode, we rotate our character towards our enemy
        if (!cameraHandler.freeLookCamera)
        {
            Vector3 vectorToTarget = new Vector3(cameraHandler.lockOnCam.m_LookAt.transform.position.x - transform.position.x,
                                    cameraHandler.lockOnCam.m_LookAt.transform.position.y - transform.position.y,
                                    cameraHandler.lockOnCam.m_LookAt.transform.position.z - transform.position.z);
            float cameraAngle;
            if (transform.position.z > cameraHandler.lockOnCam.m_LookAt.transform.position.z)
            {
                cameraAngle = -(Vector3.Angle(Vector3.right, vectorToTarget));
            }
            else
            {
                cameraAngle = (Vector3.Angle(Vector3.right, vectorToTarget));
            }

            if (!anim.GetBool("isShiftKey"))
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, cameraHandler.lockOnCam.transform.rotation, 15f);
                transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
            }
        }
        //the actual movement
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            
            if (cameraHandler.freeLookCamera || anim.GetBool("isShiftKey"))
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = moveDir.normalized * speed;
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }

    public void enableMovement()
    {
        isMovementEnabled = true;
    }

    public void disableMovement()
    {
        isMovementEnabled = false;
    }
}

