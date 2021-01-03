using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private ChildArray[] f_tiles;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitTilesArray(int x, int z)
    {
        f_tiles = new ChildArray[x];
        for(int i = 0; i < x; i++)
        {
            f_tiles[i] = new ChildArray(z);
        }
    }

    public void AddTileToArray(GameObject tile,int x,int z)
    {
        if (f_tiles == null)
        {
            return;
        }

        f_tiles[x].childArray[z] = tile;
    }

    public void DeleteTilesArray()
    {
        f_tiles = new ChildArray[0];
    }

}


//シリアライズされた子要素クラス(インスペクターに表示するため)
//（配列に入れたtileを保持できる）
[System.Serializable]
public class ChildArray
{
    public GameObject[] childArray;

    public ChildArray(int z)
    {
        childArray = new GameObject[z];
    }
}
