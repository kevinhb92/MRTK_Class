using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class myEventTriggerOnEnter : MonoBehaviour
{
    [Header("Custom Event")]

    public UnityEvent myEvents;

    private void OnTriggerEnter(Collider other)
    {
        if(myEvents == null)
        {
            print("myEventTriggerOnEnter was triggered but myEbents was null.");
            return;
        }
        
        print("myEventTriggerOnEnter Activated. Triggering" + myEvents);
        myEvents.Invoke();
    }

    

}
