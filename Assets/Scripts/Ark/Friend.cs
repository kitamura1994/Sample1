using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{

    ////////インスペクター表示////////
    [Header("識別情報")]
    public string frameName;
    public Image icon;
    public int cost;
    public PUTTABLEPLACE puttablePlace;
    [Multiline(2)] public string caption;

    [Header("攻撃")]
    public int atk;
    public float atkInterval;
    public GameObject atkArea;
    public ATACK_TARGET atkTarget;

    [Header("防御")]
    public int hp;
    public int def;
    public int block;

    [Header("現在の状態")]
    public int cHp;
    public int maxHp;
    public int cAtk;
    public float cAtkInterval;
    public int cDef;
    public int cBlock;
    public int maxBlock;

    ////////プライベート変数////////
    List<GameObject> blockList = new List<GameObject>();

    private bool isAttack = false;

    private bool isRegistering = false;

    // Start is called before the first frame update
    void Start()
    {
        //初期値を現在の値に入れる。
        cHp = hp;
        maxHp = hp;
        cAtk = atk;
        cAtkInterval = atkInterval;
        cDef = def;
        cBlock = 0;
        maxBlock = block;
    }

    // Update is called once per frame
    void Update()
    {
        //ブロックしている相手がいないなら攻撃をやめる
        //＊PENDING＊攻撃範囲≠自分のサイズを実装する際に変更する
        if (blockList.Count == 0)
        {
            if (isAttack)
            {
                StopCoroutine(Attack());
                isAttack = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            //ブロックリストに空きがあるなら
            if (cBlock < maxBlock)
            {
                GameObject enemy = other.gameObject;

                //対象のエネミーをすでに登録していないなら
                if (!blockList.Contains(enemy) && !isRegistering)
                {

                    Enemy enemyScript = enemy.GetComponent<Enemy>();

                    if (enemyScript != null && enemyScript.SetBlockFrame(gameObject))
                    {
                        isRegistering = true;
                        //リストに登録
                         blockList.Add(enemy);
                        //対象の動きを止める
                        enemyScript.FrameStop();
                        //対象が消滅した時にブロックリストを減らすようにイベントを登録
                         enemyScript.deathAction += BlockCountDecrement;
                        //登録したオブジェクトが消滅している場合のNULLを削除
                        blockList.RemoveAll(item => item == null);
                        cBlock = blockList.Count;

                        if (blockList.Count > 0)
                        {
                            if (!isAttack)
                            {
                                //攻撃開始
                                //＊PENDING＊攻撃範囲≠自分のサイズを実装する際に変更する
                                StartCoroutine(Attack());
                                isAttack = true;
                            }
                        }
                        isRegistering = false;
                    }
                }
            }

        }
    }

    public void BlockCountDecrement(GameObject enemy)
    {
        blockList.Remove(enemy);
        cBlock = blockList.Count;
    }

    IEnumerator Attack()
    {
        if (blockList.Count > 0)
        {
            blockList[0].GetComponent<Enemy>().FrameDamaged(cAtk);
        }

        yield return new WaitForSeconds(cAtkInterval);

        if (blockList.Count > 0)
        {
            StartCoroutine(Attack());
        }
    }

}

public enum PUTTABLEPLACE
{
    ROAD,
    SECONDFLOR,
}
