using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FriendIcon : MonoBehaviour
{
    //配置するフレーム
    [SerializeField] GameObject myFrame;
    [SerializeField] Text costCount;

    Friend myFrameScript;
    GameObject currentRayHitObject;
    bool isSelected = false;

    private void Start()
    {
    }
    private void Update()
    {
        RayCastForTile();
    }

    private void RayCastForTile()
    {
        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask layerMask = LayerMask.GetMask(new string[] { Commons.LAYER_TILE });
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                currentRayHitObject = hit.collider.gameObject;
            }
        }
    }
    private void RemoveCurrentRayHitObject()
    {
        currentRayHitObject = null;
    }

    public void DragStart()
    {
        //レイキャストを有効にする
        isSelected = true;
    }

    public void DragEnd()
    {
        //タイルを指していたら
        if (currentRayHitObject != null)
        {
            var tileScript = currentRayHitObject.GetComponent<Tile>();

            if (Commons.MatchBitFlag((int)tileScript.GetHabitatType(), (int)myFrameScript.GetHabitat()))
            {
                //駒を配置可能なら
                if (currentRayHitObject.GetComponent<Tile>().CheckPuttable())
                {
                    if (StageManager.stageManagerScript.PayCost(myFrameScript.GetCost())) 
                    {
                        Debug.Log(currentRayHitObject.GetComponent<Tile>().CheckPuttable());
                        //タイルの上にフレームを生成
                        var tilepos = currentRayHitObject.transform.position;
                        var obj=Instantiate(myFrame, new Vector3(tilepos.x, Commons.FRAME_POS_Y, tilepos.z), Quaternion.identity);
                        obj.GetComponent<Friend>().SetMyIconObject(gameObject);
                        gameObject.SetActive(false);
                        //レイキャストを無効にする。
                        isSelected = false;
                        //レイキャストが指すオブジェクトを外す。
                        RemoveCurrentRayHitObject();
                    } }
            }
        }
    }

    public void SetCostCount(int cost)
    {
        costCount.text = cost.ToString();
    }

    public void SetMyFrame(GameObject frame)
    {
        myFrame = frame;
        myFrameScript = myFrame.GetComponent<Friend>();
    }
}
