using UnityEngine;

public class EyeContactManager : MonoBehaviour
{
    public bool isTracking = false;

    public int totalTime;
    public int eyeContactTime;
    public int eyeContactPercent;

    private float totalTimeFloat;
    private float eyeContactTimeFloat;

    void Update()
    {
        if (!isTracking)
        {
            Debug.Log("👁 EyeTracking OFF");
            return;
        }

        totalTimeFloat += Time.deltaTime;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("👁 Looking at: " + hit.collider.name);

            if (hit.collider.CompareTag("Audience"))
            {
                eyeContactTimeFloat += Time.deltaTime;
                Debug.Log("👁 Eye Contact Detected");
            }
        }

        totalTime = Mathf.RoundToInt(totalTimeFloat);
        eyeContactTime = Mathf.RoundToInt(eyeContactTimeFloat);

        if (totalTime > 0)
            eyeContactPercent = (eyeContactTime * 100) / totalTime;

        Debug.Log($"👁 Time: {totalTime}s | Contact: {eyeContactTime}s | %: {eyeContactPercent}");
    }

    public int GetEyeContactPercentage()
    {
        return eyeContactPercent;
    }

    public void ResetData()
    {
        Debug.Log("👁 RESET CALLED");

        totalTimeFloat = 0f;
        eyeContactTimeFloat = 0f;

        totalTime = 0;
        eyeContactTime = 0;
        eyeContactPercent = 0;
    }
}