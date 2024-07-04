using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class FrameRecorder : MonoBehaviour
{
    public Camera cameraToRecord;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private int framecount = 0;
    private bool isRecording = false;

    private int targetfps = 30;
    private float _t = 0;
    private string recordingPath = "";

    int totalFrameCalcDuration = 0;

    public UnityEvent<string> OnFinishFrameRecording;

    void Start()
    {
        renderTexture = new RenderTexture(1920 * 3 / 4, 1080 * 3 / 4, 24);
        screenShot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        //cameraToRecord.targetTexture = renderTexture;
        framecount = 0;
    }

    public void StartRecording(string path)
    {
        framecount = 0;
        recordingPath = path;
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
        OnFinishFrameRecording?.Invoke(recordingPath);
        print("avg frame conversion time(in ticks): " + totalFrameCalcDuration / framecount);
    }

    void Update()
    {
        if (isRecording)
        {
            float duration = 1f / 30f;
            _t += Time.deltaTime;
            while (_t >= duration)
            {
                _t -= duration;
                float Test = Time.time;
                long startTick = DateTime.Now.Ticks;
                RecordFrame();
                totalFrameCalcDuration += (int)(DateTime.Now.Ticks - startTick);

                if (Time.time - Test > duration)
                {
                    print("Bad");
                    _t = 0;
                }
            }
        }
    }

    void RecordFrame()
    {
        //RenderTexture.active = renderTexture;
        //screenShot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        //screenShot.Apply();

        //byte[] bytes = screenShot.EncodeToJPG();
        string file = string.Format("{0}/frame_{1:D05}.jpg", recordingPath, framecount++);
        //File.WriteAllBytes(file, bytes);
        SaveRenderTextureToFile(renderTexture, file);

        //RenderTexture.active = null;
    }

    void SaveRenderTextureToFile(RenderTexture rt, string path)
    {
        NativeArray<byte> nativePixelArray = new NativeArray<byte>(rt.height * rt.width * 4, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        AsyncGPUReadbackRequest request = AsyncGPUReadback.RequestIntoNativeArray(ref nativePixelArray, rt, 0, (AsyncGPUReadbackRequest request) =>
        {
            if (!request.hasError)
            {
                NativeArray<byte> encoded = ImageConversion.EncodeNativeArrayToJPG(nativePixelArray, rt.graphicsFormat, (uint)rt.width, (uint)rt.height, 0, 100);
                File.WriteAllBytes(path, encoded.ToArray());
                encoded.Dispose();
            }
            nativePixelArray.Dispose();
        });
        request.WaitForCompletion();
    }
}
