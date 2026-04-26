using UnityEngine;

public class HeadStabilityManager : MonoBehaviour
{
    public bool isTracking = false;

    public int totalMovement;
    public int totalTime;
    public int avgMovement;
    public int confidenceScore;

    private float totalMovementFloat;
    private float totalTimeFloat;

    private Quaternion previousRotation;

    void Start()
    {
        previousRotation = Camera.main.transform.rotation;
    }

    void Update()
    {
        if (!isTracking)
        {
            Debug.Log("🧠 HeadTracking OFF");
            return;
        }

        totalTimeFloat += Time.deltaTime;

        Quaternion currentRotation = Camera.main.transform.rotation;
        float angle = Quaternion.Angle(previousRotation, currentRotation);

        totalMovementFloat += angle;
        previousRotation = currentRotation;

        totalMovement = Mathf.RoundToInt(totalMovementFloat);
        totalTime = Mathf.RoundToInt(totalTimeFloat);

        if (totalTime > 0)
            avgMovement = totalMovement / totalTime;

        confidenceScore = Mathf.RoundToInt(CalculateConfidence(avgMovement));

        Debug.Log($"🧠 Time: {totalTime}s | Movement: {avgMovement} deg/s | Confidence: {confidenceScore}%");
    }

    float CalculateConfidence(float movement)
    {
        if (movement < 2) return 100;
        if (movement < 5) return 80;
        if (movement < 10) return 60;
        if (movement < 15) return 40;
        return 20;
    }

    public int GetConfidenceScore()
    {
        return confidenceScore;
    }

    public void ResetData()
    {
        Debug.Log("🧠 RESET CALLED");

        totalMovementFloat = 0f;
        totalTimeFloat = 0f;

        totalMovement = 0;
        totalTime = 0;
        avgMovement = 0;
        confidenceScore = 0;

        previousRotation = Camera.main.transform.rotation;
    }
}