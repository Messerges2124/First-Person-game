using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using DitzelGames.FastIK;
using Debug = UnityEngine.Debug;

public class RigidEnemy : MonoBehaviour
{
    private Rigidbody rb;
    public Transform target;
    private float rotationalDamp = .5f;
    private float rayCastOffset = 2.5f;
    private float detectionDistance = 20f;
    public Rigidbody Headrb;
    private bool lookatplayer = false;
    public Rigidbody torsorb;
    public Transform head;
    public Transform size;
   // public Transform PlayerMovement;
    private Transform[] groundChecks;
    public float groundCheckRadius = .1f;
    private int nLegs;
   // public EnemyAi enemyAi;
    public Transform root;
    private bool minOneGrounded = false;
    private float force;
    private float legPushForce = 2f;
    private IkEnemy ik;
    private float speed=0f;
    private EnemyState state = EnemyState.active;

    private bool hit = false;
    // Start is called before the first frame update
   

  

    void Awake()
    {
        //IkEnemy = new IkEnemy();
         ik = GetComponent<IkEnemy>();
         rb = root.GetComponent<Rigidbody>();
         force = rb.mass * -Physics.gravity.y;
        // UpdateState(EnemyState.active);
        UpdateState(EnemyState.recovering);
         nLegs = ik.legs.Length;
         groundChecks =new Transform[nLegs];
        
         for (int i = 0; i < nLegs; i++)
         {
             groundChecks[i] = (ik.legs[i].transform);
         }
         
     }

    private void FixedUpdate()
    {
        if (!hit)
        {
            RotateBody();
           
            //  if (speed == 0)
            //  {
            //      Vector3 temp = new Vector3(1, 0, -1);
            //      IkEnemy.legTargetOffsets[0] = temp;
            //      IkEnemy.legTargetOffsets[1] = temp;
            //
            //  }
            //
            // else if (speed > 5)
            //  {
            //      Vector3 temp = new Vector3(1, 0, -1);
            //      IkEnemy.legTargetOffsets[0] = temp;
            //      IkEnemy.legTargetOffsets[1] = temp;
            //  }else if (speed < -5)
            //  {
            //      Vector3 temp = new Vector3(-1, 0, -1);
            //      IkEnemy.legTargetOffsets[0] = temp;
            //      IkEnemy.legTargetOffsets[1] = temp;
            //  }
            for (int i = 0; i < nLegs; i++)
            {
                groundChecks[i] = (ik.legs[i].transform);
            }
            
            for (int i = 0; i < nLegs; i++)
            {
                if (Physics.CheckSphere(groundChecks[i].position, groundCheckRadius, ik.whatIsGround))
                {
                    if (state == EnemyState.active)
                    {
                        //rb.AddForce(root.up * force * legPushForce);
                      //  print("hey");
                      //  rb.AddForce(Vector3.up * force * legPushForce* 60f);
                      //  Headrb.AddForce(root.up * 20f);

                    }
                    else if (state == EnemyState.recovering)
                    {
                        
                        if (size.localScale.x > 1.0)
                        {
                           // print("2");

                          //  Headrb.AddForce(root.forward * speed* 2f);
                        //    rb.AddForce(Vector3.up * force * legPushForce* 1.5f);

                        }
                        else
                        {
                          //  print("3");
                          rb.AddForce(Vector3.up * force * legPushForce);
                          //Headrb.AddForce(root.up * 20f);

                        // Headrb.AddForce(Vector3.up * 9.8f);
                         //rb.AddForce(Vector3.up *20f);
                          //rb.AddForce(root.up * 10f);
                           // Headrb.AddForce(root.forward * speed);
                           // rb.AddForce(Vector3.up * force * legPushForce);
                        }
                    }

                    minOneGrounded = true;
                }
             //    if (Vector3.Distance(Headrb.transform.position, agent.transform.position) > 4f)
             //    {
             //        if (size.localScale.x > 1.0)
             //        {
             //            agent.speed = 10f;
             //        }
             //        else
             //        {
             //            agent.speed = .1f;
             //        }
             //    }
             //    else
             //    {
             //        if (size.localScale.x > 1.0)
             //        {
             //            agent.speed = 10f;
             //        }
             //        else
             //        {
             //            agent.speed = 3f;
             //        }
             //    }
             //    {
             //    
             //    }
             //
             //    if (enemyAi.isplayerAttackRange() && enemyAi.isplayerSightRange())
             //    {
             //        lookatplayer = true;
             //
             //
             //    }
             //    else
             //    {
             //        lookatplayer = false;
             //    }
             //
             //    if (Vector3.Distance(root.transform.position,agent.transform.position) <= 3)
             //     {
             //         speed = -10;
             // //   print("hey");
             //     }
             //     else
             //     {
             //         speed = 10;
             //     }
             //
             //    if (!agent.hasPath)
             //    {
             //        speed = 0;
             //    }
             //    //     if (Vector3.Distance(root.transform.position, enemyAi.getWalkPoint()) <= 5)
             //    //     {
             //    //         speed = -5;
             //    //         //print("hey");
             //    //     }
             //    //     else
             //    //     {
             //    //         speed = 10;
             //    //     }
             //        
             //    //}
            }



            if (minOneGrounded)
            {

            }

            if (state == EnemyState.active)
            {
                StabilizingBody();
            }

            if (state == EnemyState.recovering)
            {

            }
        }

    }

