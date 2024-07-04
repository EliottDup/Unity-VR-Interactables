using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public class FfmpegRunner : MonoBehaviour
{
    private string framesPath;
    private string audioPath;
    private bool isDoneRecordingFrames = false;
    private bool isDoneRecordingAudio = false;
    private bool isDoneConverting = false;

    [SerializeField] private string ffmpegPath = "C:/ffmpeg/bin/ffmpeg.exe";
    private string persistentDataPath;

    public UnityEvent OnStartProcessing, OnFinishProcessing;

    public void OnFinishFrameRecording(string path)
    {
        framesPath = path;
        isDoneRecordingFrames = true;
        Debug.Log("frames done! path: " + path);
    }

    public void OnFinishRecordingAudio(string filename)
    {
        audioPath = filename;
        isDoneRecordingAudio = true;
        Debug.Log("audio done! file: " + filename);
    }

    void Update()
    {
        if (isDoneRecordingFrames && isDoneRecordingAudio)
        {
            Debug.Log("starting FFmpeg");
            isDoneRecordingFrames = false;
            isDoneRecordingAudio = false;
            persistentDataPath = Application.persistentDataPath;
            //EditorUtility.RevealInFinder(persistentDataPath + "/Output");
            Thread thread = new Thread(RunFFmpeg);
            OnStartProcessing?.Invoke();
            thread.Start();
        }

        if (isDoneConverting)
        {
            isDoneConverting = false;
            OnFinishProcessing.Invoke();
        }
    }

    private void RunFFmpeg()
    {
        string filename = Path.GetFileName(framesPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

        string videoOutputPath = Path.Combine(persistentDataPath, "Output");
        if (!Directory.Exists(videoOutputPath))
        {
            Directory.CreateDirectory(videoOutputPath);
        }
        videoOutputPath = Path.Combine(videoOutputPath, filename + ".mp4");
        string videoInputPattern = Path.Combine(framesPath, "frame_%05d.jpg");

        string args = string.Format(
            "-framerate 30 -i \"{0}\" -i \"{1}\"  -c:v libx264 -pix_fmt yuv420p -c:a aac \"{2}\" ", //Add audio supportd dumdum
            videoInputPattern,
            audioPath,
            videoOutputPath
            );

        Debug.Log(videoOutputPath);

        ProcessStartInfo startInfo = new(ffmpegPath, args)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Debug.Log("starting FFmpeg");

        using (Process process = new())
        {
            process.StartInfo = startInfo;
            process.OutputDataReceived += (sender, args) => UnityEngine.Debug.Log(args.Data);
            process.ErrorDataReceived += (sender, args) => UnityEngine.Debug.Log(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        UnityEngine.Debug.Log("FFmpeg is done!");
        isDoneConverting = true;
    }

}

//ffmpeg -framerate 30 -start_number 740 -i frame%05d.jpg -c:v libx264 -r 30 -pix_fmt yuv420p out.mp4