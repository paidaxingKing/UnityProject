using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchoInterval = 0.5f;
    [SerializeField] private GameObject imageEchoPrefab;

    private Coroutine imageEchoCo;

    public void CreatEffectOf(GameObject effect,Transform target)
    {
        Instantiate(effect, target.position, Quaternion.identity);
    }

    public void PlayImageEchoEffect(float duration)
    {
        if (imageEchoCo != null)
        {
            StopCoroutine(imageEchoCo);
        }

        imageEchoCo = StartCoroutine(ImageEchoEffectCo(duration));
    }

    private IEnumerator ImageEchoEffectCo(float duration)
    {
        float time = 0;
        
        while (time < duration)
        {
            CreateImageEcho();

            yield return new WaitForSeconds(imageEchoInterval);

            time += imageEchoInterval;
        }
    }

    private void CreateImageEcho()
    {
        GameObject imageEcho = Instantiate(imageEchoPrefab, transform.position, transform.rotation);//角色会翻转，所以残影的反转要和角色一致
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}
