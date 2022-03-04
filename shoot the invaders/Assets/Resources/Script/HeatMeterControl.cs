using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HeatMeterを操作するクラス
/// </summary>
public class HeatMeterControl : MonoBehaviour
{
    //お借りしたコードのサイト
    //https://capyuse-soft.com/hpbar_color/

    [Header("最大積算熱量")] public float maxHeat;
    [Header("オーバーヒート")] public bool overHeat;
    private float currentHeat;//現在の熱量
    public Slider HeatMeter;//ヒートメーター
    public Image sliderImage;//メーターの描画部分
    private float currentY;//現在のy座標
    private float minY, maxY;//最小のy座標 最大のy座標
    private float dX;//デフォルトのx座標


    // Start is called before the first frame update
    void Start()
    {
        //各変数初期化
        HeatMeter.value = 1;
        currentHeat = 0;
        dX = HeatMeter.transform.localPosition.x;
        overHeat = false;
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
    /// HeatMeterを更新するメソッド
    /// </summary>
    /// <param name="diff">熱量に加算する値</param>
    public void HeatMeterUpdate(float diff)
    {

        currentHeat = currentHeat + diff;

        //熱量の現在値が最大値を超えたら最大値にする
        //オーバーーヒートをtureにする
        if (currentHeat >= maxHeat)
        {
            currentHeat = maxHeat;
            overHeat = true;
        }
        else
        {
            //超えてなかったら
            //熱量の現在値が0以下だったら
            //オーバーヒートをfalseにする
            if(currentHeat <= 0f)
            {
                currentHeat = 0f;
                overHeat = false;
            }
        }

        HeatMeter.value = currentHeat / maxHeat;


        //HeatMeterをY軸方向のどの位置まで移動しなければならないか計算
        currentY = MapValues(currentHeat, 0, maxHeat, minY, maxY);
        //HeatMeterを移動させる
        HeatMeter.transform.localPosition = new Vector3(dX, currentY, 0);

        //HeatMeterの色を変化させる
        
        if (currentHeat < maxHeat / 2)
        {
            //色はRGB表記
            //青→黄色
            sliderImage.color = new Color32((byte)MapValues(currentHeat, 0, maxHeat / 2, 0, 255),
                (byte)MapValues(currentHeat, 0, maxHeat / 2, 0, 255),
                (byte)MapValues(currentHeat, maxHeat / 2, maxHeat, 255, 0), 255);
        }
        else
        {
            //黄色→赤
            sliderImage.color = new Color32(255, (byte)MapValues(currentHeat, maxHeat / 2, maxHeat, 255, 0), 0, 255);
        }
    }
}
