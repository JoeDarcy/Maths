using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonLauncher : MonoBehaviour {
    //publicly modifiable variables
    public float launchVelocity = 10f; //the launch velocity of the projectile
    public float launchAngle = 30f;    //the angle the projectile is fired at
    public float Gravity = -9.8f;      //the gravity that effects the projectiles

    public Vec3 v3InitialVelocity = new Vec3(); //launch velocity as a vector
    public Vec3 v3CurrentVelocity = new Vec3(); //current velocity of projectile
    public Vec3 v3Acceleration;                 //vector quantity for acceleration

    private float airTime = 0f;         //how long will the projectile be in the air
    private float xDisplacement = 0f;   //how far in the horizontal plane will the projectile travel?

    private bool simulate = false;

    //variables that relate to drawing the path of the projectile
    private List<Vec3> pathPoints;      //list of points along the path of the vector for drawing line of travel
    private int simulationSteps = 30;   //number of points on the path of projectile to draw

    // Start is called before the first frame update
    void Start() {
        //initialise path vector for drawing
        pathPoints = new List<Vec3>();
        calculateProjectile();
        calculatePath();
    }

    // Update is called once per frame
    void Update() {

        // Draw path of projectile when not in play mode too
        if (simulate == false) {
            pathPoints = new List<Vec3>();
            calculateProjectile();
            calculatePath();
        }

        drawPath();

        if (Input.GetKeyDown(KeyCode.Space) && simulate == false) {

            simulate = true;
            v3CurrentVelocity = v3InitialVelocity;
        }

        if (Input.GetKeyDown(KeyCode.R)) {

            simulate = false;
            transform.position = new Vector3(0f,0f,0f);
        }
    }

    private void calculateProjectile() {

        //work out velocity as a vector quanity
        v3InitialVelocity.x = launchVelocity * Mathf.Cos(launchAngle * Mathf.Deg2Rad);
        v3InitialVelocity.y = launchVelocity * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

        //gravity as a vec3
        v3Acceleration = new Vec3(0f, Gravity, 0f);

        //calculate total air time
        float finalYVel = 0f;
        airTime = 2f * (finalYVel - v3InitialVelocity.y) / v3Acceleration.y;

        //calculate the total distance travelled in x
        xDisplacement = airTime * v3InitialVelocity.x;

    }

    private void calculatePath() {
        Vec3 launchPos = new Vec3(transform.position);
        pathPoints.Add(launchPos);

        for(int i = 0; i <= simulationSteps; i++) {

            float simTime = (i / (float)simulationSteps) * airTime;

            //suvat formular for displacement s = ut + 1/2at^2
            Vec3 displacement = v3InitialVelocity * simTime + v3Acceleration * simTime * simTime * 0.5f;
            Vec3 drawPoint = launchPos + displacement;

            pathPoints.Add(drawPoint);
        }
    }

    void drawPath() {

        for (int i = 0; i < pathPoints.Count - 1; i++) {

            Debug.DrawLine(pathPoints[i].ToVector3(), pathPoints[i + 1].ToVector3(), Color.green);
        }
    }


    private void FixedUpdate() {

        if (simulate) {

            Vec3 currentPos = new Vec3(transform.position); 

            //work out current velocity
            v3CurrentVelocity += v3Acceleration * Time.deltaTime;

            //work out displacement
            Vec3 displacement = v3CurrentVelocity * Time.deltaTime;
            currentPos += displacement;
            transform.position = currentPos.ToVector3();

        }
    }

}
