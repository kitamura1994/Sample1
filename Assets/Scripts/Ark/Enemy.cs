using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

/// <summary>
/// 全ての敵にこのスクリプト、またはこのスクリプトを
/// 継承したスクリプトをアタッチして使用すること
/// </summary>
public class Enemy : MonoBehaviour
{

    //////////////////////// メンバ ////////////////////////

    #region インスペクター表示
    [Header("識別情報")]
    [SerializeField] string frameName;
    [SerializeField] Image icon;
    [SerializeField] Commons.CATEGORY category;
    [SerializeField] Commons.HABITAT habitat;
    [Multiline(2)] [SerializeField] string caption;

    [Header("攻撃")]
    [SerializeField] int atk;
    [SerializeField] float atkInterval;
    [SerializeField] int targetCount;
    [SerializeField] Commons.HABITAT targetHabitat;

    [Header("防御")]
    [SerializeField] int hp;
    [SerializeField] int def;

    [Header("移動")]
    [SerializeField] float spd;

    [Header("現在の状態")]
    [SerializeField] float currentSpd;
    [SerializeField] int currentHp;
    [SerializeField] int maxHp;
    [SerializeField] int currentAtk;
    [SerializeField] float currentAtkInterval;
    [SerializeField] int currentTargetCount;
    [SerializeField] int currentDef;

    [Header("経路")]
    //＊PENDING＊EnemyCreatorが生成時に中継地と目的地を渡すようにする
    //中継地
    [SerializeField] List<Vector2> wayPoints;
    //目的地
    [SerializeField] Vector2 destination;

    [Header("アタッチ用")]
    [SerializeField] Slider hpSlider;
    [SerializeField] EnemyAttack enemyAttackScript;
    [SerializeField] Animator animator;
    #endregion

    #region インスペクター非表示
    private Vector3 currentDestination; //現在の目的地
    private int wayPointsCount = 0;//中継地点の数
    private int passedWPC = 0; //現在通過した中継地点の数
    [SerializeField] private bool isMoving = true;
    private GameObject blockFrame = null;//ブロックしている駒
    [SerializeField] List<GameObject> attackTarget = new List<GameObject>();

    bool isAttackable = false;
    bool isAttacking = false;
    bool isAttacked = false;
    bool isCooling = false;

    #endregion

    //////////////////////// メソッド ////////////////////////

    #region MonoBehaviour系
    virtual protected void Start()
    {
        Initialize();

        //子オブジェクトの初期化
        enemyAttackScript.Initialize();
    }

    virtual protected void Update()
    {
        FrameChangeStatus();
        FrameAttack();
        FrameMove();
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
        currentSpd = spd;
        currentHp = hp;
        maxHp = hp;
        currentAtk = atk;
        currentAtkInterval = atkInterval;
        currentTargetCount = targetCount;
        currentDef = def;

        //目的地を設定
        if (wayPoints.Count > 0)
        {
            currentDestination = new Vector3(wayPoints[passedWPC].x, Commons.FRAME_POS_Y, wayPoints[passedWPC].y);
            wayPointsCount = wayPoints.Count;
        }
        else
        {
            currentDestination = new Vector3(destination.x, Commons.FRAME_POS_Y, destination.y);
        }

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

        //isMoving
        isMoving = true;
        if (blockFrame != null) isMoving = false;
        if (isAttacked || isAttacking) isMoving = false;

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
    /// 移動処理
    /// </summary>
    virtual protected void FrameMove()
    {
        if (isMoving)
        {
            //現在の目的地に到達したら、目的地を次の目的地に変更
            if (transform.position == currentDestination)
            {
                if (passedWPC < wayPointsCount - 1)
                {
                    //中継地点を全て通過していないなら次の中継地へ
                    passedWPC++;
                    currentDestination = new Vector3(wayPoints[passedWPC].x, Commons.FRAME_POS_Y, wayPoints[passedWPC].y);
                }
                else
                {
                    //中継地を全て過ぎたなら目的地へ
                    currentDestination = new Vector3(destination.x, Commons.FRAME_POS_Y, destination.y);
                }
                //＊PENDING＊道をふさぐ障害物出す時はAstar実装して中継地点リストを更新する
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentDestination.x, Commons.FRAME_POS_Y, currentDestination.z), spd * Time.deltaTime);
        }

        animator.SetBool(Commons.ANIMATOR_ISMOVING, isMoving);
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
            target.GetComponent<Friend>().FrameDamaged(currentAtk);
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
            StageManager.stageManagerScript.IncreaseDestroyedEnemyCount();
            FrameDestroy();
        }
    }

    /// <summary>
    /// 消滅処理
    /// </summary>
    virtual public void FrameDestroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// ブロックするオブジェクトの登録
    /// </summary>
    /// <param name="frame">ブロックするオブジェクト</param>
    /// <returns>登録可否</returns>
    public bool RegisterBlockFrame(GameObject frame)
    {
        if (blockFrame == null)
        {
            blockFrame = frame;
            return true;
        }
        return false;
    }
    #endregion
    #region Setter系
    public void SetDestination(Vector2 destination)
    {
        this.destination = destination;
    }

    public void SetWayPoints(List<Vector2> wayPoints)
    {
        this.wayPoints = wayPoints;
    }
    #endregion

    #region Getter系
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
    #endregion
}


