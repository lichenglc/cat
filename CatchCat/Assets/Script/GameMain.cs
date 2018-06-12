using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameMain : MonoBehaviour
{


    private GameObject objPlayer;
    private Transform uiLogin;
    private Transform uiGame;
    private Transform boxWin;
    private Transform boxFail;

    private Transform _bgLayout;

    int nClick = 0;

    GameData insData;
    int mapx = 0;
    int mapy = 0;
    int plock = 1000;

    // Use this for initialization
    void Start()
    {

        insData = GameData.GetInstance();

        uiGame = transform.Find("Panel/uiGame");
        uiLogin = transform.Find("Panel/uiLogin");

        GameObject _btnBegin = uiLogin.Find("btnBegin").gameObject;
        _bgLayout = uiGame.Find("bgLayout");

        _btnBegin.GetComponent<Button>().onClick.AddListener(BeginClick);

        objPlayer = uiGame.Find("Player").gameObject;
        Init();

        boxWin = uiGame.Find("boxWin");
        boxFail = uiGame.Find("boxFail");



        boxWin.Find("btnAgain").gameObject.GetComponent<Button>().onClick.AddListener(AgainClick);
        boxFail.Find("btnAgain").gameObject.GetComponent<Button>().onClick.AddListener(AgainClick);
        boxWin.gameObject.SetActive(false);
        boxFail.gameObject.SetActive(false);
    }

    void Init()
    {
        mapx = _bgLayout.childCount;
        mapy = _bgLayout.GetChild(0).childCount;


        int _idx = 0;
        int _center = Mathf.CeilToInt((float)mapx * mapy / 2);
        for (int i = 0; i < mapx; i++)
        {
            var grid = _bgLayout.GetChild(i);
            for (int j = 0; j < mapy; j++)
            {
                _idx = _idx + 1;
                var item = grid.GetChild(j);
                item.name = _idx.ToString();
                ItemData itemData = new ItemData();
                itemData.tran = item;
                itemData.val = insData.SetPahtNum(_idx);

                //Debug.Log("___***__" + _idx);
                insData.sList.Add(_idx, itemData);

                if (_idx == _center)
                    FixPlayerPos(item);
            }
        }

        BornLock();
    }

    void Clear()
    {
        nClick = 0;
        foreach (var item in insData.sList)
        {
            item.Value.tran.GetComponent<Image>().color = Color.white;
        }
        insData.sList.Clear();
    }

    void BornLock()
    {
        int numRd = Random.Range(7, 16);
        int _center = Mathf.CeilToInt((float)mapx * mapy / 2);

        for (int i = 0; i < numRd; i++)
        {
            int idxRd = Random.Range(1, mapx * mapx);
            if (idxRd != _center)
            {
                insData.sList[idxRd].val = plock;
                insData.sList[idxRd].tran.GetComponent<Image>().color = Color.red;

            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        KeyDown();
    }

    void FixPlayerPos(Transform item)
    {
        objPlayer.transform.parent = item;
        objPlayer.transform.localPosition = new Vector3(0, 50, 0);
        insData.idxPlayer = int.Parse(item.name);
    }

    void BeginClick()
    {
        uiLogin.gameObject.SetActive(false);
        MainUI();
    }

    void AgainClick()
    {
        boxWin.gameObject.SetActive(false);
        boxFail.gameObject.SetActive(false);
        Clear();
        Init();
    }

    void ItemClick(Transform go)
    {
        if (insData.sList[insData.idxPlayer].val == 0)
        {
            Debug.Log("__Over___");
            return;
        }
        int idx = int.Parse(go.name);
        if (insData.sList[idx].val == plock)
            return;

        go.GetComponent<Image>().color = Color.red;
        insData.sList[idx].val = plock;//阻碍点
        nClick = nClick + 1;


        var idxNext = insData.PlayerNextIdx(insData.idxPlayer);
        FixPlayerPos(insData.sList[idxNext].tran);

        //FixPlayerPos(go);
    }


    void MainUI()
    {


        for (int i = 0; i < _bgLayout.childCount; i++)
        {
            var grid = _bgLayout.GetChild(i);
            for (int j = 0; j < grid.childCount; j++)
            {
                var item = grid.GetChild(j);
                item.GetComponent<Button>().onClick.AddListener(() => { ItemClick(item); });

            }

        }


    }

    void Win()
    {
        boxWin.gameObject.SetActive(true);
        boxFail.gameObject.SetActive(false);


        string _str = string.Format("您用了{0}步住了神经猫!!!", nClick);

        boxWin.Find("Image/Text").GetComponent<Text>().text = _str;


    }
    void Fail()
    {
        boxWin.gameObject.SetActive(false);
        boxFail.gameObject.SetActive(true);

    }

    void KeyDown()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            //Win();
            insData.DifficultyMode();
          
        }

        if (Input.GetKeyDown(KeyCode.B))
        {

            Fail();
        }
    }


}
