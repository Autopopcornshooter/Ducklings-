using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MomDuckCtrl : MonoBehaviour
{

    public GameObject player;
    public GameObject Eagle;
    public GameObject[] Babies;
    public Maneuver maneuver;
    public float countRange;
    Rigidbody rgd;
    private float angle;
    private MomDuckCtrl m_instance;
    private m_State current;
    private m_State prev;
    public EventManager eventManager;
    public SpriteRenderer sRender;
    // Start is called before the first frame update
    void Start()
    {
        sRender = this.GetComponent<SpriteRenderer>();
            Babies = GameObject.FindGameObjectsWithTag("Babies");
        eventManager = new EventManager().Instance();
        rgd = this.GetComponent<Rigidbody>();
        maneuver = this.GetComponent<Maneuver>();
        changeState(new FollowFood());
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position - this.transform.position).sqrMagnitude < countRange * countRange)
        {
            if(current!=new m_Flee())
            {
                changeState(new m_Flee());
            }
        }
        current.Execute();

        angle = Mathf.Atan2(rgd.velocity.y, rgd.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void changeState(m_State next)
    {
        prev = current;
        if (current != null)
        {
            current.OnExit();
        }
        current = next;
        current.OnEnter(this);
    }

    public m_State getCurrent()
    {
        return current;
    }

}

public interface m_State
{
    void OnEnter(MomDuckCtrl instance);
    void Execute();
    void OnExit();
    void OnEvent(EventManager message);
}

public class FollowFood:m_State
{
    private MomDuckCtrl p_instance;
    void m_State.OnEnter(MomDuckCtrl instance)
    {
        Debug.Log("OnEnter");
        p_instance = instance;
        p_instance.maneuver.behavior = Behavior.b_FindFood;
    }
    void m_State.Execute()
    {
        if ((p_instance.Eagle.transform.position - p_instance.transform.position).sqrMagnitude < p_instance.countRange * p_instance.countRange)
        {
            p_instance.changeState(new HideBaby());
            // Baby에게 이벤트 send
            EventManager babyHide;
            babyHide = new EventManager();
            babyHide.e_message = EventList.e_eagleClose;
         
            for (int i = 0; i < p_instance.Babies.Length; i++)
            {
                babyHide.e_receiver = p_instance.Babies[i];
                p_instance.eventManager.DispatchMessage(babyHide);
            }

        }
        
    }
    void m_State.OnExit()
    {

    }
    void m_State.OnEvent(EventManager message)
    {
        if (message.e_message == EventList.e_babyCry)
        {
            p_instance.changeState(new Stop());
        }
    }
}
public class m_Flee : m_State
{
    private MomDuckCtrl p_instance;
    void m_State.OnEnter(MomDuckCtrl instance)
    {
        p_instance = instance;
        p_instance.maneuver.behavior = Behavior.b_Evade;
    }
    void m_State.Execute()
    {
        if ((p_instance.player.transform.position - p_instance.transform.position).sqrMagnitude > p_instance.countRange * p_instance.countRange)
        {
            p_instance.changeState(new FollowFood());
        }
        Debug.Log("실행 중");
    }
    void m_State.OnExit()
    {

    }
    void m_State.OnEvent(EventManager message)
    {

    }
}
public class HideBaby : m_State
{
    private MomDuckCtrl p_instance;
    void m_State.OnEnter(MomDuckCtrl instance)
    {
        p_instance = instance;
        p_instance.maneuver.MaxSpeed = p_instance.maneuver.MaxSpeed/2;
    }
    void m_State.Execute()
    {
        p_instance.sRender.color = new Color(0.5f, 0.5f, 0.5f, 1);
        if ((p_instance.Eagle.transform.position - p_instance.transform.position).sqrMagnitude >= p_instance.countRange * p_instance.countRange)
        {
            p_instance.changeState(new FollowFood());
            // Baby에게 이벤트 send
            EventManager babySafe;
            babySafe = new EventManager();
            babySafe.e_message = EventList.e_eagleFar;
            for (int i = 0; i < p_instance.Babies.Length; i++)
            {
                babySafe.e_receiver = p_instance.Babies[i];
                p_instance.eventManager.DispatchMessage(babySafe);
            }
        }
    }
    void m_State.OnExit()
    {
        p_instance.sRender.color = new Color(1,1,1, 1);
        p_instance.maneuver.MaxSpeed *= 2;
    }
    void m_State.OnEvent(EventManager message)
    {

    }
}
public class Stop : m_State
{
    private MomDuckCtrl p_instance;
    void m_State.OnEnter(MomDuckCtrl instance)
    {
        p_instance = instance;
        p_instance.maneuver.behavior = Behavior.b_Stop;
    }
    void m_State.Execute()
    {
        for(int i = 0; i < p_instance.Babies.Length; i++)
        {
            if((p_instance.Babies[i].transform.position-p_instance.transform.position).sqrMagnitude<(p_instance.countRange/2)* (p_instance.countRange / 2))
            {
                p_instance.changeState(new FollowFood());
            }
        }
    }
    void m_State.OnExit()
    {

    }
    void m_State.OnEvent(EventManager message)
    {

    }
}