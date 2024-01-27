using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 1f;
    [Tooltip("Distance before follower stop following")]
    [SerializeField] private float minDistance = 2f;

    public void AddSpeed(float _toAdd)
    {
        speed += _toAdd;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.transform.position - this.transform.position;
        if(minDistance < 0 || direction.sqrMagnitude < minDistance) { return; }
        this.transform.position += direction.normalized * speed * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, minDistance);
    }
}
