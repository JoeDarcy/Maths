using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vec3 v3CurrentVelocity = new Vec3(0f, 0f, 0f);      // Launch velocity as a vector
    private Vec3 v3Acceleration = new Vec3(0f, -9.8f, 0f);      // Vector quantity for acceleration

    private float m_lifeSpan = 0f;

    public void SetVelocity(Vec3 a_vel) {
        v3CurrentVelocity = a_vel;
	}

    public void SetAcceleration ( Vec3 a_accel) {
        v3Acceleration = a_accel;
	}

    public void SetLifeTime ( float a_lifespan) {
        m_lifeSpan = a_lifespan;
	}

	private void FixedUpdate() {

        m_lifeSpan -= Time.deltaTime;

        Vec3 currentPos = new Vec3(transform.position);

        // Work out current Velocity
        v3CurrentVelocity += v3Acceleration * Time.deltaTime;

        // Work out displacement
        Vec3 displacement = v3CurrentVelocity * Time.deltaTime;
        currentPos += displacement;
        transform.position = currentPos.ToVector3();

        if (m_lifeSpan < 0f) {
            Destroy(gameObject);
		}
	}
}
