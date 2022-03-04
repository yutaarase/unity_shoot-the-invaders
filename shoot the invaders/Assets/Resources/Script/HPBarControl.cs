using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerのHPBarを操作するクラス
/// </summary>
public class HPBarControl : MonoBehaviour
{
    //お借りしたコードのサイト
    //https://capyuse-soft.com/hpbar_color/

    [Header("体力")]　public int maxHp;
    public bool death = false; //死亡したかどうか
    public Slider HpBar; //HPバー
    public Image sliderImage; //HPバーの描画部分
    private int currentHp; //HPの現在値
    private float currentX; //現在のx座標
    private float minX, maxX; //最小x座標 最大x座標
    private float dY; // デフォルトY座標
    
    

    // Start is called before the first frame update
    void Start()
    {
        //各変数初期化
        HpBar.value = 1;
        currentHp = maxHp;
        dY = HpBar.transform.localPosition.y;
    }

    /// <summary>
    /// inMin inMaxの間で現在値がxに対応したoutMin outMaxの間の値を取得するメソッド
    /// inMin - x - inMax
    /// outMin - return - outMax
    /// </summary>
    /// <param name="x">現在値</param>
    /// <param name="inMin">現在値に対応した最小値</param>
    /// <param name="inMax">現在値に対応した最大値</param>
    /// <param name="outMin">取得する値に対応した最小値</param>
    /// <param name="outMax">取得する値に対応した最大値</param>
    /// <returns>ｘに対応した値</returns>
    public float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    }

    /// <summary>
    /// HPBarを更新するメソッド
    /// </summary>
    /// <param name="diff">HPから減算する値</param>
    public void HPBarUpdate(int diff)
    {
        
        currentHp = currentHp - diff;

        //HPの現在値が最大値を超えたら最大値にする
        if(currentHp >= maxHp)
        {
            currentHp = maxHp;
        }

        //最大HPにおける現在のHPをSliderに反映。
        HpBar.value = (float)currentHp / (float)maxHp;

        //HpバーをX軸方向のどの位置まで移動しなければならないか計算
        currentX = MapValues(currentHp, 0, maxHp, minX, maxX);
        //Hpバーを移動させる
        HpBar.transform.localPosition = new Vector3(currentX, dY, 0);

        //Hpバーの色を変化させる
        //Hpが50%以上であれば緑から黄色へ、HPが50%未満であれば黄色から赤色へ変化する
        if (currentHp > maxHp / 2)
        {
            //色はRGB表記
            //最初は(R=0,G=255,B=0)で開始、Rを0→255に変化させて緑→黄色
            sliderImage.color = new Color32((byte)MapValues(currentHp, maxHp / 2, maxHp, 255, 0), 255, 0, 255);
        }
        else
        {
            //HP50%未満では(R=255,G=255,B=0)で開始、Gを255→0に変化させて黄色→赤
            sliderImage.color = new Color32(255, (byte)MapValues(currentHp, 0, maxHp / 2, 0, 255), 0, 255);
        }

        //HPの現在値が0以下になったらdeathをtrueにする
        if(currentHp <= 0)
        {
            death = true;
        }
    }
}
