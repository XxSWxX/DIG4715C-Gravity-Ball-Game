using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public GameObject level;
    public GameObject ball;
    public GameObject monky;

    // Start is called before the first frame update
    void Start()
    {

        //for every celestial body in the level, create a gameobject joined to the golfball with a ConstantForce2D
        foreach (Transform child in level.transform.GetComponentsInChildren<Transform>())
        {
            if (child.tag == "CelestialBody")
            {


                GameObject objToSpawn = new GameObject(child.transform.name); //same name as celestial body so it's easier to reference later

                objToSpawn.transform.parent = transform;

                Rigidbody2D RB2D = objToSpawn.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
                RB2D.mass = 0; // these force objects need minimum mass or the joint will wobble
                RB2D.gravityScale = 0;

                FixedJoint2D joint = gameObject.AddComponent(typeof(FixedJoint2D)) as FixedJoint2D;
                joint.connectedBody = RB2D;
                joint.frequency = 20;

                ConstantForce2D CF = objToSpawn.AddComponent(typeof(ConstantForce2D)) as ConstantForce2D;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {


        //get the direction vector from the ball to each celestial body and apply it to the corresponding constantforce2Ds. As ball get's closer, increase the power multiplier.
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (Transform child in level.transform.GetComponentsInChildren<Transform>())
        {
            if (child.tag == "CelestialBody")
            {

                Vector3 gravityVector = (child.transform.position - transform.position).normalized;
                float distancePower = (child.transform.position - transform.position).magnitude;
                float massMulti = child.GetComponent<Rigidbody2D>().mass;
                float maxDist = child.transform.localScale.x * 2;
                if (distancePower > maxDist)
                {
                    distancePower = maxDist;
                }
                GameObject ballGravityForce = ball.transform.Find(child.transform.name).gameObject; //the force gameobject for this planet in the foreach loop
                ConstantForce2D gravityForce = ballGravityForce.GetComponent<ConstantForce2D>();

                gravityForce.force = gravityVector * Mathf.Abs(distancePower - maxDist) * massMulti;

                // set Monky rotation to closest planet
                float dist = Vector3.Distance(child.transform.position,currentPos)-(child.transform.localScale.x);
                if (dist < minDist)
                {
                    tMin = child;
                    minDist = dist;
                }
                Vector3 targetDirection = monky.transform.position - tMin.transform.position;
                float angle = (Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg)-90;

                monky.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


            }
        }

        if (Input.GetMouseButtonDown(0)) //placeholder for possible thrust feature
        {
            
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(1000,1000));

            // GetComponent<Rigidbody2D>().AddForce(new Vector2(200000,200000), ForceMode.Impulse);
        }
    }

}
