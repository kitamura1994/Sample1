using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FriendIcon : MonoBehaviour
{
    //配置するフレーム
    public GameObject myFrame;

    InputManager inputMng;
    private void Start()
    {
        inputMng = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
    }
    public void DragStart()
    {
        //レイキャストを有効にする
        inputMng.isTile = true;
    }

    public void DragEnd()
    {
        //タイルを指していたら
        if (inputMng.currentRayHitObject != null)
        {
            //タイルの上にフレームを生成
            var tilepos = inputMng.currentRayHitObject.transform.position;
            Instantiate(myFrame, new Vector3(tilepos.x, 0.55f, tilepos.z), Quaternion.identity);

            //レイキャストを無効にする。
            inputMng.isTile = false;
            //レイキャストが指すオブジェクトを外す。
            inputMng.RemoveCurrentRayHitObject();
        }
    }
}
