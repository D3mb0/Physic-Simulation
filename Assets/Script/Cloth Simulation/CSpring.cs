using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSpring 
{
    public CSpring()
    {

    }
    public CNode m1;
    public CNode m2;
    public float mRestLegnth;
    public float Ks,Kd;
    public int type; // 0 : structural spring , 1 shear spring , 2 bending spring


    float Rounding(float x, int digit)
    {
        return (float)Math.Round(x, digit);
    }
    public void init(CNode a, CNode b)
    {
        m1 = a;
        m2 = b;

        Vector3 forceDirection = m2.GetPos() - m1.GetPos();
        mRestLegnth = forceDirection.magnitude;

    }
    public void ComputeSpringForce()
    {
        Vector3 forceDirection = m2.GetPos() - m1.GetPos();
        Vector3 velocityDirection = m2.GetVel() - m1.GetVel();

        float len = forceDirection.magnitude;


        len = Rounding(len, 4);
        float spforce = (float)((len - mRestLegnth) * Ks);
        float damp = (float)((Vector3.Dot(velocityDirection, forceDirection) / len) * Kd);

        Vector3 nSPF = ((spforce + damp) * forceDirection) / len;

        m1.AddForce(nSPF);
        m2.AddForce(-nSPF);

        //m1.AddForce(Vector3.zero);
        //m2.AddForce(Vector3.zero);
    }
    public void update()
    {
        ComputeSpringForce();
    }
    
}
