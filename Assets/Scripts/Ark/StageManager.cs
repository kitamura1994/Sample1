using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    //////////////////////// メンバ ////////////////////////

    #region インスペクター表示
    [SerializeField] int maxEnemyCount;
    [SerializeField] int passedEnemyCount;
    [SerializeField] int destroyedEnemyCount;

    [SerializeField] int durableCount;
    [SerializeField] int maxFriendCount;
    [SerializeField] int initialCost;
    [SerializeField] int maxCost;
    [SerializeField] int currentCost;
    [SerializeField] int costInterval;
    [SerializeField] List<GameObject> friendFrameList;
    [SerializeField] List<EnemyCreator> enemyCreatorList;

    #endregion

    #region インスペクター非表示
    public static StageManager stageManagerScript = null;
    bool isPause = false;
    float timeScale = Commons.TIMESCALE_NORMAL;

    bool isEnd = false;
    #endregion


    //////////////////////// メソッド ////////////////////////
    #region MonoBehaviour系

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        InitializeGameInfo();
        UIManager.uiManagerScript.InitializeFrameIconList();
        Invoke("GameStart", 2f);
    }

    private void Update()
    {
        CheckGameOver();
        CheckGameClear();
    }

    #endregion

    #region 初期化系
    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        if (stageManagerScript == null)
        {
            stageManagerScript = this;
        }
        else
        {
            Destroy(this.gameObject);
        }


    }

    private void InitializeGameInfo()
    {
        maxEnemyCount = 0;

        foreach (var creator in enemyCreatorList)
        {
            maxEnemyCount += creator.CountCreateEnemy();
        }

        currentCost = initialCost;
        UIManager.uiManagerScript.WriteDurableCountText(durableCount.ToString());
        UIManager.uiManagerScript.WriteDestroyedEnemyCountText(destroyedEnemyCount.ToString() + "/" + maxEnemyCount);
        UIManager.uiManagerScript.WriteCostText(currentCost.ToString());
    }

    /// <summary>
    /// ゲームスタート処理
    /// </summary>
    public void GameStart()
    {
        StartCoroutine(IncreaseCostCoroutine());
    }
    #endregion

    #region 更新系
    /// <summary>
    /// ゲームクリア条件監視
    /// </summary>
    public void CheckGameClear()
    {
        if (!isEnd)
        {
            if (destroyedEnemyCount + passedEnemyCount >= maxEnemyCount)
            {
                isEnd = true;
                UIManager.uiManagerScript.ActivateGameEndScreen(Commons.GAMEEND_CLEAR);
            }
        }
    }

    /// <summary>
    /// ゲームオーバー条件監視
    /// </summary>
    public void CheckGameOver()
    {
        if (!isEnd)
        {
            if (durableCount <= 0)
            {
                isEnd = true;
                UIManager.uiManagerScript.ActivateGameEndScreen(Commons.GAMEEND_OVER);
            }
        }
    }
    #endregion

    #region 内部部呼出し系
    void CommitChangeTimeScale()
    {
        if (isPause)
        {
            Time.timeScale = Commons.TIMESCALE_ZERO;
        }
        else
        {
            Time.timeScale = timeScale;
        }
    }

    IEnumerator IncreaseCostCoroutine()
    {
        yield return new WaitForSeconds(costInterval);

        if (currentCost < maxCost)
        {
            currentCost++;
            UIManager.uiManagerScript.WriteCostText(currentCost.ToString());
        }

        StartCoroutine(IncreaseCostCoroutine());
    }
    #endregion

    #region 外部呼出し系
    public void ChangeTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
        CommitChangeTimeScale();
    }

    public void ChangeIsPause(bool isPause)
    {
        this.isPause = isPause;
        CommitChangeTimeScale();
    }

    public void DecreaseDurableCount()
    {
        durableCount--;
        UIManager.uiManagerScript.WriteDurableCountText(durableCount.ToString());
    }

    public void IncreasePassedEnemyCount()
    {
        passedEnemyCount++;
    }

    public void IncreaseDestroyedEnemyCount()
    {
        destroyedEnemyCount++;
        UIManager.uiManagerScript.WriteDestroyedEnemyCountText(destroyedEnemyCount.ToString() + "/" + maxEnemyCount);
    }

    public bool PayCost(int cost)
    {
        if (cost > currentCost)
            return false;

        currentCost -= cost;
        UIManager.uiManagerScript.WriteCostText(currentCost.ToString());
        return true;
    }
    #endregion

    #region getter系
    public List<GameObject> GetFriendFrameList()
    {
        return friendFrameList;
    }
    #endregion

}

