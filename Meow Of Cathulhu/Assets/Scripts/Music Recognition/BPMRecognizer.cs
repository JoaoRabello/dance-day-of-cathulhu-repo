using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BPMRecognizer : MonoBehaviour 
{
    public AudioSource song;
    public GameObject cube;
    [SerializeField] private float sensibility;
    [SerializeField] private UnityEvent onBeatEvent;
    //Default value: 1.5142857f

    private bool Beated;
    private float[] historyBuffer = new float[43];
    private float[] channelRight;
    private float[] channelLeft;
    private int SamplesSize = 1024;
    private float InstantSpec;
    private float AverageSpec;
    private float Variance;
    private float Constant;

    private void Update () 
    {
      InstantSpec = sumStereo2(song.GetSpectrumData(SamplesSize, 0, FFTWindow.Hamming));
      
      AverageSpec = (SamplesSize / historyBuffer.Length) * sumLocalEnergy2(historyBuffer);
      
      Variance = VarianceAdder(historyBuffer) / historyBuffer.Length;
      Constant = (float)((-0.0025714 * Variance) + sensibility);
      
      var shiftingHistoryBuffer = new float[historyBuffer.Length];
      
      for (int i = 0; i < (historyBuffer.Length - 1); i++) 
      { 
          shiftingHistoryBuffer[i+1] = historyBuffer[i];
      }
      
      shiftingHistoryBuffer [0] = InstantSpec;
      
      for (int i = 0; i < historyBuffer.Length; i++) {
          historyBuffer[i] = shiftingHistoryBuffer[i];
      }
      
      if (InstantSpec> (Constant * AverageSpec)) 
      { 
          if(!Beated) 
          {
              Debug.Log("Beat");
              onBeatEvent?.Invoke();
              if(cube) cube.transform.localScale = new Vector3(3, 3, 3);
              Beated = true;
          }
      } 
      else 
      {
          if(Beated) 
          {
              Beated = false;
          }
      }

      if(cube) cube.transform.localScale = Vector3.Lerp(cube.transform.localScale, Vector3.one, 0.1f);
    }

    float sumStereo2(float[] Channel) {
      float e = 0;
      for (int i = 0; i < Channel.Length; i++) {
          float ToSquare = Channel[i];
          e += (ToSquare * ToSquare);
      }
      return e;
    }

    float sumLocalEnergy2(float[] Buffer) {
      float E = 0;
      for (int i = 0; i < Buffer.Length; i++) {
          float ToSquare = Buffer[i];
          E += (Buffer[i] * Buffer[i]);
      }
      return E;
    }

    float VarianceAdder (float[] Buffer) {
      float VarSum = 0;
      for (int i = 0; i < Buffer.Length; i++) {  //Rafa
          float ToSquare = Buffer[i] - AverageSpec;
          VarSum += (ToSquare * ToSquare);
      }
      return VarSum;
    }
}