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

    public AudienceManager audienceManager;
    public SoundManager soundManager;

    public bool isSessionRunning = false;

    void Start()
    {
        startCanvas.SetActive(true);
        liveCanvas.SetActive(false);
        resultCanvas.SetActive(false);
    }

    void Update()
    {
        if (!isSessionRunning) return;

        string feedback = "";

        // 👁 Eye Contact (instant)
        if (eye.eyeContactPercent < 50)
            feedback += "<color=#FF4444>Try to focus more on the audience</color>\n";
        else
            feedback += "<color=#00FF88>Good eye contact</color>\n";

        // 🔊 Volume (instant)
        if (audioManager.currentVolume < 0.008f)
            feedback += "<color=#FF4444>Try speaking a bit louder</color>\n";
        else if (audioManager.currentVolume > 0.03f)
            feedback += "<color=#FF4444>Try lowering your voice slightly</color>\n";
        else
            feedback += "<color=#00FF88>Your voice level is good</color>\n";

        // 🎯 Movement (instant)
        if (head.avgMovement > 25)
            feedback += "<color=#FF4444>Try to stay a bit more steady</color>\n";
        else
            feedback += "<color=#00FF88>Your posture looks good</color>\n";

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

        // 👏 Audience + sound
        if (audienceManager != null)
            audienceManager.StartClapping();

        if (soundManager != null)
            soundManager.PlayClap();

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