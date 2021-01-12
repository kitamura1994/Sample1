using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendBlock : MonoBehaviour
{
    //////////////////////// メンバ ////////////////////////

    #region インスペクター表示
    [Header("アタッチ用")]
    [SerializeField] Friend friendScript;
    #endregion

    #region インスペクター非表示
    int maxBlock;
    int currentBlock = 0;
    List<GameObject> blockList;
    #endregion

    //////////////////////// メソッド ////////////////////////

    #region MonoBehaviour系
    void Update()
    {
        RemoveNullFromBlockList();
    }

    private void OnTriggerStay(Collider other)
    {
        AddBlockList(other);
    }
    #endregion

    #region 初期化系
    /// <summary>
    /// 初期化処理(Friendスクリプトが呼び出す)
    /// </summary>
    public void Initialize()
    {
        maxBlock = friendScript.GetMaxBlock();
        blockList = friendScript.GetBlockList();
    }
    #endregion
    
    #region 更新系
    /// <summary>
    /// ブロックリストからNULLを除去
    /// </summary>
    void RemoveNullFromBlockList()
    {
        //消滅した参照をListから削除
        blockList.RemoveAll(item => item == null);
    }
    #endregion

    #region 当たり判定系
    /// <summary>
    /// ブロックリストにオブジェクトを追加
    /// </summary>
    void AddBlockList(Collider other)
    {
        if (other.CompareTag(Commons.TAG_ENEMY))
        {
            //ブロックリストに消滅したオブジェが無いかチェック
            blockList.RemoveAll(item => item == null);

            //ブロックリストに空きがあるなら
            if (blockList.Count < maxBlock)
            {
                GameObject enemy = other.gameObject;

                //対象のエネミーをすでに登録していないなら
                if (!blockList.Contains(enemy))
                {

                    Enemy enemyScript = enemy.GetComponent<Enemy>();

                    //エネミーに自分がブロックしていることを登録出来たら
                    if (enemyScript != null && enemyScript.RegisterBlockFrame(gameObject))
                    {
                        //リストに登録
                        blockList.Add(enemy);
                        currentBlock = blockList.Count;
                        friendScript.SetCurrentBlock(currentBlock);
                    }
                }
            }

        }
    }
    #endregion
}
