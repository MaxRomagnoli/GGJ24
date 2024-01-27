using UnityEngine;
using UnityEngine.Events;

public class Triggerator : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToTrigger = 2.5f;
    [SerializeField] private UnityEvent eventsOnTriggerEnter;
    [SerializeField] private UnityEvent eventsOnTriggerExit;

    private bool entered = false;

    private void FixedUpdate()
    {
        Vector3 distance = target.transform.position - this.transform.position;
        if(!entered && distance.sqrMagnitude <= distanceToTrigger) {
            entered = true;
            Entered();
        }
        else if(entered && distance.sqrMagnitude > distanceToTrigger) {
            entered = false;
            Exited();
        }
    }

    private void Entered()
    {
        eventsOnTriggerEnter.Invoke();
    }

    private void Exited()
    {
        eventsOnTriggerExit.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, distanceToTrigger);
    }
}