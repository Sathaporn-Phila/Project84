using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleRay
{
    // Start is called before the first frame update
    public Ray m_ray;
    public float prevCosValue;
    public toggleRay(Ray ray,float val){
            m_ray = ray;
            prevCosValue = val;
    }
    public virtual void toggle(float val){
            if(prevCosValue*val<0){
                m_ray.direction = -m_ray.direction;
            }
            prevCosValue = val;
        }
    public void setDirection(Vector3 val){
        m_ray.direction = val;
    }
    public Ray getRay(){
        return m_ray;
    }
}
