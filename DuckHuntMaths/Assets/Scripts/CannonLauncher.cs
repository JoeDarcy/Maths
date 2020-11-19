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

    // Start is called before the first frame update
    void Start() {
        calculateProjectile();
    }

    // Update is called once per frame
    void Update() {

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

    private void FixedUpdate() {

        if (simulate) {

            Vec3 currentPos = new Vec3(transform.position); //issue here with Vec3 ------------

            //work out current velocity
            v3CurrentVelocity += v3Acceleration * Time.deltaTime;

            //work out displacement
            Vec3 displacement = v3CurrentVelocity * Time.deltaTime;
            currentPos += displacement;
            transform.position = currentPos.ToVector3();

        }
    }

}
