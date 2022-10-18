using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Behavior
{
    b_Evade,
    b_OffsetPursuit,
    b_FindFood,
    b_HideOnMom,
    b_Stop,
    b_Wander
}

public class Maneuver : MonoBehaviour
{
    public GameObject m_LeaderActor;
    public GameObject m_Person;
    public GameObject m_Food;
    public float MaxSpeed;
    public Behavior behavior;
    private Vector3 FirstPos;
    public float threatRange;

    Rigidbody LA_rgd;
    Rigidbody T_rgd;
    Rigidbody rgd;
    // Start is called before the first frame update
    void Start()
    {
        rgd = this.GetComponent<Rigidbody>();
        LA_rgd = m_LeaderActor.GetComponent<Rigidbody>();
        T_rgd = m_Food.GetComponent<Rigidbody>();
        FirstPos = m_LeaderActor.transform.InverseTransformPoint(this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        switch (behavior)
        {
            case Behavior.b_FindFood:
                rgd.AddForce(Arrive(m_Food.transform.position, MaxSpeed));
                break;
            case Behavior.b_OffsetPursuit:
                rgd.AddForce(OffsetPursuit(FirstPos, MaxSpeed));
                break;
            case Behavior.b_Evade:
                rgd.AddForce(Flee(m_Person.transform.position, MaxSpeed));
                break;
            case Behavior.b_HideOnMom:
                rgd.AddForce(Arrive(m_LeaderActor.transform.position, MaxSpeed));
                break;
            case Behavior.b_Stop:
                rgd.AddForce(new Vector3(0, 0, 0));
                break;
           
        }
    }
    public Vector3 Arrive(Vector3 targetPos, float MaxSpeed)
    {
        float distance = (targetPos - this.transform.position).magnitude;

        if (distance > 0)
        {
            const float DecelerationTweaker = 0.3f;
            float speed = distance / DecelerationTweaker;
            speed = Mathf.Min(speed, MaxSpeed);
            Vector3 desireVelocity = (targetPos - this.transform.position) / distance * speed;
            return (desireVelocity - rgd.velocity);
        }
        return new Vector3(0, 0);
    }
    public Vector3 OffsetPursuit(Vector3 Firstpos, float MaxSpeed)
    {
        Vector3 offsetPos = LA_rgd.transform.TransformPoint(Firstpos);      //로컬좌표에서의 액터 위치를 월드좌표에서 정의
        Vector3 toOffset = this.transform.position - offsetPos;
        float arriveTime = toOffset.magnitude / (MaxSpeed + LA_rgd.velocity.magnitude);
        return Arrive(offsetPos + LA_rgd.velocity * arriveTime, MaxSpeed);
    }

    public Vector3 Flee(Vector3 targetPos, float MaxSpeed)
    {
        Vector3 desireVelocity = (this.transform.position - targetPos).normalized * MaxSpeed;
        
        Vector3 fleeVec = this.transform.position - targetPos;
        
        if (fleeVec.sqrMagnitude > threatRange * threatRange)
        {
            return new Vector3(0, 0);
        }
        return (desireVelocity - rgd.velocity);
    }

    public Vector3 Evade(GameObject target, float MaxSpeed)
    {
        Rigidbody T_rgd = target.GetComponent<Rigidbody>();
        Vector3 fleeVec = this.transform.position - target.transform.position;
        float LookAheadTime = fleeVec.magnitude / (MaxSpeed + rgd.velocity.magnitude);
        if (fleeVec.sqrMagnitude > threatRange * threatRange)
        {
            return new Vector3(0, 0);
        }

        return Flee(target.transform.position + T_rgd.velocity * LookAheadTime, MaxSpeed);
    }
  
}
