using UnityEngine;

public class RotateAround : MonoBehaviour {

    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 31f;

    void LateUpdate()
    {
        transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);
    }
}