using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;

    [Header("On Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.2f;
    
    private Material originalMaterial;
    private Coroutine onDamageVFXCo;

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hitVFXColor = Color.white;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject critHitVFX;

    [Header("Element Colors")]
    [SerializeField] private Color chillVFX = Color.cyan;//冰冻特效颜色
    private Color originalHitVFXColor;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
        originalMaterial = sr.material;
        originalHitVFXColor = hitVFXColor;
    }

    public void PlayOnStatusVFX(float duration, ElementType elementType)
    {
        if (elementType == ElementType.Ice)
        {
            StartCoroutine(PlayStatusVFXCo(duration, chillVFX));
        }
    }

    private IEnumerator PlayStatusVFXCo(float duration,Color color)
    {
        float tickInterval = 0.2f;
        float timer = 0;

        Color lightColor = color * 1.2f;//乘以大于1的数，使得颜色变浅
        Color darkColor = color * 0.8f;//乘以小于1的数，使得颜色变深

        bool toggle = false;//切换器

        while(timer < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timer += tickInterval;
        }

        sr.color = Color.white;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit)
    {
        GameObject hitVFXPrefab = isCrit ? critHitVFX : hitVFX;//根据是否暴击选择不同的特效预制件
        GameObject vfx = Instantiate(hitVFXPrefab,target.position,Quaternion.identity);//实例化，在指定位置生成预制件特效
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVFXColor;//设置特效颜色

        if (entity.facingDir == -1 && isCrit)
        {
            vfx.transform.Rotate(0,180,0);//如果人物朝向左边且发生暴击，则水平翻转特效
        }
    }

    public void UpdateOnHitColor(ElementType elementType)
    {
        switch(elementType)
        {
            case ElementType.Ice:
                hitVFXColor = chillVFX;
                break;
            default:
                hitVFXColor = originalHitVFXColor;
                break;
        }
    }

    public void PlayOnDamageVFX()
    {
        if (onDamageVFXCo != null)
        {
            StopCoroutine(onDamageVFXCo);
        }
        onDamageVFXCo =  StartCoroutine(OnDamageVFXCo());
    }

    private IEnumerator OnDamageVFXCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVFXDuration);
        sr.material = originalMaterial;
    }

}
