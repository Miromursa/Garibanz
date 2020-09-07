using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    //states
    private enum States { idle, walk, sprint, attack, roll };
    private States currentState;

    //need to be initialized in Start() method
    public AnimationHandler animationHandler;
    public Animator anim;
    public CameraHandler cameraHandler;
    public Rigidbody rb;

    //main cam
    private Transform cam;

    //speed variables
    private float normalSpeed = 12f;
    private float sprintSpeed = 15f;

    //Camera and input variables
    public float turnSmooth = 0.1f;

    private float horizontal;
    private float vertical;
    private float turnSmoothVelocity;
    private Vector3 direction;

    //important for combat, but we need to replace this
    private bool isMovementEnabled = true;

    void Start()
    {
        //lock cursor in screen
        Cursor.lockState = CursorLockMode.Locked;

        //set default state to idle
        currentState = States.idle;

        //get all the needed Components
        animationHandler = GetComponent<AnimationHandler>();
        anim = GetComponent<Animator>();
        cameraHandler = GetComponent<CameraHandler>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    //Input detection is handled every frame
    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        stateSwitcher();
    }

    //Movement is handled via FixedUpdate because we use Rigidbody and physics
    void FixedUpdate()
    {
        switch (currentState)
        {
            case States.idle: idle(); break;
            case States.walk: walk(); break;
            case States.sprint: sprint(); break;
            case States.attack: attack(); break;
            case States.roll: roll(); break;
            default: break;
        }
    }

    void idle()
    {
        if (!cameraHandler.isFreeCam())
        {
            rotateToTarget();
        }

        //nullifies x, y and z movement
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
      
    }

    void walk()
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);

        if (!cameraHandler.isFreeCam())
        {
            rotateToTarget();
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        rb.velocity = moveDir.normalized * normalSpeed;
    }

    void sprint()
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        rb.velocity = moveDir.normalized * sprintSpeed;
    }

    void attack()
    {

    }

    void roll()
    {

    }

    void stateSwitcher()
    {
        switch (currentState)
        {
            case States.idle:
                if (direction.magnitude >= 0.1f && Input.GetKey("left shift"))
                {
                    currentState = States.sprint;
                }
                else if (direction.magnitude >= 0.1f)
                {
                    currentState = States.walk;
                }

                if (Input.GetKey("space"))
                {
                    currentState = States.roll;
                }
                break;

            case States.walk:
                if (Input.GetKey("left shift"))
                {
                    currentState = States.sprint;
                }

                if (direction.magnitude < 0.1f)
                {
                    currentState = States.idle;
                }
                break;

            case States.sprint:
                if (!Input.GetKey("left shift"))
                {
                    currentState = States.walk;
                }

                if (direction.magnitude < 0.1f)
                {
                    currentState = States.idle;
                }
                break;

            case States.roll:
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Roll") &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    currentState = States.idle;
                }
                break;
            //case States.attack: attack(); break;

            default: break;
        }
    }

    void rotateToTarget()
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

        transform.rotation = Quaternion.RotateTowards(transform.rotation, cameraHandler.lockOnCam.transform.rotation, 15f);
        transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
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

