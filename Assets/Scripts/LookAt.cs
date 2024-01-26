using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        if(speed == 0)
        {
            // Rotate the camera every frame so it keeps looking at the target
            this.transform.LookAt(target);
        }
        else
        {
            Vector3 direction = (target.position) - this.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, this.transform.up);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, toRotation, speed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
