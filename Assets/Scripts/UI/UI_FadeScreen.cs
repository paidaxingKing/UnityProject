using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeScreen : MonoBehaviour
{
    private Image fadeImage;
    public Coroutine fadeEffectCO { get; private set; }

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    public void DoFadeIn(float duration = 1)//黑色->透明 淡入
    {
        fadeImage.color = new Color(0, 0, 0, 1);//表示黑色
        FadeEffect(0f,duration);
    }

    public void DoFadeOut(float duration = 1)//透明->黑色 淡出
    {
        fadeImage.color = new Color(0,0, 0, 0);
        FadeEffect(1f, duration);
    }


    private void FadeEffect(float targetAlpha,float duration)
    {
        if (fadeEffectCO != null)
        {
            StopCoroutine(fadeEffectCO);
        }

        fadeEffectCO = StartCoroutine(FadeEffectCo(targetAlpha, duration));
    }

    private IEnumerator FadeEffectCo(float targetAlpha,float duration)
    {
       
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < duration)//相当于每一帧循环一次
        {
            time = time + Time.deltaTime;//一帧过去的时间

            var color = fadeImage.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);//线性插值函数
            //Mathf.Lerp(起点, 终点, 比例) 是线性插值函数。time / duration 计算出了当前时间占总时间的百分比（0 到 1 之间）。
            //它会根据这个百分比，计算出当前帧应该处于 startAlpha 和 targetAlpha 之间的哪个具体数值
            //也就是说当time >=duration后，就会变为终点值，即targetAlpha，即0

            fadeImage.color = color;

            yield return null;//暂停执行后续代码，等到下一帧再继续循环
        }

        fadeImage.color = new Color(fadeImage.color.r,fadeImage.color.g,fadeImage.color.b,targetAlpha);
    }
}
