using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{

    //////////////////////// メンバ ////////////////////////

    #region インスペクター表示
    [Header("識別情報")]
    [SerializeField] string frameName;
    [SerializeField] Image icon;
    [SerializeField] int cost;
    [SerializeField] float respawnTime;
    [SerializeField] Commons.HABITAT habitat;
    [Multiline(2)] [SerializeField] string caption;

    [Header("攻撃")]
    [SerializeField] int atk;
    [SerializeField] float atkInterval;
    [SerializeField] int targetCount;
    [SerializeField] GameObject atkArea;
    [SerializeField] Commons.HABITAT targetHabitat;

    [Header("防御")]
    [SerializeField] int hp;
    [SerializeField] int def;
    [SerializeField] int block;

    [Header("現在の情報")]
    [SerializeField] int currentHp;
    [SerializeField] int maxHp;
    [SerializeField] int currentAtk;
    [SerializeField] float currentAtkInterval;
    [SerializeField] int currentTargetCount;
    [SerializeField] int currentDef;
    [SerializeField] int currentBlock;
    [SerializeField] int maxBlock;

    [Header("アタッチ用")]
    [SerializeField] FriendBlock blockScript;
    [SerializeField] FriendAttack attackScript;
    [SerializeField] Slider hpSlider;
    [SerializeField] Animator animator;
    #endregion

    #region インスペクター非表示
    [SerializeField] List<GameObject> attackTarget = new List<GameObject>();
    bool isAttackable = false;
    bool isAttacking = false;
    bool isAttacked = false;
    bool isCooling = false;

    List<GameObject> blockList = new List<GameObject>();
    GameObject myIcon;
    #endregion

    //////////////////////// メソッド ////////////////////////

    #region MonoBehaviour系
    virtual protected void Awake()
    {
        Initialize();

        //子オブジェクトの初期化
        blockScript.Initialize();
        attackScript.Initialize();
    }

    virtual protected void Update()
    {
        FrameChangeStatus();
        FrameAttack();
        UpdateHpDisplay();
    }
    #endregion

    #region 初期化系
    /// <summary>
    /// 初期化処理
    /// </summary>
    virtual protected void Initialize()
    {
        //初期値を現在の値に入れる。
        currentHp = hp;
        maxHp = hp;
        currentAtk = atk;
        currentAtkInterval = atkInterval;
        currentTargetCount = targetCount;
        currentDef = def;
        currentBlock = 0;
        maxBlock = block;
    }
    #endregion

    #region 更新系
    /// <summary>
    /// 状態変更処理
    /// </summary>
    virtual protected void FrameChangeStatus()
    {
        //isAttackable
        if (!isAttackable)
        {
            if (attackTarget.Count > 0)
                isAttackable = true;
        }
        else
        {
            if (attackTarget.Count <= 0)
                isAttackable = false;
        }

        //isAttacking
        if (isAttackable && !isCooling)
            isAttacking = true;
        else
            isAttacking = false;
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void FrameAttack()
    {
        //攻撃ダメージ処理後は攻撃モーションが終わるまでフラグを変更しない
        if (!isAttacked)
        {
            //攻撃アニメーション実施
            animator.SetBool(Commons.ANIMATOR_ISATTACKING, isAttackable & isAttacking);
        }
    }

    /// <summary>
    /// HP表示の更新
    /// </summary>
    virtual protected void UpdateHpDisplay()
    {
        hpSlider.value = (float)currentHp / (float)maxHp;
    }
    #endregion

    #region Animation呼び出し系
    /// <summary>
    /// 攻撃ダメージ処理
    /// </summary>
    virtual public void Attaking()
    {
        //登録したオブジェクトが消滅している場合のNULLを削除
        attackTarget.RemoveAll(item => item == null);
        if (attackTarget.Count <= 0) return;

        foreach (var target in attackTarget)
        {
            target.GetComponent<Enemy>().FrameDamaged(currentAtk);
        }

        isAttacked = true;
    }

    /// <summary>
    /// 攻撃クールタイム処理
    /// </summary>
    virtual public IEnumerator CoolTimeCoroutine()
    {
        isCooling = true;
        isAttacked = false;
        yield return new WaitForSeconds(currentAtkInterval);
        isCooling = false;
    }
    #endregion

    #region 外部呼出し系
    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    /// <param name="attackpoint">攻撃力</param>
    virtual public void FrameDamaged(int attackpoint)
    {
        currentHp = currentHp - (attackpoint - currentDef);

        if (currentHp <= 0)
        {
            FrameDestroy();
        }
    }

    /// <summary>
    /// 消滅処理
    /// </summary>
    virtual public void FrameDestroy()
    {
        //myIcon.SetActive(true);
        Destroy(gameObject);
    }
    #endregion

    #region Setter系
    virtual public void SetCurrentBlock(int blockCunt)
    {
        currentBlock = blockCunt;
    }

    public void SetMyIconObject(GameObject icon)
    {
        myIcon = icon;
    }

    #endregion

    #region Getter系
    public int GetMaxBlock()
    {
        return maxBlock;
    }

    public int GetCurrentAttack()
    {
        return currentAtk;
    }

    public float GetCurrentAttackInterval()
    {
        return currentAtkInterval;
    }

    public int GetCurrentTargetCount()
    {
        return currentTargetCount;
    }

    public Commons.HABITAT GetHabitat()
    {
        return habitat;
    }

    public Commons.HABITAT GetTargetHabitat()
    {
        return targetHabitat;
    }

    public List<GameObject> GetAttackTarget()
    {
        return attackTarget;
    }

    public List<GameObject> GetBlockList()
    {
        return blockList;
    }

    public int GetCost()
    {
        return cost;
    }

    public Image GetIcon()
    {
        return icon;
    }
    #endregion









}

