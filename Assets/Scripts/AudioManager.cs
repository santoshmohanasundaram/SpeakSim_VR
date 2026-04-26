using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool isTracking = false;

    public int speakingTime;
    public int totalTime;
    public int speakingPercent;

    public float currentVolume;
    public string volumeFeedback;

    private float speakingTimeFloat;
    private float totalTimeFloat;

    private AudioClip micClip;
    private string micDevice;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0];
            micClip = Microphone.Start(micDevice, true, 10, 44100);
            Debug.Log("🎤 Mic Started: " + micDevice);
        }
        else
        {
            Debug.LogError("❌ No Microphone Found");
        }
    }

    void Update()
    {
        if (!isTracking)
        {
            Debug.Log("🎤 AudioTracking OFF");
            return;
        }

        if (micClip == null) return;

        totalTimeFloat += Time.deltaTime;

        int micPosition = Microphone.GetPosition(micDevice) - 256;
        if (micPosition < 0) return;

        float[] samples = new float[256];
        micClip.GetData(samples, micPosition);

        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
            sum += samples[i] * samples[i];

        currentVolume = Mathf.Sqrt(sum / samples.Length);

        if (currentVolume > 0.01f)
            speakingTimeFloat += Time.deltaTime;

        totalTime = Mathf.RoundToInt(totalTimeFloat);
        speakingTime = Mathf.RoundToInt(speakingTimeFloat);

        if (totalTime > 0)
            speakingPercent = (speakingTime * 100) / totalTime;

        if (currentVolume < 0.008f)
            volumeFeedback = "Low";
        else if (currentVolume > 0.03f)
            volumeFeedback = "High";
        else
            volumeFeedback = "Good";

        Debug.Log($"🎤 Volume: {currentVolume:F3} | Speaking%: {speakingPercent}% | Feedback: {volumeFeedback}");
    }

    public int GetSpeakingPercentage()
    {
        return speakingPercent;
    }

    public void ResetData()
    {
        Debug.Log("🎤 RESET CALLED");

        totalTimeFloat = 0f;
        speakingTimeFloat = 0f;

        speakingTime = 0;
        totalTime = 0;
        speakingPercent = 0;
        currentVolume = 0f;
        volumeFeedback = "";
    }
}