using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Objective_TrainingStone : MonoBehaviour
{
    [SerializeField] private int numOfEnemies;
    [SerializeField] private List<GameObject> enemiesPrefab = new List<GameObject>();
    [SerializeField] private GameObject text;
    [SerializeField] private float radius;
    private bool canProduceEnemies;
    private float interval = 2f;
    private float lastProduceTime;
    private int currentNum;
    private bool isInZone;
    

    private void Update()
    {
        if (isInZone && Input.GetKeyDown(KeyCode.T))
        {
            canProduceEnemies = true;
        }

        if (canProduceEnemies && currentNum < numOfEnemies)
        {
            if (!CanProduce()) return;
          
            int randomIndex = Random.Range(0,enemiesPrefab.Count);
            GameObject randomEnemy = enemiesPrefab[randomIndex];
            Vector3 position = GenerateRandomPosition();
            Instantiate(randomEnemy, position, Quaternion.identity);
            lastProduceTime = Time.time;
            currentNum++;
        }
        else
        {
            canProduceEnemies = false;
            currentNum = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null) return;

        text.SetActive(true);
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == null) return;
        isInZone = true;

    }

    private Vector3 GenerateRandomPosition()
    {
        float xOffset = Random.Range(transform.position.x - radius, transform.position.x + radius);
        Vector3 randomPosition = new Vector3(xOffset, transform.position.y, 0);

        return randomPosition;
    }

    private bool CanProduce()
    {
        return Time.time > lastProduceTime + interval;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null) return;
        text.gameObject.SetActive(false);
        isInZone = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.right * radius);
        Gizmos.DrawRay(transform.position, -transform.right * radius);
    }
}