    private void Update()
    {
        
       // Headrb.AddForce(Vector3.up*2f);
        //Quaternion temp =  Quaternion.Euler( Headrb.transform.rotation.x, Headrb.transform.rotation.y, Headrb.transform.rotation.z);
       // Headrb.transform.rotation = temp;
    }

    private enum EnemyState
    {
        active,
        recovering
    }
    
  
    
    public Vector3 GetVelocity()
    {
        if (rb.velocity.magnitude > 1)
        {
            return rb.velocity.normalized;
        }
    
        return rb.velocity;
    }
    
    void StabilizingBody()
    {
       // Headrb.AddForce(Vector3.forward*force);
      
        //torsorb.AddForce(Vector3.down*force);
    }

   // private float angle;
    private float rotationForce = 5f;
    float damp = 5f;
    void RotateBody()
    {
       // Vector3 pos = target.position - root.position;
       // Quaternion rotation = Quaternion.LookRotation(pos);
        //mptransform.rotation=Quaternion.Slerp(root.rotation,rotation,rotationalDamp*Time.deltaTime);
        // if (!lookatplayer)
        // {
        //     Vector3 offset = new Vector3(agent.nextPosition.x, agent.nextPosition.y + 2.5f, agent.nextPosition.z);
        //     var rotationAngle = Quaternion.LookRotation(offset - root.position); // we get the angle has to be rotated
        //     root.rotation = Quaternion.Slerp(root.rotation, rotationAngle, Time.deltaTime * damp); // we rotate the rotationAngle 
        // }
        // else
        // {
        //     Vector3 offset = new Vector3(PlayerMovement.transform.position.x, PlayerMovement.transform.position.y ,PlayerMovement.transform.position.z);
        //     var rotationAngle = Quaternion.LookRotation(offset - root.position); // we get the angle has to be rotated
        //     root.rotation = Quaternion.Slerp(root.rotation, rotationAngle, Time.deltaTime * damp); // we rotate the rotationAngle 
        // }


        //  print(PlayerMovement.position);
        //      float rootAngle = -Headrb.transform.eulerAngles.y;
        //     // print(PlayerMovement.transform.position); 
        //    //  Vector3 offset = new Vector3(0f, 0f, 10f);
        //     float desiredAngle = Quaternion.LookRotation(agent.transform.position + Headrb.position).eulerAngles.y;
        //    // angle between player and robot
        //     float deltaAngle = Mathf.DeltaAngle(rootAngle, desiredAngle);
        //  //   print(deltaAngle);
        //
        //   //  root.rotation = PlayerMovement.rotation;
        //     //clamp the angle, the use it to rotate towards the player
        //     deltaAngle = Mathf.Clamp(deltaAngle, -2f, 2f);
        //  //  Headrb
        //   // print(deltaAngle);
        //  // force = 10;
        // rb.AddTorque(Vector3.up*deltaAngle*force*rotationForce);
    }


   // public NavMeshAgent agent;
    //public Transform destination;
  
    

    // public void Sethit(bool hitT)
    // {
    //     hit = hitT;
    // }

    
     
       void UpdateState(EnemyState s)
       {
           if (state == s) return;
           state = s;
           switch (s) {
               case EnemyState.active:
                   rb.drag = 2f;
                   rb.angularDrag = 20f;
              ///     rotationForce = 0.05f;
                   break;
               case EnemyState.recovering:
                   rb.drag = 2f;
                 //  Headrb.AddForce(Vector3.up*1.5f);
                   rb.angularDrag = 2f;
                   rotationForce = 0.05f;
                   break; 
               default:
                    rb.drag = 0f;
                    rb.angularDrag = 0f;
                  //  rotationForce = 0f;
                    break;
           }
    
       }
}
