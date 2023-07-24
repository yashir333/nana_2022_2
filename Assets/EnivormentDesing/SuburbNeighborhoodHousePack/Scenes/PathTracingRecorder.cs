using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PathTracingRecorder : MonoBehaviour
{
    public string folderPath = "SavedFrames";
    public string fileNamePrefix = "Frame";
    public int startFrame = 1;
    public int endFrame = 100;

    private Camera mainCamera;
    private HDCamera hdCamera;
    private int currentFrame;
    private string fullPath;
    private HDRenderPipeline hdPipeline;
    public PlayableDirector playableDirector;

    private void Start()
    {
        mainCamera = Camera.main;
        hdCamera = HDCamera.GetOrCreate(mainCamera);
        currentFrame = startFrame;
        fullPath = Path.Combine(Application.dataPath, folderPath);

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        StartCoroutine(CaptureFrames());
    }

    private IEnumerator CaptureFrames()
    {
        while (currentFrame <= endFrame)
        {
            playableDirector.Pause();
            yield return new WaitUntil(() => hdCamera != null);
          

            hdPipeline = RenderPipelineManager.currentPipeline as HDRenderPipeline;
            yield return new WaitUntil(() => hdPipeline != null);
             
            yield return new WaitUntil(() => hdPipeline.IsFrameCompleted(hdCamera));
         
            
            CaptureFrame();
            currentFrame++;

            // Advance playable director by one frame
            TimelineAsset timelineAsset = (TimelineAsset)playableDirector.playableAsset;
            double frameDuration = 1.0 / timelineAsset.editorSettings.fps;
            playableDirector.time += frameDuration;

            // Play the timeline
            playableDirector.Play();

            // Wait for the end of frame update
            yield return new WaitForEndOfFrame();
        }
    }

    private void CaptureFrame()
    {
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        mainCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        mainCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Path.Combine(fullPath, $"{fileNamePrefix}_{currentFrame:D04}.png");
        File.WriteAllBytes(filename, bytes);
        Debug.Log($"Saved frame {currentFrame} to: {filename}");
    }
}
