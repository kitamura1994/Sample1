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

    ////////インスペクター表示////////
    [Header("識別情報")]
    public string frameName;
    public Image icon;
    public CATEGORY category;
    [Multiline(2)] public string caption;

    [Header("攻撃")]
    public int atk;
    public float atkInterval;
    public bool isMyCollider;
    public ATACK_TARGET atkTarget;

    [Header("防御")]
    public int hp;
    public int def;

    [Header("移動")]
    public float spd;
    public ROUTE_TYPE routeType;

    [Header("現在の状態")]
    public float cSpd;
    public int cHp;
    public int cAtk;
    public int cDef;

    [Header("経路")]
    //＊PENDING＊EnemyCreatorが生成時に中継地と目的地を渡すようにする
    //中継地
    public List<Vector2> wayPoints;
    //目的地
    public Vector2 destination;


    ////////プライベート定数////////
    //Y座標固定値
    private static float STATIC_Y = 0.55f;

    ////////プライベート変数////////
    //現在の目的地
    private Vector3 currentDestination;
    //中継地点の数
    private int wayPointsCount = 0;
    //現在通過した中継地点の数
    private int passedWPC = 0;

    private bool isMoving = true;

    //消滅するときに呼び出す関数
    public event Action<GameObject> deathAction;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        //初期値を現在の値に入れる。
        cSpd = spd;
        cHp = hp;
        cAtk = atk;
        cDef = def;

        //目的地を設定
        if (wayPoints.Count > 0)
        {
            currentDestination = new Vector3(wayPoints[passedWPC].x, STATIC_Y, wayPoints[passedWPC].y);
            wayPointsCount = wayPoints.Count;
        }
        else
        {
            currentDestination = new Vector3(destination.x, STATIC_Y, destination.y);
        }
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        //動ける状態なら
        if (isMoving) FrameMove();
    }

    //移動処理
    virtual protected void FrameMove()
    {
        //現在の目的地に到達したら、目的地を次の目的地に変更
        if (transform.position == currentDestination)
        {
            if (passedWPC < wayPointsCount - 1)
            {
                //中継地点を全て通過していないなら次の中継地へ
                passedWPC++;
                currentDestination = new Vector3(wayPoints[passedWPC].x, STATIC_Y, wayPoints[passedWPC].y);
            }
            else
            {
                //中継地を全て過ぎたなら目的地へ
                currentDestination = new Vector3(destination.x, STATIC_Y, destination.y);
            }
            //＊PENDING＊道をふさぐ障害物出す時はAstar実装して中継地点リストを更新する
        }
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentDestination.x, STATIC_Y, currentDestination.z), spd * Time.deltaTime);
    }

    //自身を消滅させる
    virtual public void FrameDestroy()
    {
        deathAction?.Invoke(gameObject);
        Destroy(gameObject);
    }

    //移動を許可しない
    virtual public void FrameStop()
    {
        isMoving = false;
    }

    //移動をを許可する
    virtual public void FrameWalk()
    {
        isMoving = true;
    }

    //ダメージ処理
    virtual public void FrameDamaged(int attackpoint)
    {
        cHp = cHp - (attackpoint - cDef);

        if (cHp <= 0)
        {
            FrameDestroy();
        }
    }
}


//移動手段
public enum ROUTE_TYPE
{
    ROAD,   //道を歩く
    SKY,    //空を飛ぶ
}

//攻撃対象
public enum ATACK_TARGET
{
    ROAD,   //下の相手だけ
    SKY,    //上の相手だけ
    BOTH,   //両方
}

//カテゴリー
public enum CATEGORY
{
    HUMAN,      //人間
    ROBOT,      //ロボット
    CREATURE,   //クリーチャー
}
