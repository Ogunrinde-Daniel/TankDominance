using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    public Image imageToFade;
    public float fadeDuration = 2.0f;
    private float fadeTimer = 0.0f;
    bool startFading = false;
    public void Startfading()
    {
        imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 1.0f);
        startFading = true;
    }


    void Update()
    {
        if(!startFading)return;

        fadeTimer += Time.deltaTime;
        float currentAlpha = Mathf.Lerp(1.0f, 0.0f, fadeTimer / fadeDuration);
        imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, currentAlpha);

        if (currentAlpha <= 0.0f)
        {
            startFading = false;
        }
        else if(currentAlpha <= 0.5f)
        {
            startFading = false;
            Invoke(nameof(resetStartFading), 0.01f);
        }

    }

    void resetStartFading()
    {
        startFading = true;
    }
}