using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FrameRecorder : MonoBehaviour
{
    public Camera cameraToRecord;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private int framecount = 0;
    private bool isRecording = false;

    private string recordingPath = "";

    void Start()
    {
        renderTexture = new RenderTexture(1920, 1080, 24);
        screenShot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        cameraToRecord.targetTexture = renderTexture;
        framecount = 0;
    }

    public void StartRecording(string path)
    {
        framecount = 0;
        recordingPath = path;
        isRecording = true;
        StartCoroutine(RecordFrames());
    }

    public void StopRecording()
    {
        isRecording = false;
        StopCoroutine(RecordFrames());
    }

    IEnumerator RecordFrames()
    {
        while (isRecording)
        {
            yield return new WaitForSeconds(1 / 30f);
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            screenShot.Apply();

            byte[] bytes = screenShot.EncodeToJPG();
            string file = string.Format("{0}/frame{1:D05}.jpg", recordingPath, framecount++);
            File.WriteAllBytes(file, bytes);

            //print("Recorded frame: " + framecount);

            RenderTexture.active = null;
            //remember: ffmpeg -framerate 30 -start_number 740 -i frame%05d.jpg -c:v libx264 -r 30 -pix_fmt yuv420p out.mp4
        }
    }
}
