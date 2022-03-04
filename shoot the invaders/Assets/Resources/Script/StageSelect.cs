using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ選択ボタンを制御するクラス
/// </summary>
public class StageSelect : MonoBehaviour
{
    //Scene遷移を制御するコントローラー
    private GameObject controler;

    private void Start()
    {
        //オブジェクトを取得
        controler = GameObject.Find("SceneControler");
    }

    //クリックされたらコントローラーのステージ番号にstagenumの値を設定する
    public void OnClick(int stagenum)
    {
        controler.GetComponent<SceneControl>().stageNum = stagenum;
    }
}
