using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGravity : MonoBehaviour
{
    public float mMass = 1.0f;
    Vector3 mPosition;
    Vector3 mVelocity;
    Vector3 mForce = Vector3.zero;
    public Vector3 Gravity = new Vector3(0.0f, -9.81998f, 0.0f);
    public float dt = 0.0008f;
   
    // Start is called before the first frame update
    void Start()
    {
        mPosition=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        mVelocity += (mForce / mMass + Gravity) * dt;   // V(t+1) = V(t) + (F(t) / mass) * h 	
        mPosition += mVelocity * dt;                    // X(t+1) = X(t) + V(t) * h	
        transform.position = mPosition;
    }
}
