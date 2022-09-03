using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidEnemy2 : MonoBehaviour
{
    private Rigidbody rb;
    private float rotationalDamp = .5f;
    private float rayCastOffset = 2.5f;
    private float detectionDistance = 20f;
    public Rigidbody torsorb;
    public Rigidbody headrb;
    public Transform head;
    public Transform size;
    private Transform[] groundChecks;
    public float groundCheckRadius = .1f;
    private int nLegs;
    public Transform root;
    private bool minOneGrounded = false;
    private float force;
    private float legPushForce = 1.7f;
    private IkEnemy ik;
    private float speed=0f;
    private EnemyState state;

    // Start is called before the first frame update
    

    private void Awake()
    {
        ik = GetComponent<IkEnemy>();
         rb = root.GetComponent<Rigidbody>();
        force = rb.mass * -Physics.gravity.y;
        UpdateState(EnemyState.active);
        nLegs = ik.legs.Length;

        groundChecks = new Transform[nLegs];
        for (int i = 0; i < nLegs; i++)
        {
        
            groundChecks[i] = ik.legs[i].transform;
        }
    }

    private void FixedUpdate()
    {
        if (state == EnemyState.active)
        {
            //Iterate all legs and apply force up if leg is toutching
            for (int i = 0; i < nLegs; i++)
            {
                if (Physics.CheckSphere(groundChecks[i].position, groundCheckRadius, ik.whatIsGround))
                {
                    if (state == EnemyState.active)
                    {
                        print("ypyppy");
                        rb.AddForce(root.up * force * legPushForce);
                    }
                    else if (state == EnemyState.recovering)
                    {
                        rb.AddForce(Vector3.up * force * legPushForce);
                    }
                }
            }
        }

        if (state == EnemyState.active)
        {
            StabilizingBody();
        }

    }

    void StabilizingBody()
    {
        headrb.AddForce(Vector3.up *force);
        torsorb.AddForce(Vector3.up *force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private enum EnemyState
    {
        active,
        recovering
    }
    
    void UpdateState(EnemyState s)
    {
        if (state == s) return;
        state = s;
        switch (s) {
            case EnemyState.active:
                rb.drag = 2f;
                rb.angularDrag = 2f;
                ///     rotationForce = 0.05f;
                break;
            case EnemyState.recovering:
                rb.drag = 2f;
                //  Headrb.AddForce(Vector3.up*1.5f);
                rb.angularDrag = 2f;
                //rotationForce = 0.05f;
                break; 
            default:
                rb.drag = 0f;
                rb.angularDrag = 0f;
                //  rotationForce = 0f;
                break;
        }
    
    }
}
