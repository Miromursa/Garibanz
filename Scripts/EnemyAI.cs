using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    float moveSpeed = 2f; //move speed
    float rotationSpeed = 3f;
    public Rigidbody rb;

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 playerPOS = target.transform.position;
        Vector3 selfPOS = transform.position;
        Vector3 followVector = Vector3.MoveTowards(selfPOS, playerPOS, 0.06f);
        rb.MovePosition(followVector);

    }
}
