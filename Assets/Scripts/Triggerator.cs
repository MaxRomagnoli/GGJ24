using UnityEngine;
using UnityEngine.Events;

public class Triggerator : MonoBehaviour
{
    [SerializeField] private string tag;
    [SerializeField] private UnityEvent eventsOnTriggerEnter;
    [SerializeField] private UnityEvent eventsOnTriggerExit;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Triggered " + collision.gameObject.name);
        if(collision.gameObject.tag == tag)
        {
            Debug.Log("Triggered kyle " + collision.gameObject.name);
            eventsOnTriggerEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == tag)
        {
            eventsOnTriggerExit.Invoke();
        }
    }
}