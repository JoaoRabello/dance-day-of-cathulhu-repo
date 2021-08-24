using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRenderer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider pleasingSlider;
    [SerializeField] private List<Image> lifeImages = new List<Image>();

    public void UpdateLife(int lifeAmount)
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            if (i < lifeAmount)
            {
                lifeImages[i].gameObject.SetActive(true);
                continue;
            }

            lifeImages[i].gameObject.SetActive(false);
        }

    }

    public void UpdatePleasingValue(float actualValue, int totalValue)
    {
        pleasingSlider.value = actualValue / totalValue;
    }

    public void UpdateScoreText(float score)
    {
        scoreText.SetText(score.ToString());
    }
}
