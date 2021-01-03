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

    // Start is called before the first frame update
    void Start()
    {
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
            if (blockList.Count < maxBlock)
            {
                var enemy = other.gameObject;

                if (!blockList.Contains(enemy))
                {

                    blockList.Add(enemy);
                    enemy.GetComponent<Enemy>().FrameStop();
                    enemy.GetComponent<Enemy>().deathAction += BlockCountDecrement;
                    cBlock = blockList.Count;

                    if (blockList.Count > 0)
                    {
                        if (!isAttack)
                        {
                            StartCoroutine(Attack());
                            isAttack = true;
                        }
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
        blockList[0].GetComponent<Enemy>().FrameDamaged(cAtk);
        yield return new WaitForSeconds(cAtkInterval);
        StartCoroutine(Attack());
    }

}

public enum PUTTABLEPLACE
{
    ROAD,
    SECONDFLOR,
}
