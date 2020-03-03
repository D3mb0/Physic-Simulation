using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNode 
{
    public CNode()
    {
        faceCount = 0;
    }

    public float mMass = 0.0f;
    public Vector3 mPosition;
    public Vector3 mVelocity;
    public Vector3 mForce = Vector3.zero;
    public Vector3 Gravity = new Vector3(0.0f, -9.81f, 0.0f);
    public Vector3 Normal;
    public int faceCount;
    public bool SimulationFlag = true;

    public void update(float dt)
    {
        if (SimulationFlag)
        {
            mVelocity += (mForce / mMass + Gravity) * dt;   // V(t+1) = V(t) + (F(t) / mass) * h 	
            mPosition += mVelocity * dt;                    // X(t+1) = X(t) + V(t) * h	
        }
    }
    public void AddForce(Vector3 Force)
    {
        mForce += Force;
    }
    public void setPos(Vector3 Pos)
    {
        mPosition = Pos;
    }
    public void ClearForce()
    {
        mForce = new Vector3(0f, 0f, 0f);
    }
    public Vector3 GetForce()
    {
        return mForce;
    }
    public Vector3 GetVel()
    {
        return mVelocity;
    }
    public Vector3 GetPos()
    {
        return mPosition;
    }
}
