using System.Collections.Generic;
using UnityEngine;


public class MTreeNode
{

    string _name;//节点名
    int _val;
    //private int _nChild;//子节点数
    //private int _level = -1;// 记录该节点在多叉树中的层数
    List<MTreeNode> _listChild;// 指向其自身的子节点，children一个链表，该链表中的元素是MTreeNode类型的指针

    public MTreeNode()
    {
        _listChild = new List<MTreeNode>();
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int Val
    {
        get { return _val; }
        set { _val = value; }
    }


    //public int NChild
    //{
    //    get { return _nChild; }
    //    set { _nChild = value; }
    //}

    //public int Level
    //{
    //    get { return _level; }
    //    set { _level = value; }
    //}

    public List<MTreeNode> ListChild
    {
        get { return _listChild; }
        set { _listChild = value; }
    }



}

public class MultiTree
{
    public List<int> yetList = new List<int>();

    public static MultiTree _instance = null;
    public static MultiTree GetInstance()
    {
        if (_instance == null)
            _instance = new MultiTree();
        return _instance;
    }

    private MTreeNode SearchNodeR(string name, MTreeNode head)
    {
        MTreeNode temp = null;
        if (head != null)
        {
            if (name.Equals(head.Name))
            {
                temp = head; //如果名字匹配
            }
            else //如果不匹配，则查找其子节点
            {
                for (int i = 0; i < head.ListChild.Capacity && temp == null; i++)
                {
                    temp = SearchNodeR(name, head.ListChild[i]);
                }
            }
        }
        return temp;
    }


    public void RecursionNode(MTreeNode node)
    {
        for (int i = 0; i < node.ListChild.Count; i++)
        {
            node.ListChild.Add(new MTreeNode());
            node.ListChild[i].Name = node.ListChild[i].ToString();
            RecursionNode(node.ListChild[i]);
        }
    }
    //public List<int> yetList = new List<int>();//已经找过的点

    public void InitTree(int idx)
    {
        yetList.Clear();
        MTreeNode head = new MTreeNode();
        head.Name = idx.ToString();
        head.Val = idx;
        CreateTree(ref head, idx);
    }

    void CreateTree(ref MTreeNode head, int idxNow)
    {

        var vlist = GameData.GetInstance().PassableIdx(idxNow);
        List<int> tempList = new List<int>();
        foreach (int v in vlist)
        {
            if (!yetList.Contains(v))
            {
                tempList.Add(v);
            }
        }
        vlist = tempList;



        vlist.Insert(0, idxNow);
        foreach (var v in vlist)
        {
            yetList.Add(v);
        }

        CreateNode(ref head, vlist);


    }

    void CreateNode(ref MTreeNode head, List<int> listChild)
    {
        MTreeNode temp = null;
        int nChild = listChild.Count - 1;
        string name = listChild[0].ToString();
        if (head == null)
        {
            //temp = head = new MTreeNode();
            //temp.Name = name;
            Debug.LogError("___head nil___");
            return;
        }
        else
        {
            temp = SearchNodeR(name, head);
        }

        //temp.ListChild.Count = nChild;
        for (int i = 0; i < nChild; i++)
        {
            temp.ListChild.Add(new MTreeNode());
            temp.ListChild[i].Val = listChild[i + 1];
            temp.ListChild[i].Name = listChild[i + 1].ToString();
            CreateTree(ref head, listChild[i + 1]);
        }
    }

}
