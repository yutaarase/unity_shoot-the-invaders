using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Taskを管理するクラス
/// </summary>
public class TaskManager : MonoBehaviour
{
    public float maxCnt; //ターゲットの最大値
    public float currentCnt; //現在のターゲット数
    private GameObject controler;

    // Start is called before the first frame update
    void Start()
    {
        //変数初期化
        //オブジェクトを取得
        //ターゲットのオブジェクトの配列を取得
        maxCnt = 0;
        controler = GameObject.FindGameObjectWithTag("Controler");
        GameObject[] tagets = GameObject.FindGameObjectsWithTag("MainTarget");

        //取得した配列分ループをしターゲットの数をカウントする
        foreach (GameObject taget in tagets)
        {
            ++maxCnt;
        }
        currentCnt = maxCnt;
    }

    // Update is called once per frame
    void Update()
    {
        //現在のターゲット数が0以下だったら
        //ゲームクリアにする
        //Taskの達成率を更新する
        if(currentCnt <= 0)
        {
            controler.GetComponent<SceneControl>().gameClear = true;
        }
        controler.GetComponent<SceneControl>().completeRate = 100 - (int)(currentCnt / maxCnt * 100);
    }
}
