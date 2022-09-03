using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLegsController : MonoBehaviour
{
    [SerializeField] Transform homesParent;
    [SerializeField] Transform polesParent;
    [SerializeField] Transform[] homes;

    [SerializeField] bool alternateLegs;

    [SerializeField] InverseKinematics leftIk;
    [SerializeField] InverseKinematics rightIk;
    [SerializeField] ProceduralAnimation leftAnim;
    [SerializeField] ProceduralAnimation rightAnim;

    [SerializeField] public Transform body;

    public LayerMask groundMask;

    void Start()
    {
        if (!alternateLegs)
        {
            StartCoroutine(LegUpdate());
        }
        else
        {
            StartCoroutine(AlternatingLegUpdate());
        }
    }

    public void GroundHomeParent()
    {
        homesParent.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        homesParent.eulerAngles =
            new Vector3(homesParent.eulerAngles.x, transform.eulerAngles.y, homesParent.eulerAngles.z);
        //print( Hips.worldCenterOfMass);
        //print(body.localEulerAngles);
        //Vector2 toVector = target.transform.position - transform.position;
        //  float angleToTarget = Vector2.Angle(transform.up, toVector);
        //print(body.transform.localRotation.normalized);

        // homesParent.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        // homesParent.eulerAngles = new Vector3(homesParent.eulerAngles.x, transform.eulerAngles.y, homesParent.eulerAngles.z);

        polesParent.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        polesParent.eulerAngles =
            new Vector3(polesParent.eulerAngles.x, transform.eulerAngles.y, polesParent.eulerAngles.z);
        AdjustHomes();

    }

    public void AdjustHomes()
    {
         RaycastHit hit;
         RaycastHit hit2;
         RaycastHit hit3;
         Ray ray = new Ray(body.position, -body.up);
         Ray ray2 = new Ray(body.position, Vector3.down);
         Vector3 temp = new Vector3(-body.up.x, -body.up.y, body.up.z);
         Ray ray3 = new Ray(body.position, temp);
       // Ray ray3 = new Ray(body.position,-body.);

         float h=0;
         float o=0;
         float o2=0;
         if (Physics.Raycast(ray2, out hit2, 10, groundMask))
         {
             o = Vector3.Distance(ray2.origin, hit2.point);
            
         }
         
         if (Physics.Raycast(ray, out hit, 10, groundMask))
         {
        
             h = Vector3.Distance(ray.origin, hit.point);
        
             if (hit.point.y != hit2.point.y)
             {
                 if (hit.point.y > hit2.point.y)
                 {
                     Vector3 newhitPt = new Vector3(hit.point.x, hit2.point.y, hit.point.z);
                     h = Vector3.Distance(ray.origin, newhitPt);
                 }
                 else
                 {
                     Vector3 newhitPt = new Vector3(hit2.point.x, hit.point.y, hit2.point.z);
                     o = Vector3.Distance(ray.origin, newhitPt);
        
                 }
             }
         }

        if (Physics.Raycast(ray3, out hit3, 10, groundMask))
        {
            o2 = Vector3.Distance(ray3.origin, hit3.point);

            // if (hit.point.y != hit3.point.y)
            // {
            //     if (hit.point.y > hit3.point.y)
            //     {
            //       //  Vector3 newhitPt = new Vector3(hit.point.x, hit3.point.y, hit.point.z);
            //       //  h = Vector3.Distance(ray.origin, newhitPt);
            //     }
            //     else
            //     {
            //        // Vector3 newhitPt = new Vector3(hit3.point.x, hit.point.y, hit3.point.z);
            //        // o2 = Vector3.Distance(ray.origin, newhitPt);
            //
            //     }
            //
            // }

        }

        // Plane plane = new Plane(ray2.origin,hit2.point);
        // Vector3 reflectedPT = ReflectionOverPlane(hit.point, plane);


      float A = Mathf.Asin((Mathf.Min(o, h) / Mathf.Max(o, h)));
     // float B = Mathf.Asin((Mathf.Min(o2, h) / Mathf.Max(o2, h)));
    // float B = Mathf.Asin(o2/h);
        if (A * Mathf.Rad2Deg < 60)
        {
            // homes[0].Translate(homes[0].forward* .5f *  Time.deltaTime,Space.World); 
            // homes[1].Translate(homes[1].forward* .5f *  Time.deltaTime,Space.World); 
        
        }
        else
        {
            //  homes[0].Translate(-homes[0].forward* .04f * Time.deltaTime,Space.World); 
            // homes[1].Translate(-homes[1].forward* .04f * Time.deltaTime,Space.World);    
        }
       // print(B*Mathf.Rad2Deg);
       // print(o2);
       // print(A*Mathf.Rad2Deg);
    }




public Vector3 ReflectionOverPlane(Vector3 point, Plane plane) {
        Vector3 N = transform.TransformDirection(plane.normal);
        return point - 2 * N * Vector3.Dot(point, N) / Vector3.Dot(N, N);
    }

    public void EnableIk()
    {
        rightIk.enabled = true;
        leftIk.enabled = true;
    }
    public void DisableIk()
    {
        rightIk.enabled = false;
        leftIk.enabled = false;
    }

    IEnumerator AlternatingLegUpdate()
    {
        while (true)
        {
            do
            {
                leftAnim.TryMove();
                yield return null;
            } while (leftAnim.moving);

            do
            {
                rightAnim.TryMove();
                yield return null;

            } while (rightAnim.moving);
        }
    }
    IEnumerator LegUpdate()
    {
        while (true)
        {
            do
            {
                leftAnim.TryMove();
                rightAnim.TryMove();
                yield return null;
            } while (leftAnim.moving && rightAnim.moving);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        RaycastHit hit;
        Ray ray = new Ray(body.position,-body.up);
        Ray ray2 = new Ray(body.position,Vector3.down);
        if (Physics.Raycast(ray2, out hit, groundMask))
        {
            Gizmos.DrawLine(ray2.origin, hit.point);
             // Plane plane = new Plane(ray2.origin.normalized,hit.point);
             // Vector3 reflectedPT = ReflectionOverPlane(hit.point, plane);
             // Gizmos.DrawSphere(reflectedPT,1);
        }
        if (Physics.Raycast(ray, out hit, groundMask)) {
            Gizmos.DrawLine(ray.origin,hit.point);
            Vector3 temp = new Vector3(-body.up.x, -body.up.y, body.up.z);
            Ray ray3 = new Ray(body.position,temp);
            Physics.Raycast(ray3, out hit);
            Gizmos.DrawLine(ray3.origin,hit.point);
        }
    }
}


