using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 1f;

    public void AddSpeed(float _toAdd)
    {
        speed += _toAdd;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += (target.transform.position - this.transform.position).normalized * speed * Time.deltaTime;
    }
}
