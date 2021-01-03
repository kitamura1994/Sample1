using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //オブジェクトが"Enemy"なら
        if (other.CompareTag("Enemy"))
        {
            //オブジェクトの削除処理を実行
            other.gameObject.GetComponent<Enemy>().FrameDestroy();
        }
    }
}
