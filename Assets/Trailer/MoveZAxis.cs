using UnityEngine;

public class MoveZAxis : MonoBehaviour
{
    public float speed = 1.0f; // units per second

    void Update()
    {
        Vector3 pos = transform.position;
        pos.z += speed * Time.deltaTime;  // increase Z
        transform.position = pos;
    }
}