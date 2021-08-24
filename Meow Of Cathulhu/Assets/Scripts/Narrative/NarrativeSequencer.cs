using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NarrativeSequencer : MonoBehaviour
{
    [SerializeField] private List<string> texts = new List<string>();
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private float timeToCompleteAlpha;

    public UnityEvent OnNarrativeEnded;

    private int textIndex = 0;

    private void Start()
    {
        StartCoroutine(TextChangeBehaviour());
    }

    private IEnumerator TextChangeBehaviour()
    {
        float timer = 0;
        Color oldColor;

        while (textIndex < texts.Count)
        {
            label.SetText(texts[textIndex]);
            textIndex++;

            oldColor = label.color;

            while (timer < timeToCompleteAlpha)
            {
                label.color = new Color(oldColor.r,oldColor.g,oldColor.b,timer/timeToCompleteAlpha);
                timer += Time.deltaTime;
                yield return null;
            }

            label.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1);

            yield return new WaitForSeconds(2);

            while (timer > 0)
            {
                label.color = new Color(oldColor.r, oldColor.g, oldColor.b, timer / timeToCompleteAlpha);
                timer -= Time.deltaTime;
                yield return null;
            }

            label.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
            timer = 0;

            yield return new WaitForSeconds(0.5f);
        }

        OnNarrativeEnded?.Invoke();
    }
}
