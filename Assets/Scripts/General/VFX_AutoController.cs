using System.Collections;
using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    private SpriteRenderer sr; 

    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;
    [Space]
    [SerializeField] private bool randomOffset = true;
    [SerializeField] private bool randomRotation = true;

    [Header("Fade Effect")]
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1;

    [Header("Random Rotation")]
    [SerializeField] private float minRotation = 0f;
    [SerializeField] private float maxRotation = 360f;

    [Header("Random Position")]
    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.3f;
    [Space]
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.3f;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (canFade)
        {
            StartCoroutine(FadeCo());
        }


        ApplyRandomOffset();
        ApplyRandomRotation();

        if (autoDestroy)
        {
            Destroy(gameObject, destroyDelay);//经过一定延迟后，摧毁本特效对象
        }
    }

    private IEnumerator FadeCo()
    {
        Color targetColor = Color.white;

        while (targetColor.a > 0)//当透明度大于0
        {
            targetColor.a = targetColor.a - (fadeSpeed * Time.deltaTime);//每帧时间间隔减少一次透明度
            sr.color = targetColor;

            yield return null;//默认是停止一帧的时间再重新执行
        }

        sr.color = targetColor;
    }

    private void ApplyRandomOffset()
    {
        if (randomOffset)
        {
            float xOffset = Random.Range(xMinOffset,xMaxOffset);
            float yOffset = Random.Range(yMinOffset,yMaxOffset);

            transform.position += new Vector3(xOffset,yOffset);//在原有位置基础上增加一个随机偏移量
        }
    }

    private void ApplyRandomRotation()
    {
        if (randomRotation)
        {
            float zRotation = Random.Range(minRotation, maxRotation );
            transform.Rotate(0,0, zRotation);//在z轴上随机旋转一个角度
        }
    }
}
