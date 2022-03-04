using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 全てのScene遷移を統括するクラス
/// </summary>
public class SceneControl : MonoBehaviour 
{

    public int stageNum;　//遷移するステージ番号
    public bool playerDeath = false; //Playerが死亡しているかどうか
    public int score; //スコア
    public int completeRate; //Taskの達成率
    private int index; //現在のSceneのID

    // 各SceneのID
    private const int TITLE_ID = 0; //タイトル
    private const int STAGESELECT_ID = 1; //ステージ選択
    private const int STAGE1_ID = 2; //ステージ1
    private const int STAGE2_ID = 3; //ステージ2
    private const int GAMEOVER_ID = 4; //ゲームオーバー
    private const int CLEAR_ID = 5; //クリア
    public int clearNum = 0; //クリアしたステージ番号
    public bool gameClear = false; //ゲームをクリアしたかどうか
    private Button button2; //処理用ボタンインスタンス
    private Text text1, text2; //処理用テキストインスタンス
    private bool getButton; //ボタンを取得したかどうか
    private bool getText; //テキストを取得したかどうか

    

    public void Start()
    {
        //現在のIDを初期化 
        //このスクリプトをアタッチしているオブジェクトを破壊不可能にする
        index = SceneManager.GetActiveScene().buildIndex;
        DontDestroyOnLoad(gameObject);
    }
    public void Update()
    {
        //各状態に合わせて処理をする
        switch (index)
        {
            case TITLE_ID:
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    //画面をクリックしたらステージ選択画面に遷移する
                    //各変数初期化
                    NextScene(STAGESELECT_ID);
                    getButton = false;
                    getText = false;
                    stageNum = 0;

                }
                if (Input.GetKey(KeyCode.Escape))
                {
                    //Escapeを押すとアプリケーションを終了させる
                    //UnityEditorだった場合実行を停止する
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                }
                break;
            case STAGESELECT_ID:
                //各変数初期化
                score = 0;
                completeRate = 0;
                gameClear = false;
                getText=false;

                //何度もボタンオブジェクトを取得しないようにする
                if (!getButton)
                {
                    //ボタン型のオブジェクトの配列を取得
                    //それぞれのインスタンスを初期化
                    var buttons = FindObjectsOfType<Button>();
                    foreach (var button in buttons)
                    {
                        if(button.name == "Button2") button2 = button;
                    }

                    //クリア番号が0以下だったらボタン２を非表示にする
                    //じゃなかったら表示させる
                    //ボタンを取得したのでtrueにする
                    if (clearNum <= 0) button2.gameObject.SetActive(false);
                    else button2.gameObject.SetActive(true);
                    getButton = true;
                }

                if (Input.GetKey(KeyCode.Escape))
                {
                    //Escapeを押すとアプリケーションを終了させる
                    //UnityEditorだった場合実行を停止する
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                }

                //ステージ番号ごとに遷移先を変える
                switch (stageNum)
                {
                    case 0:
                        break;
                    case 1:
                        NextScene(STAGE1_ID);
                        break;
                    case 2:
                        NextScene(STAGE2_ID);
                        break;
                }
                break;
            //共通の処理を行う
            case STAGE1_ID:
            case STAGE2_ID:
                //Playerが死亡していたらゲームオーバーに遷移
                if (playerDeath)
                {
                    NextScene(GAMEOVER_ID);
                }
                //クリアしていたらクリアに遷移
                if (gameClear)
                {
                    NextScene(CLEAR_ID);
                }

                //事故防止の為Escapeはなしにする
                break;
            case GAMEOVER_ID:
                if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    //画面をクリックしたらステージ選択画面に遷移する
                    //各変数初期化
                    getButton = false;
                    stageNum = 0;
                    NextScene(STAGESELECT_ID);
                }

                if (Input.GetKey(KeyCode.Escape))
                {
                    //Escapeを押すとアプリケーションを終了させる
                    //UnityEditorだった場合実行を停止する
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                }

                //何度もテキストオブジェクトを取得しないようにする
                if (!getText)
                {
                    //テキスト型のオブジェクトの配列を取得する
                    //それぞれのインスタンスを初期化
                    var texts = FindObjectsOfType<Text>();
                    foreach (var text in texts)
                    {
                        if (text.name == "Score") text1 = text;
                        else if (text.name == "CompleteRate") text2 = text;
                    }
                    getText = true;
                }

                //テキストにそれぞれの内容を代入する
                text1.text = "スコア: " + score;
                text2.text = "達成率: " + completeRate +" ％";
                break;
            case CLEAR_ID:
                //ステージ番号がクリア番号よりおおきかったら
                //クリア番号をステージ番号にする
                if(stageNum > clearNum)
                {
                    clearNum = stageNum;
                }

                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    //画面をクリックしたらステージ選択画面に遷移する
                    //各変数初期化
                    getButton = false;
                    stageNum = 0;
                    NextScene(STAGESELECT_ID);
                }

                if (Input.GetKey(KeyCode.Escape))
                {
                    //Escapeを押すとアプリケーションを終了させる
                    //UnityEditorだった場合実行を停止する
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                }

                //何度もテキストオブジェクトを取得しないようにする
                if (!getText)
                {
                    //テキスト型のオブジェクトの配列を取得する
                    //それぞれのインスタンスを初期化
                    var texts = FindObjectsOfType<Text>();
                    foreach (var text in texts)
                    {
                        if (text.name == "CompleteRate") text1 = text;
                        else if (text.name == "Score") text2 = text;
                    }
                    getText = true;
                }

                //テキストにそれぞれの内容を代入する
                text1.text = "スコア: " + score;
                text2.text = "達成率: " + completeRate +" ％";
                break;
        }
        //現在のIDを更新
        index = SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// 入力した値のSceneに遷移するメソッド
    /// </summary>
    /// <param name="index">遷移するScene番号</param>
    public void NextScene(int index)
    {
        SceneManager.LoadScene(index);
    }


    //いつか使いそうだから残しておく
    /// <summary>
    /// 現在のシーンをリロードする
    /// </summary>
    public void RetryScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
