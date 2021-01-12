
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    //////////////////////// メンバ ////////////////////////

    #region インスペクター表示
    [Header("アタッチ用")]
    [SerializeField] Enemy enemyScript;
    #endregion

    #region インスペクター非表示
    int currentTargetCount;
    List<GameObject> targettableList = new List<GameObject>();
    List<GameObject> attackTarget;
    #endregion

    //////////////////////// メソッド ////////////////////////

    #region MonoBehaviour系
    void Update()
    {
        SelectAttackTarget();
    }

    void OnTriggerStay(Collider other)
    {
        AddEnemyList(other);
    }

    void OnTriggerExit(Collider other)
    {
        RemoveEnemyList(other);
    }
    #endregion

    #region 初期化系
    /// <summary>
    /// 初期化処理(Enemyスクリプトが呼び出す)
    /// </summary>
    public void Initialize()
    {
        currentTargetCount = enemyScript.GetCurrentTargetCount();
        attackTarget = enemyScript.GetAttackTarget();
    }
    #endregion

    #region 更新系
    /// <summary>
    ///攻撃対象を決定
    /// </summary>
    private void SelectAttackTarget()
    {
        //ターゲットが消滅している場合にリストから削除
        attackTarget.RemoveAll(item => item == null);
        //ターゲットに変更が無ければreturn
        if (attackTarget.Count >= currentTargetCount)
            return;

        //登録したオブジェクトが消滅している場合のNULLを削除
        targettableList.RemoveAll(item => item == null);
        if (targettableList.Count <= 0) return;

        //アタックリストから登録
        for (int i = 0; i < targettableList.Count; i++)
        {
            //すでに登録されていたら次へ
            if (attackTarget.Contains(targettableList[i]))
                continue;

            attackTarget.Add(targettableList[i]);

            if (attackTarget.Count >= currentTargetCount)
                return;
        }

    }
    #endregion

    #region 当たり判定系
    /// <summary>
    /// 攻撃範囲にいる敵をリストに追加
    /// </summary>
    /// <param name="other"></param>
    private void AddEnemyList(Collider other)
    {
        if (other.CompareTag(Commons.TAG_FRIEND))
        {
            GameObject friend = other.gameObject;
            Friend friendScript = friend.GetComponent<Friend>();

            if (friendScript != null)
            {
                //攻撃対象の生息域かチェック
                if (Commons.MatchBitFlag((int)enemyScript.GetTargetHabitat(), (int)friendScript.GetHabitat()))
                {
                    //対象のフレンドをすでに登録していないなら
                    if (!targettableList.Contains(friend))
                    {
                        //リストに登録
                        targettableList.Add(friend);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 攻撃エリアから出る時にリストから削除
    /// </summary>
    /// <param name="other"></param>
    private void RemoveEnemyList(Collider other)
    {
        if (other.CompareTag(Commons.TAG_FRIEND))
        {
            GameObject enemy = other.gameObject;
            targettableList.Remove(enemy);
            attackTarget.Remove(enemy);
        }
    }
    #endregion
}
