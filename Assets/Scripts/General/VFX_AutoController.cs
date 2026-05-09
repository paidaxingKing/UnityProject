using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;
    [Space]
    [SerializeField] private bool randomOffset = true;
    [SerializeField] private bool randomRotation = true;
    [Header("Random Rotation")]
    [SerializeField] private float minRotation = 0f;
    [SerializeField] private float maxRotation = 360f;

    [Header("Random Position")]
    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.3f;
    [Space]
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.3f;

    private void Start()
    {
        ApplyRandomOffset();
        ApplyRandomRotation();

        if (autoDestroy)
        {
            Destroy(gameObject, destroyDelay);//经过一定延迟后，摧毁本特效对象
        }
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
