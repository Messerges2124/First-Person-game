using System;
using System.ComponentModel;
using DitzelGames.FastIK;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class IkEnemy : MonoBehaviour
{
    public LayerMask whatIsGround;

    public float heightAboveGround;


    public FastIK[] legs;
  //  public BoxCollider leg1;
  //  public BoxCollider leg2;
    
    private Transform[] legTargets;
    private Vector3[] targetPositions, currentPositions;
    public Vector3 legTargetOffset ;
    public Vector3 legTargetOffset2 ;
    public Vector3[] legTargetOffsets = new Vector3[2];
    private float foottimer =0f;
    private Transform[] groundChecks;
    public float groundCheckRadius = .1f;
    public Transform root;
    public Rigidbody headrb;
    private float thresholdDistance;
    private float[] legProgress;
    private Rigidbody rb;
    private bool hit = false;
  //  private RigidEnemy re;
    
   // private IkEnemy ik;
    
    // Start is called before the first frame update
    void Start(){
        //ik = GetComponent<IkEnemy>();
     //   re = new RigidEnemy();
        legTargets = new Transform[legs.Length];
      targetPositions = new Vector3[legs.Length];
      currentPositions=new Vector3[legs.Length];
      legProgress = new float[legs.Length];
      legTargetOffsets[0] = legTargetOffset;
      legTargetOffsets[1] = legTargetOffset2;
     // groundChecks =new Transform[legs.Length];
        
      // for (int i = 0; i < legs.Length; i++)
      // {
      //     groundChecks[i] = (ik.legs[i].transform);
      // }
        InitLegTargets();
        if (heightAboveGround == 0f)
        {
            heightAboveGround = legs[0].ChainLength;
            thresholdDistance = 1f; //heightAboveGround;
        }
        else
        {
            thresholdDistance = 0.5f;
        }

      
        UpdateLegTargets();
       UpdateCurrentLegPosition(0);
        UpdateCurrentLegPosition(1);
    }
    

    // Update is called once per frame
    void Update()
    {
   
       
            setoffsetsandmoveenemy();
            UpdateLegTargets();
            UpdateCurrentLegPositions();
            LerpLegs();
      
      

    }

    void setoffsetsandmoveenemy()
    {
      

        // if (headrb.velocity.magnitude > 1)
        // {
         //    currentSpeed = 1f;
            // print("fast");
        // }
        // else if (headrb.velocity.magnitude<-1 )
        // {
         //   currentSpeed = -1f;
       //  }
        // else
       //  {
         //    currentSpeed = 0;
       //  }
       // currentSpeed = headrb.velocity.x;
        //  currentSpeed = re.speed / 15;

        // Vector3 temp = new Vector3(1, 0, -1);
        // legTargetOffsets[0] = temp;
        // legTargetOffsets[1] = temp;
        // if()
        // Vector3 temp = new Vector3(0, 0, -1);
        //legTargetOffset = temp;
        // legTargetOffset2 = temp;
        //  legTargetOffsets[0] = temp;
        //legTargetOffsets[1] = temp;

        // for (int i = 0; i < legs.Length; i++)
        // {
        //     if (Physics.CheckSphere(groundChecks[i].position, groundCheckRadius, ik.whatIsGround))
        //     {
        //        
        //         //Headrb.AddForce(Vector3.left*speed);  
        //     }
        //
        // }
    }

    void InitLegTargets()
    {
        for (int i = 0; i < legs.Length; i++)
        {
            int chainLength = legs[i].ChainLength;
            Transform chainRoot = legs[i].transform;
            while (chainLength > 0)
            {
                chainRoot = chainRoot.parent;
                chainLength--;
            }

            legTargets[i] = chainRoot;
        }
    }

   void UpdateLegTargets()
   {
       for (int i = 0; i < legTargets.Length; i++){
           Vector3 raycastPoint = legTargets[i].position  + legTargetOffset + (root.forward* currentSpeed);
           
           RaycastHit hit;
           if (Physics.Raycast(raycastPoint, Vector3.down, out hit, 50f, whatIsGround))
           {
               targetPositions[i] = hit.point;
           }
       }
   }
   
   void UpdateCurrentLegPositions()
   {
      
       for (int i = 0; i < legs.Length; i++)
       {
           if (!OppositeLegGounded(i))return ;
           
           
            if (CheckDistanceFromTargetPoint(i) > thresholdDistance)
            {
                print("moved leg"+ i);
              UpdateCurrentLegPosition(i);
            }
          
       }
   }

   bool OppositeLegGounded(int leg)
   {
       int otherLeg = (leg + 1) % (legs.Length);
       return legProgress[otherLeg] < 0.01f;
   }

   float CheckDistanceFromTargetPoint(int leg)
   {
       return Vector3.Distance(currentPositions[leg], targetPositions[leg]);
   }

   void UpdateCurrentLegPosition(int leg)
   {
       currentPositions[leg] = targetPositions[leg];
       legProgress[leg] = 1;
   }

   private float legSpeed = 10f;
   private float currentSpeed =1f;

   void LerpLegs()
   {
       for (int i = 0; i < legs.Length; i++)
       {
           Transform legTarget = legs[i].Target;

           legProgress[i] = Mathf.Lerp(legProgress[i], 0, Time.deltaTime * legSpeed);
           Vector3 offset = Vector3.up *2 * legProgress[i];
           legTarget.position = Vector3.Lerp(legTarget.position, currentPositions[i] + offset, Time.deltaTime*legSpeed);

       }
   }

   // private void OnDrawGizmos()
   // {
   //     for (int i = 0; i < legTargets.Length; i++)
   //     {
   //         Gizmos.color = Color.red;
   //         Gizmos.DrawWireSphere(targetPositions[i],0.15f);
   //         
   //         Gizmos.color = Color.cyan;
   //         Gizmos.DrawLine(targetPositions[i],targetPositions[i]);
   //
   //     }
   // }
}
