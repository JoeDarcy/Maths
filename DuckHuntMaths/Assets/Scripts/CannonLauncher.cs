using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonLauncher : MonoBehaviour {
    // Timer for firing
    private float timer = 3;

    //publicly modifiable variables
    public float launchVelocity = 10f; //the launch velocity of the projectile
    public float launchAngle = 30f;    //the angle the projectile is fired at
    public float Gravity = -9.8f;      //the gravity that effects the projectiles

    public Vec3 v3InitialVelocity = new Vec3(); //launch velocity as a vector
    //public Vec3 v3CurrentVelocity = new Vec3(); //current velocity of projectile
    private Vec3 v3Acceleration;                 //vector quantity for acceleration

    private float airTime = 0f;         //how long will the projectile be in the air
    private float horizontalDisplacement = 0f;   //how far in the horizontal plane will the projectile travel?

    //variables that relate to drawing the path of the projectile
    private List<Vec3> pathPoints;      //list of points along the path of the vector for drawing line of travel
    private int simulationSteps = 30;   //number of points on the path of projectile to draw

    public GameObject projectile;       // Game object to instantiate for pthe projectile
    public GameObject launchPoint;      // Game object to use as the launch point

    private bool simulate = false;

    // Start is called before the first frame update
    void Start() {
        //initialise path vector for drawing
        pathPoints = new List<Vec3>();
        calculateProjectile();
        calculatePath();
    }

    private void calculateProjectile() {

        launchAngle = transform.parent.eulerAngles.x;

        // Work out the vertical offset 
        float launchHeight = launchPoint.transform.position.y;

        // Work out velocity as a vector quanity
        // The velocity is calculated here from the perspective of the cannon.
        // In the cannon model that this script is attached to the cannon faces down it's -z axis 
        v3InitialVelocity.x = 0f;       // Set x value to 0
        v3InitialVelocity.z = launchVelocity * Mathf.Cos(launchAngle * Mathf.Deg2Rad);
        v3InitialVelocity.y = launchVelocity * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

        // v3Velocity is in local space facing down the cannon's -z axis.
        // Transform that into a world space direction if this step is ommitted 
        // the projectile will always move in the worlds -z axis.
        Vector3 txDirection = launchPoint.transform.TransformDirection(v3InitialVelocity.ToVector3());
        v3InitialVelocity = new Vec3(txDirection);

        //gravity as a vec3
        v3Acceleration = new Vec3(0f, Gravity, 0f);

        //calculate total air time
        // Use quadratic equation to find the air time
        airTime = UseQuadraticFormula(v3Acceleration.y, v3InitialVelocity.y * 2f, launchHeight * 2f);

        //float finalYVel = 0f;
        //airTime = 2f * (finalYVel - v3InitialVelocity.y) / v3Acceleration.y;

        //calculate the total distance travelled in x
        horizontalDisplacement = airTime * v3InitialVelocity.x;

    }

    float UseQuadraticFormula(float a, float b, float c) {

        // If A is nearly 0 then the formula doesn't really hold true
        if (0.0001f > Mathf.Abs(a)) {
            return 0f;
        }

        float bb = b * b;
        float ac = a * c;
        float b4ac = Mathf.Sqrt(bb - 4f * ac);
        float t1 = (-b + b4ac) / (2f * a);
        float t2 = (-b - b4ac) / (2f * a);
        float t = Mathf.Max(t1, t2); // Only return the highest value as one of these may be negative
        return t;
    }

    private void calculatePath() {

        Vec3 launchPos = new Vec3(launchPoint.transform.position);
        pathPoints.Add(launchPos);

        for (int i = 0; i <= simulationSteps; i++) {

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

    // Update is called once per frame
    void Update() {

        // Draw path of projectile when not in play mode too
        if (simulate == false) {
            
        }

        drawPath();

        if (Input.GetKeyDown(KeyCode.Space) && simulate == false) {

            
        }

        cannonFiringTimed();

        if (Input.GetKeyDown(KeyCode.R)) {

            simulate = false;
            transform.position = new Vector3(0f, 0f, 0f);
        }
    }

    private void cannonFiringTimed() {
        timer -= Time.deltaTime;

        if (timer <= 0) {
            simulate = true;
            //v3CurrentVelocity = v3InitialVelocity;

            pathPoints.Clear();
            calculateProjectile();
            calculatePath();

            // Instantiate at the launch point position, with the current rotation
            GameObject p = Instantiate(projectile, launchPoint.transform.position, launchPoint.transform.rotation);
            p.GetComponent<Projectile>().SetVelocity(v3InitialVelocity);
            p.GetComponent<Projectile>().SetAcceleration(v3Acceleration);
            p.GetComponent<Projectile>().SetLifeTime(airTime);

            timer = 3f;
        }
    }

    /*
    public void FireProjectile ( Vec3 direction, float a_lifeSpan) {

        transform.rotation = Quaternion.LookRotation(direction.ToVector3());

        pathPoints.Clear();
        calculateProjectile();
        calculatePath();

        // Instantiate at the launch point position, with the current rotation
        GameObject p = Instantiate(projectile, launchPoint.transform.position, launchPoint.transform.rotation);
        p.GetComponent<Projectile>().Velocity = v3InitialVelocity;
        p.GetComponent<Projectile>().Acceleration = v3Acceleration;
        p.GetComponent<Projectile>().LifeSpan = a_lifeSpan;
    }
    */
}
