using UnityEngine;
using System.Collections;
 
public class CSBars : MonoBehaviour {
   
    // Public Variables
    public int numSamples;
    public GameObject abar;
    public float valorTrinta;
    public float valorZeroDois;
    public float lerpSpeed;
    // Private Varaibles
    float[] numberleft;
    float[] numberright;
    GameObject[] thebarsleft;
    GameObject[] thebarsright;
    float spacing;
    float width;
   
    // Use this for initialization
    void Start () {
        thebarsleft = new GameObject[numSamples];
        //thebarsright = new GameObject[numSamples];
        spacing = 0.4f - (numSamples * 0.001f);
        width = 0.3f - (numSamples * 0.001f);
        for(int i=0; i < numSamples; i++){
            float xpos = i*spacing -8.0f;
            Vector3 positionleft = new Vector3(xpos,3, 0);
            thebarsleft[i] = (GameObject)Instantiate(abar, positionleft, Quaternion.identity) as GameObject;
            thebarsleft[i].transform.localScale = new Vector3(width,1,0.2f);
           
            // Vector3 positionright = new Vector3(xpos,-3, 0);
            // thebarsright[i] = (GameObject)Instantiate(abar, positionright, Quaternion.identity) as GameObject; 
            // thebarsright[i].transform.localScale = new Vector3(width,1,0.2f);
        }
    }
   
    // Update is called once per frame
    void Update () {
        numberleft = AudioListener.GetSpectrumData(numSamples, 0, FFTWindow.BlackmanHarris);
        //numberright = AudioListener.GetSpectrumData(numSamples, 1, FFTWindow.BlackmanHarris);
        
        for(int i=0; i < numSamples; i++){
            if (float.IsInfinity(numberleft[i]*valorTrinta) || float.IsNaN(numberleft[i]*valorTrinta)){
            }else{
                var destinyScale = new Vector3(width, numberleft[i]*valorTrinta,valorZeroDois);
                thebarsleft[i].transform.localScale =
                    Vector3.Lerp(thebarsleft[i].transform.localScale, destinyScale, lerpSpeed);
                //thebarsright[i].transform.localScale = new Vector3(width, numberright[i]*valorTrinta,0.2f); 
            }
        }
    }
}