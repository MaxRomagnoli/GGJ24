using UnityEngine;
using UnityEngine.Events;

public class RaycastSomething : MonoBehaviour {

    [SerializeField] private LayerMask rayMask;
    [SerializeField] private float raycastLenght = 15f;
    [SerializeField] private UnityEvent eventsTriggerEnter;
    [SerializeField] private UnityEvent eventsTriggerExit;
    private GameObject hittedGameObject = null;
    [HideInInspector] public Vector3 hittedPosition; //variables you can call from another script

	void FixedUpdate ()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, raycastLenght, rayMask))
        {
            if (hittedGameObject == null)
            {
                hittedPosition = hit.collider.transform.position;
                Debug.Log("invoke enter");
                hittedGameObject = hit.transform.gameObject;
                eventsTriggerEnter.Invoke();
            }
        }
        else
        {
            if(hittedGameObject != null)
            {
                Debug.Log("invoke exit");
                hittedGameObject = null;
                eventsTriggerExit.Invoke();
            }
        }
    }

    public GameObject GetHittedGameObject()
    {
        return hittedGameObject;
    }

    void OnDrawGizmos()
    {
        DrawHelperAtCenter(this.transform.position, this.transform.forward, Color.blue, raycastLenght);
    }

    private void DrawHelperAtCenter(Vector3 origin, Vector3 direction, Color color, float scale)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(origin, origin + direction * scale);
    }

}