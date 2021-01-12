using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //////////////////////// メンバ ////////////////////////

    #region インスペクター表示
    [Header("アタッチ用")]
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject timeScaleButton;
    [SerializeField] GameObject frameIcon;
    [SerializeField] GameObject frameIconList;


    [SerializeField] Sprite speedUpSprite;
    [SerializeField] Sprite normalSpeedSprite;

    [SerializeField] Text durableCountText;
    [SerializeField] Text destroyedEnemyCountText;
    [SerializeField] Text costCountText;



    #endregion

    #region インスペクター非表示
    public static UIManager uiManagerScript = null;
    bool isPause = false;
    bool isSpeedUp = false;
    #endregion

    //////////////////////// メソッド ////////////////////////

    #region MonoBehaviour系

    private void Awake()
    {
        Initialize();
    }
    #endregion

    #region 初期化系
    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        if (uiManagerScript == null)
        {
            uiManagerScript = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void InitializeFrameIconList()
    {
        List<GameObject> objList = new List<GameObject>();
        foreach (var obj in StageManager.stageManagerScript.GetFriendFrameList())
        {
            objList.Add(obj);
        }

        //コスト順に並び替え(降順)
        objList.Sort((a, b) => b.GetComponent<Friend>().GetCost() - a.GetComponent<Friend>().GetCost());

        float nextPos = 0f;
        foreach (var obj in objList)
        {
            var icon = (GameObject)Instantiate(frameIcon);
            icon.transform.SetParent(frameIconList.transform,false);
            var pos = icon.GetComponent<RectTransform>().localPosition;
            var x = pos.x + nextPos;
            icon.GetComponent<RectTransform>().localPosition = new Vector3(x, pos.y, pos.z);
            var friendScript = obj.GetComponent<Friend>();
            icon.GetComponent<Image>().sprite = friendScript.GetIcon().sprite;
            icon.GetComponent<Image>().color = friendScript.GetIcon().color;
            icon.GetComponent<FriendIcon>().SetCostCount(friendScript.GetCost());
            icon.GetComponent<FriendIcon>().SetMyFrame(obj);

            nextPos -= 100f;

        }

    }
    #endregion

    #region 外部呼出し系
    /// <summary>
    /// ポーズボタン処理
    /// </summary>
    public void PauseButton()
    {
        isPause = !isPause;
        StageManager.stageManagerScript.ChangeIsPause(isPause);
        pauseScreen.SetActive(isPause);
    }

    /// <summary>
    /// スピードボタン処理
    /// </summary>
    public void TimeScaleButton()
    {
        if (isSpeedUp)
        {
            StageManager.stageManagerScript.ChangeTimeScale(Commons.TIMESCALE_NORMAL);
            timeScaleButton.GetComponent<Image>().sprite = speedUpSprite;
        }
        else
        {
            StageManager.stageManagerScript.ChangeTimeScale(Commons.TIMESCALE_SPEEDUP);
            timeScaleButton.GetComponent<Image>().sprite = normalSpeedSprite;
        }
        isSpeedUp = !isSpeedUp;
    }

    /// <summary>
    /// 終了ボタン処理
    /// </summary>
    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }

    public void ActivateGameEndScreen(string endingWord)
    {
        endScreen.GetComponentInChildren<Text>().text = endingWord;
        endScreen.SetActive(true);
    }

    public void WriteDurableCountText(string str)
    {
        durableCountText.text = str;
    }

    public void WriteDestroyedEnemyCountText(string str)
    {
        destroyedEnemyCountText.text = str;
    }

    public void WriteCostText(string str)
    {
        costCountText.text = str;
    }
    #endregion


}
