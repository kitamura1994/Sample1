using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [System.Serializable]
    public class Route
    {
        public List<Vector2> wayPoints;

        public Route()
        {
            wayPoints = new List<Vector2>();
        }
    }

    [System.Serializable]
    public struct CreateSetting
    {
        public int enemyPrefabNumber;
        public int destinationNumber;
        public int routeNumber;
        public float interval;
    }

    [SerializeField] List<GameObject> enmeyList;
    [SerializeField] float interval;

    [SerializeField] List<Route> routeList;
    [SerializeField] List<Vector2> destinationList;
    [SerializeField] List<CreateSetting> createSettingList;

    int currentCreateCount = 0;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(CreateEnemy());
    }

    IEnumerator CreateEnemy()
    {
        if (currentCreateCount < createSettingList.Count)
        {
            yield return new WaitForSeconds(createSettingList[currentCreateCount].interval);
            var obj = Instantiate(enmeyList[createSettingList[currentCreateCount].enemyPrefabNumber]);
            obj.GetComponent<Enemy>().SetDestination(destinationList[createSettingList[currentCreateCount].destinationNumber]);
            obj.GetComponent<Enemy>().SetWayPoints(routeList[createSettingList[currentCreateCount].routeNumber].wayPoints);

            currentCreateCount++;
            StartCoroutine(CreateEnemy());
        }
    }

    public int CountCreateEnemy()
    {
        return createSettingList.Count;
    }
}

