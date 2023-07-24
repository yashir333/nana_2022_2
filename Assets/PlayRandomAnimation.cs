using UnityEngine;

public class PlayRandomAnimation : MonoBehaviour
{
    public AnimationClip[] animationClips; // List of animation clips to randomly choose from
    private int currentClipIndex;
    private float currentClipLength;
    private float timer;
    Animator YourAnimator;

    private void Start()
    {
        PlayRandomAnimationClip();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentClipLength)
        {
            timer = 0f;
            PlayRandomAnimationClip();
        }
    }

    private void PlayRandomAnimationClip()
    {
        if (animationClips.Length == 0)
        {
            Debug.LogWarning("No animation clips assigned!");
            return;
        }

        currentClipIndex = Random.Range(0, animationClips.Length);
        AnimationClip clip = animationClips[currentClipIndex];
        currentClipLength = clip.length;

        // Play the animation
        // Replace "YourAnimator" with the actual reference to your Animator component
        YourAnimator.Play(clip.name, 0, 0f);
    }
}
