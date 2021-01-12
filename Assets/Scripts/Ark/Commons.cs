using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class Commons
{
    #region tag
    public static readonly string TAG_ENEMY         = "Enemy";
    public static readonly string TAG_TILE          = "Tile";
    public static readonly string TAG_EVENTSYSTEM   = "EventSystem";
    public static readonly string TAG_FRIEND        = "Friend";
    public static readonly string TAG_FRIEND_BLOCK  = "FriendBlock";
    public static readonly string TAG_FRIEND_ATTACK = "FriendAttack";
    public static readonly string TAG_ENEMY_ATTACK  = "EnemyAttack";
    public static readonly string TAG_STAGE_MANAGER  = "StageManager";
    #endregion

    #region Layer
    public static readonly string LAYER_ENEMY           = "Enemy";
    public static readonly string LAYER_TILE            = "Tile";
    public static readonly string LAYER_EVENTSYSTEM     = "EventSystem";
    public static readonly string LAYER_FRIEND          = "Friend";
    public static readonly string LAYER_FRIEND_BLOCK    = "FriendBlock";
    public static readonly string LAYER_FRIEND_ATTACK   = "FriendAttack";
    public static readonly string LAYER_ENEMY_ATTACK    = "EnemyAttack";
    #endregion

    #region その他定数
    //フレーム
    public static readonly float FRAME_POS_Y = 0.55f;

    //タイムスケール
    public static readonly float TIMESCALE_ZERO     = 0.0f;
    public static readonly float TIMESCALE_NORMAL   = 1.0f;
    public static readonly float TIMESCALE_SPEEDUP  = 2.0f;

    //Animator
    public static readonly string ANIMATOR_ISMOVING = "isMoving";
    public static readonly string ANIMATOR_ISATTACKING = "isAttacking";

    //ゲーム終了文字
    public static readonly string GAMEEND_CLEAR = "GAME CLEAR";
    public static readonly string GAMEEND_OVER = "GAME OVER";

    #endregion

    //生息域
    [Flags]
    public enum HABITAT
    {
        ROAD        = 1 << 0,    //地面の相手
        SECONDFLOOR = 1 << 1,    //二階の相手
        SKY         = 1 << 2,    //空の相手
    }

    //カテゴリー
    public enum CATEGORY
    {
        HUMAN       = 1 << 0,   //人間
        ROBOT       = 1 << 1,   //ロボット
        CREATURE    = 1 << 2,   //クリーチャー
    }

    /// <summary>
    /// ビットフラグ判定
    /// </summary>
    /// <param name="bitFlag"></param>
    /// <param name="bitMask"></param>
    /// <returns></returns>
    public static bool MatchBitFlag(int bitFlag,int bitMask)
    {
        return (bitFlag & bitMask) > 0 ? true:false;
    }

}
