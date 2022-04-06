using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Textを点滅表示させるクラス
/// </summary>
public class Blinker : MonoBehaviour
{
    //点滅スピード
    public float speed;
    //点滅するテキスト
    private Text text;
    //経過時間
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        //インスタンス初期化
        text = this.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //点滅表示
        text.color = GetAlphaColor(text.color);
    }

    /// <summary>
    /// 点滅表示メソッド
    /// </summary>
    /// <param name="color">点滅表示させるテキストの色</param>
    /// <returns>変更した色</returns>
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time);
        return color;
    }
}
