using UnityEngine;

public class EnemyProducerController : MonoBehaviour
{
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private GameObject target;
    [SerializeField] private float interval;
    [SerializeField] private int maxNum = 10;
    [SerializeField] private int currentNum = 0;
    private float lastProduceTime;



    public void Update()
    {
        if (CanProduce() && currentNum < maxNum)
        {
            Vector3 position = GenerateRandomPosition();
            Instantiate(skeletonPrefab, position,Quaternion.identity);
            lastProduceTime = Time.time;
            currentNum++;
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        float xOffset = Random.Range(-5, 5);
        float x = target.transform.position.x + xOffset;
        float y = target.transform.position.y;
        Vector3 randomPositon = new Vector3(x, y,0);
        return randomPositon;
    }

    private bool CanProduce()
    {
        return Time.time > lastProduceTime + interval;
    }
   


}
