using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventList
{
    e_babyCry,
    e_personClose,
    e_eagleClose,
    e_eagleFar
}
public class EventManager { 
    // Start is called before the first frame update
    public  GameObject e_sender;
    public  GameObject e_receiver;
    public  EventList e_message;
    private EventManager p_instance;
    public void DispatchMessage(EventManager sendingMessage)
    {
        GameObject receiver = sendingMessage.e_receiver;
        if (receiver.GetComponent<BabyDuckCtrl>() != null)
        {
            receiver.GetComponent<BabyDuckCtrl>().getCurrent().OnEvent(sendingMessage);
        }
        if (receiver.GetComponent<MomDuckCtrl>() != null)
        {
            receiver.GetComponent<MomDuckCtrl>().getCurrent().OnEvent(sendingMessage);
        }
    }
    public EventManager Instance()
    {
        if (p_instance == null)
        {
            p_instance = new EventManager();
            return p_instance;
        }
        return p_instance;
    }

}

