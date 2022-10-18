using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDuckCtrl : MonoBehaviour
{

    public GameObject player;
    public GameObject Mom;
    public Maneuver maneuver;
    public float countRange;
    Rigidbody rgd;
    private float angle;
    private BabyDuckCtrl b_instance;
    private b_State current;
    private b_State prev;
    public EventManager eventManager;
    public SpriteRenderer sRender;
    // Start is called before the first frame update
    void Start()
    {
        sRender = this.GetComponent<SpriteRenderer>();
        eventManager = new EventManager().Instance();
        rgd = this.GetComponent<Rigidbody>();
        maneuver = this.GetComponent<Maneuver>();
        changeState(new FollowMom());

    }
    void Update()
    {
        if ((player.transform.position - this.transform.position).sqrMagnitude < countRange * countRange)
        {
            if (current != new b_Flee())
            {
                changeState(new b_Flee());
            }
        }
        current.Execute();



        angle = Mathf.Atan2(rgd.velocity.y, rgd.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void changeState(b_State next)
    {
        prev = current;
        if (current != null)
        {
            current.OnExit();
        }
        current = next;
        current.OnEnter(this);
    }

    public b_State getCurrent()
    {
        return current;
    }
  
}

public interface b_State
{
    void OnEnter(BabyDuckCtrl instance);
    void Execute();
    void OnExit();
    void OnEvent(EventManager message);
}


public class FollowMom : b_State
{
    private BabyDuckCtrl p_instance;
    void b_State.OnEnter(BabyDuckCtrl instance)
    {
        Debug.Log("OnEnter");
        p_instance = instance;
        p_instance.maneuver.behavior = Behavior.b_OffsetPursuit;
    }
    void b_State.Execute()
    {
        if ((p_instance.transform.position - p_instance.Mom.transform.position).sqrMagnitude > p_instance.countRange * p_instance.countRange)
        {
            p_instance.sRender.color = new Color(0.5f, 0.5f, 0.5f, 1);
            //Mom 에게 cry 이벤트 전달
            EventManager babyCry;
            babyCry = new EventManager();
            babyCry.e_message = EventList.e_babyCry;
            babyCry.e_receiver = p_instance.Mom;
            p_instance.eventManager.DispatchMessage(babyCry);
        }
        else
        {
            p_instance.sRender.color = new Color(1,1,1, 1);

        }
    }
    void b_State.OnExit()
    {

    }
    void b_State.OnEvent(EventManager message)
    {
        if (message.e_message == EventList.e_eagleClose)
        {
            p_instance.changeState(new HideOnMom());
        }
    }
}

public class HideOnMom : b_State
{
    private BabyDuckCtrl p_instance;
    void b_State.OnEnter(BabyDuckCtrl instance)
    {
        Debug.Log("OnEnter");
        p_instance = instance;
        p_instance.maneuver.MaxSpeed *= 3;
        p_instance.maneuver.behavior = Behavior.b_HideOnMom;
    }
    void b_State.Execute()
    {

    }
    void b_State.OnExit()
    {
        p_instance.maneuver.MaxSpeed = p_instance.maneuver.MaxSpeed / 3;
    }
    void b_State.OnEvent(EventManager message)
    {
        if (message.e_message == EventList.e_eagleFar)
        {
            p_instance.changeState(new FollowMom());
        }
    }
}
public class b_Flee : b_State
{
    private BabyDuckCtrl p_instance;
    void b_State.OnEnter(BabyDuckCtrl instance)
    {
        Debug.Log("OnEnter");
        p_instance = instance;
        p_instance.maneuver.behavior = Behavior.b_Evade;
    }
    void b_State.Execute()
    {
        if ((p_instance.player.transform.position - p_instance.transform.position).sqrMagnitude > p_instance.countRange * p_instance.countRange)
        {
            p_instance.changeState(new FollowMom());
        }
    }
    void b_State.OnExit()
    {

    }
    void b_State.OnEvent(EventManager message)
    {

    }
}