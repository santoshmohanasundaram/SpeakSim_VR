using UnityEngine;
using UnityEngine.InputSystem;

public class GazeClickManager : MonoBehaviour
{
    public float maxDistance = 10f;

    private GameObject currentTarget;

    public GameObject startButton;
    public GameObject endButton;
    public GameObject retryButton;
    public GameObject quitButtonStart;
    public GameObject quitButtonResult;

    public SessionManager sessionManager;
    public SoundManager soundManager;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            currentTarget = hit.collider.gameObject;
            Debug.Log("🎯 Looking at: " + currentTarget.name);
        }
        else
        {
            currentTarget = null;
        }

        if (IsPressed())
        {
            Debug.Log("🖱 CLICK DETECTED");
            HandleClick();
        }
    }

    bool IsPressed()
    {
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            return true;

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
            return true;

        return false;
    }

    void HandleClick()
    {
        if (currentTarget == null)
        {
            Debug.LogError("❌ No target clicked");
            return;
        }

        Debug.Log("🔥 Clicked: " + currentTarget.name);

        // 🔘 PLAY CLICK SOUND HERE
        if (soundManager != null)
            soundManager.PlayClick();

        if (currentTarget == startButton)
            sessionManager.StartSession();

        else if (currentTarget == endButton)
            sessionManager.EndSession();

        else if (currentTarget == retryButton)
            sessionManager.StartSession();

        else if (currentTarget == quitButtonStart || currentTarget == quitButtonResult)
            sessionManager.QuitApp();
    }
}