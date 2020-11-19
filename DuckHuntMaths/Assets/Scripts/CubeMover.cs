using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    //axis (basis) vectors for cube
    private Vec3 xAxis = new Vec3( 1f, 0f, 0f);
    private Vec3 yAxis = new Vec3( 0f, 1f, 0f);

    private Vec3 position = new Vec3( 0f, 0f, 0f);

    public Vec3 Velocity = new Vec3( 0f, 0f, 0f);

    public Vec3 Acceleration = new Vec3(0f, 12f, 7f);

    public float duration = 1.0f;
    private bool renewDuration = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //draws basic lines along our defined axis
    private void OnDrawGizmos() {
        //Draw X Axis in Red
       Gizmos.color = Color.red;
       Gizmos.DrawLine(position.ToVector3(), (position + (xAxis * 3) ).ToVector3());

       //Draw Y Axis in Green
       Gizmos.color = Color.green;
       Gizmos.DrawLine(position.ToVector3(), (position + (yAxis * 3) ).ToVector3());

       //Produce Z Axis from CrossProduct of X Axis and Y Axis and Colour it Blue
       Gizmos.color = Color.blue;
       Vec3 zAxis = Vec3.CrossProduct(xAxis, yAxis); 

       //axis produced should be perpendicular to plane created by vectors xAxis and yAxis 
       Gizmos.DrawLine(position.ToVector3(), (position + (zAxis * 3) ).ToVector3());

       //draw velocity vector as a line
       Gizmos.color = Color.white;
       Gizmos.DrawLine(position.ToVector3(), (position + Velocity).ToVector3());
       
    }

    private Vec3 calculateDisplacement( Vec3 a_velocity, float a_deltaTime){
        return a_velocity * a_deltaTime;
    }

    private Vec3 calculatePosition( Vec3 a_initPos, Vec3 a_velocity, float a_deltaTime){
        return a_initPos + calculateDisplacement(a_velocity, a_deltaTime);
    }

    private Vec3 calculateAverageVelocity( Vec3 a_initPos, Vec3 a_finalPos, float a_deltaTime){
        if( a_deltaTime > 0f){
            return (a_finalPos - a_initPos) / a_deltaTime; 
        }
        return new Vec3(0f, 0f, 0f);
    }

    private Vec3 calulateAcceleration( Vec3 a_initVel, Vec3 a_finalVel, float a_deltaTime){
        if( a_deltaTime > 0f){
            return (a_finalVel - a_initVel) / a_deltaTime;
        }
        return new Vec3( 0f, 0f, 0f);
    }

    private Vec3 calculateVelocity( Vec3 a_acceleration, float a_deltaTime){
        return a_acceleration * a_deltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        
        if( position.y < 0f)
        {
            Velocity = new Vec3(0f, 0f, 0f);
            Acceleration = new Vec3(0f, 0f, 0f);
        }
        //calculate velocity based off accelleration
        Velocity += calculateVelocity( Acceleration, Time.deltaTime);
        //change in displacement is velocity multiplied by time
        //position + change in displacement is the new position
        Vec3 displacement = calculateDisplacement(Velocity , Time.deltaTime);
        Debug.Log(displacement.ToString());
        position += displacement;
        //set positon variable back into transform.position
        transform.position = position.ToVector3();
    
        if( duration < 0f && renewDuration ){
            Acceleration = new Vec3(0f, -9.8f, 0f);
            renewDuration = false;
        }
        duration -= Time.deltaTime;
        
    }
}
