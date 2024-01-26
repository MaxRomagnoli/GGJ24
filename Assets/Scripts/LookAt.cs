using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        // this.transform.LookAt(target.transform);

        Vector3 direction = target.transform.position - this.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, this.transform.up);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, toRotation, speed * Time.deltaTime);
    }
}
