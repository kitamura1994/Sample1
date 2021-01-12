using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendAttack : MonoBehaviour
{

    //////////////////////// メンバ ////////////////////////

    #region インスペクター表示
    [Header("アタッチ用")]
    [SerializeField] Friend friendScript;
    #endregion

    #region インスペクター非表示
    bool isAttack = false;
    int currentTargetCount;
    List<GameObject> attackTarget;
    List<GameObject> attackList = new List<GameObject>();
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
    /// 初期化処理(Friendスクリプトが呼び出す)
    /// </summary>
    public void Initialize()
    {
        currentTargetCount = friendScript.GetCurrentTargetCount();
        attackTarget = friendScript.GetAttackTarget();
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
        attackList.RemoveAll(item => item == null);
        if (attackList.Count <= 0) return;

        //ブロックリストから攻撃対象を選定
        var blockList = friendScript.GetBlockList();
        if (blockList != null && blockList.Count > 0)
        {
            foreach (var blockObj in blockList)
            {
                if (attackList.Contains(blockObj))
                {
                    //リストからターゲットを追加
                    attackTarget.Add(blockObj);

                    //ターゲットが決まればreturn
                    if (attackTarget.Count >= currentTargetCount)
                        return;
                }
            }
        }

        //最終的にはアタックリストから登録
        for (int i = 0; i < attackList.Count; i++)
        {
            //すでに登録されていたら次へ
            if (attackTarget.Contains(attackList[i]))
                continue;

            attackTarget.Add(attackList[i]);

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
        if (other.CompareTag(Commons.TAG_ENEMY))
        {
            GameObject enemy = other.gameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                //攻撃対象の生息域かチェック
                if (Commons.MatchBitFlag((int)friendScript.GetTargetHabitat(), (int)enemyScript.GetHabitat()))
                {
                    //対象のエネミーをすでに登録していないなら
                    if (!attackList.Contains(enemy))
                    {
                        //リストに登録
                        attackList.Add(enemy);
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
        if (other.CompareTag(Commons.TAG_ENEMY))
        {
            GameObject enemy = other.gameObject;
            attackList.Remove(enemy);
            attackTarget.Remove(enemy);
        }
    }
    #endregion
}
