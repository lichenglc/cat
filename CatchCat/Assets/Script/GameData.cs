using System;
using System.Collections.Generic;
using UnityEngine;



public class ItemData
{
    public int val;
    public Transform tran;
}


public class GameData
{

    public static GameData _instance = null;

    public static GameData GetInstance()
    {
        if (_instance == null)
            _instance = new GameData();
        return _instance;
    }

 
    public Dictionary<int, ItemData> sList = new Dictionary<int, ItemData>();


    public int _idxPlayer = 0;
    int mapx = 9;
    int plock = 1000;

    public int idxPlayer
    {
        get { return _idxPlayer; }
        set { _idxPlayer = value; }
    }

    public int SetPahtNum(int idx)
    {
        int val = plock;//阻碍点

        int x = (int)SwitchXY(idx).x;
        int y = (int)SwitchXY(idx).y;

        //Debug.Log("_xx_" + x + "_yy__" + y + "__&&idx&&__" + idx);

        if (x <= 5 || y <= 5)
        {
            if (x <= y)
                val = x - 1;
            else
                val = y - 1;
        }
        else
        {
            if (x > y)
                val = mapx - x;
            else
                val = mapx - y;
        }
        //UnityEngine.Debug.Log("__xx__" + x + "__yy__" + y + "__val__" + val);
        return val;
    }

    public List<int> PassableIdx(int idxNow)
    {
        int y = Mathf.FloorToInt(idxNow / mapx);
        if (idxNow % mapx != 0)
            y = y + 1;
        List<int> vlist = new List<int>();
        if (y % 2 == 0)//偶数行
        {
            if (CheakPointExist(idxNow - 1))//左
                vlist.Add(idxNow - 1);
            if (CheakPointExist(idxNow + 1))//右
                vlist.Add(idxNow + 1);
            if (CheakPointExist(idxNow - mapx))//左上
                vlist.Add(idxNow - mapx);
            if (CheakPointExist(idxNow - mapx + 1))//右上
                vlist.Add(idxNow - mapx + 1);
            if (CheakPointExist(idxNow + mapx))//左下
                vlist.Add(idxNow + mapx);
            if (CheakPointExist(idxNow + mapx + 1))//右下
                vlist.Add(idxNow + mapx + 1);
        }
        else
        {
            if (CheakPointExist(idxNow - 1))//左
                vlist.Add(idxNow - 1);
            if (CheakPointExist(idxNow + 1))//右
                vlist.Add(idxNow + 1);
            if (CheakPointExist(idxNow - mapx - 1))//左上
                vlist.Add(idxNow - mapx - 1);
            if (CheakPointExist(idxNow - mapx))//右上
                vlist.Add(idxNow - mapx);
            if (CheakPointExist(idxNow + mapx - 1))//左下
                vlist.Add(idxNow + mapx - 1);
            if (CheakPointExist(idxNow + mapx))//右下
                vlist.Add(idxNow + mapx);
        }


        return vlist;
    }

    public int PlayerNextIdx(int idxNow)
    {


        var idxVal = SimpleMode(idxNow);

        return idxVal;
    }


    public bool CheakPointExist(int idx)
    {
        bool bisExist = true;

        int x = (int)SwitchXY(idx).x;
        int y = (int)SwitchXY(idx).y;

        if (x < 1 || y > mapx || sList[idx].val == plock)
            bisExist = false;

        return bisExist;
    }

    public Vector2 SwitchXY(int idx)
    {
        int x = idx % mapx;
        if (x == 0)
            x = mapx;
        int y = Mathf.FloorToInt(idx / mapx);
        if (idx % mapx != 0)
            y = y + 1;

        return new Vector2(x, y);
    }

    /// <summary>
    /// 不是墙的点
    /// </summary>
    /// <returns>The node.</returns>
    public List<int> AliveNode()
    {
        List<int> passList = new List<int>();
        foreach (var data in sList)
        {
            if (data.Value.val != plock)
            {
                passList.Add(data.Key);
            }
        }
        return passList;
    }

    public int SimpleMode(int idxNow)
    {
        var vlist = PassableIdx(idxNow);

        int resultVal = 999;
        int idxVal = -1;

        foreach (int v in vlist)
        {
            if (sList[v].val < resultVal)
            {
                resultVal = sList[v].val;
                idxVal = v;
            }
        }


        return idxVal;

    }


    public int DifficultyMode()
    {



        MultiTree.GetInstance().InitTree(idxPlayer);



        return 0;
    }

    public void SimpleArithmetic(int idxNow)
    {
      



    }

}
