using UnityEditor;
using UnityEngine;

public class CreateMap : EditorWindow
{
    //生成するマップのサイズ
    public int f_xInt = 1;
    public int f_zInt = 1;

    public GameObject tileObject;

    // Editorメニューに "CreateMap" というメニュー項目を追加
    [MenuItem("Editor/CreateMap")]
    public static void ShowWindow()
    {
        //既存のウィンドウのインスタンスを表示。ない場合は作成します。
        GetWindow(typeof(CreateMap));
    }

    void OnGUI()
    {
        //マップサイズの入力
        GUILayout.Label("Map Size", EditorStyles.boldLabel);
        f_xInt = EditorGUILayout.IntSlider("size x", f_xInt, 1, 99);
        f_zInt = EditorGUILayout.IntSlider("size z", f_zInt, 1, f_xInt);
        EditorGUILayout.Space();

        //マップ用タイルの指定
        tileObject = (GameObject)EditorGUILayout.ObjectField(tileObject, typeof(GameObject), true);

        //生成ボタン
        if (GUI.Button(new Rect(10.0f, 100.0f, 120.0f, 20.0f), "Create Map"))
        {
            //タイル用のオブジェクトが使用されていなければ終了
            if (tileObject == null)
            {
                Debug.Log("Tileに使用するオブジェクトを指定してください");
                return;
            }

            //Mapオブジェクトがなければ生成
            var map = GameObject.Find("Map");
            if (map == null)
            {
                map = new GameObject("Map");
            }
            else
            {
                //Mapの子オブジェクトを削除
                for (int i = map.transform.childCount - 1; i >= 0; --i)
                {
                    DestroyImmediate(map.transform.GetChild(i).gameObject);
                }

                //配列情報削除
                if (map.GetComponent<MapManager>() != null)
                {
                    map.GetComponent<MapManager>().DeleteTilesArray();
                }
            }

            //マップマネージャーを追加
            var mapManager = map.GetComponent<MapManager>();
            if (mapManager == null)
            {
                mapManager = map.AddComponent<MapManager>();
            }

            //タイル格納用配列をマップサイズで初期化
            mapManager.InitTilesArray(f_xInt, f_zInt);

            //Map生成
            for (int i = 0; i < f_xInt; i++)
            {
                for (int j = 0; j < f_zInt; j++)
                {
                    var tile = Instantiate(tileObject, new Vector3(i, 0, j), Quaternion.identity, map.transform);

                    mapManager.AddTileToArray(tile, i, j);
                }
            }

            //カメラの位置調整
            var cam = Camera.main;
            //原点に最初のタイルの中心があるため、半個分マイナス
            float xpos = (f_xInt / 2.0f) - 0.5f;
            //カメラの視野角が60度なので1:2:√3(×1.1は端を見せるため)
            float ypos = (Mathf.Sqrt(3.0f) * f_xInt / 2.0f) * 1.2f;
            //原点に最初のタイルの中心があるため、半個分マイナス
            float zpos = (f_zInt / 2.0f) - 0.5f;
            cam.transform.position = new Vector3(xpos, ypos, zpos);

            //カメラの回転調整
            cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            //マップの中心を中心にカメラを公転
            cam.transform.RotateAround(new Vector3(xpos, 0.0f, zpos), Vector3.left, 30.0f);

        }

        //削除ボタン
        if (GUI.Button(new Rect(10.0f, 130.0f, 120.0f, 20.0f), "Delete Map"))
        {
            var map = GameObject.Find("Map");
            if (map != null)
            {
                //Mapの子オブジェクトを削除
                for (int i = map.transform.childCount - 1; i >= 0; --i)
                {
                    DestroyImmediate(map.transform.GetChild(i).gameObject);
                }

                //配列情報削除
                if (map.GetComponent<MapManager>() != null)
                {
                    map.GetComponent<MapManager>().DeleteTilesArray();
                }
            }
        }
    }
}
