using UnityEngine;

public class AudienceManager : MonoBehaviour
{
    public Animator[] audienceAnimators;

    public void StartClapping()
    {
        Debug.Log("👏 All Audience Clapping");

        foreach (Animator anim in audienceAnimators)
        {
            anim.SetTrigger("Clap");
        }
    }
}