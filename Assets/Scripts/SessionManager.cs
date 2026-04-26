using UnityEngine;
using TMPro;

public class SessionManager : MonoBehaviour
{
    [Header("Managers")]
    public EyeContactManager eye;
    public HeadStabilityManager head;
    public AudioManager audioManager;

    [Header("UI")]
    public GameObject startCanvas;
    public GameObject liveCanvas;
    public GameObject resultCanvas;

    public TextMeshProUGUI eyeText;
    public TextMeshProUGUI confidenceText;
    public TextMeshProUGUI speakingText;
    public TextMeshProUGUI volumeText;
    public TextMeshProUGUI feedbackText;

    public bool isSessionRunning = false;

    void Start()
    {
        // Initial UI state
        startCanvas.SetActive(true);
        liveCanvas.SetActive(false);
        resultCanvas.SetActive(false);
    }

    void Update()
    {
        if (!isSessionRunning) return;

        // 🔹 Live Feedback (clean, minimal)
        string feedback = "";

        if (eye.eyeContactPercent < 50)
            feedback += "Look at audience\n";

        if (audioManager.currentVolume < 0.008f)
            feedback += "Speak louder\n";

        if (audioManager.currentVolume > 0.03f)
            feedback += "Too loud\n";

        if (head.avgMovement > 8)
            feedback += "Stay steady\n";

        if (feedback == "")
            feedback = "Good performance";

        feedbackText.text = feedback;
    }

    public void StartSession()
    {
        isSessionRunning = true;

        // 🔥 Reset all data
        eye.ResetData();
        head.ResetData();
        audioManager.ResetData();

        // 🔹 Start tracking
        eye.isTracking = true;
        head.isTracking = true;
        audioManager.isTracking = true;

        // 🔹 Clear UI
        feedbackText.text = "";
        eyeText.text = "";
        confidenceText.text = "";
        speakingText.text = "";
        volumeText.text = "";

        // 🔹 Switch UI
        startCanvas.SetActive(false);
        resultCanvas.SetActive(false);
        liveCanvas.SetActive(true);
    }

    public void EndSession()
    {
        isSessionRunning = false;

        // 🔹 Stop tracking
        eye.isTracking = false;
        head.isTracking = false;
        audioManager.isTracking = false;

        // 🔹 Show results
        eyeText.text = "Eye Contact: " + eye.GetEyeContactPercentage() + "%";
        confidenceText.text = "Confidence: " + head.GetConfidenceScore() + "%";
        speakingText.text = "Speaking: " + audioManager.GetSpeakingPercentage() + "%";
        volumeText.text = "Volume: " + audioManager.volumeFeedback;

        // 🔹 Switch UI
        liveCanvas.SetActive(false);
        resultCanvas.SetActive(true);
    }

    public void QuitApp()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}