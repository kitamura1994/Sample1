using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] StageManager stageManager;
    void OnTriggerEnter(Collider other)
    {
        //オブジェクトが"Enemy"なら
        if (other.CompareTag("Enemy"))
        {
            //オブジェクトの削除処理を実行
            other.gameObject.GetComponent<Enemy>().FrameDestroy();

            //通過したことを報告
            stageManager.DecreaseDurableCount();
            stageManager.IncreasePassedEnemyCount();

        }
    }
}
