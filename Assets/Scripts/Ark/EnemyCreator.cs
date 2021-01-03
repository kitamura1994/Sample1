using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public List<GameObject> enmeyList;
    public float interval;
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(CreateEnemy());
    }

    IEnumerator CreateEnemy()
    {
        yield return new WaitForSeconds(interval);
        Instantiate(enmeyList[0]);
        StartCoroutine(CreateEnemy());
    }
}
