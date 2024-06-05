using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    // Convert an AudioClip to a byte array in WAV format
    public static byte[] FromAudioClip(AudioClip clip, int sampleCount, out int length)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        // Write WAV header
        writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
        writer.Write(0); // Placeholder for file size
        writer.Write(new char[4] { 'W', 'A', 'V', 'E' });

        // Write fmt chunk
        writer.Write(new char[4] { 'f', 'm', 't', ' ' });
        writer.Write(16);
        writer.Write((ushort)1); // Audio format (PCM)
        writer.Write((ushort)clip.channels);
        writer.Write(clip.frequency);
        writer.Write(clip.frequency * clip.channels * 2); // Byte rate
        writer.Write((ushort)(clip.channels * 2)); // Block align
        writer.Write((ushort)16); // Bits per sample

        // Write data chunk
        writer.Write(new char[4] { 'd', 'a', 't', 'a' });
        writer.Write(0); // Placeholder for data size

        // Write audio data
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);
        short[] intData = new short[sampleCount];
        byte[] bytesData = new byte[sampleCount * 2];

        int rescaleFactor = 32767; // To convert float to short

        for (int i = 0; i < sampleCount; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            byte[] byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        writer.Write(bytesData);

        // Update file and data sizes
        writer.Seek(4, SeekOrigin.Begin);
        writer.Write((int)writer.BaseStream.Length - 8);

        writer.Seek(40, SeekOrigin.Begin);
        writer.Write((int)writer.BaseStream.Length - 44);

        length = (int)writer.BaseStream.Length;
        byte[] result = stream.ToArray();

        writer.Close();
        stream.Close();

        return result;
    }

    // Save byte array to a file
    public static void Save(string path, byte[] bytes)
    {
        File.WriteAllBytes(path, bytes);
    }
}
