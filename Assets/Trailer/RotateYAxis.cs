using UnityEngine;

public class RotateYAxis : MonoBehaviour
{
    public float rotationSpeed = 5f; // degrees per second

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0f , 0f);
    }
}