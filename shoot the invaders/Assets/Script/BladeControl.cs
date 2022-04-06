using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerのBladeの挙動と正面の当たり判定のクラス
/// </summary>
public class BladeControl : MonoBehaviour
{
    //Z軸の角度
    private float z;
    //回転速度
    private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //各変数初期化
        z = 0.0f;
        rotationSpeed = 1260.0f;
    }

    private void FixedUpdate()
    {
        //Bladeの回転処理
        z += Time.fixedDeltaTime * rotationSpeed;

        //360を超えると初期値に戻す
        if (z > 360.0f)
        {
            z = 0.0f;
        }

        transform.localRotation = Quaternion.Euler(0f, 0f, z);
    }

    private void OnTriggerEnter(Collider collider)
    {
        //正面衝突の処理
        //敵の弾に衝突すると弾の攻撃力分ダメージを受ける
        if(collider.tag == "E_Bullet")
        {
            Bullet bullet = collider.gameObject.GetComponent<Bullet>();
            HPBarControl hpbarcontrol = GetComponentInParent<HPBarControl>();
            hpbarcontrol.HPBarUpdate(bullet.power);
            
        }

        //地面に衝突すると即死する
        if(collider.tag == "Terrain")
        {
            HPBarControl hpbarcontrol = GetComponentInParent<HPBarControl>();
            hpbarcontrol.HPBarUpdate(hpbarcontrol.maxHp);
            
        }

        //敵やメインターゲットに衝突するとHPの半分のダメージを受ける
        if(collider.tag == "Enemy" || collider.tag == "MainTarget")
        {
            HPBarControl hpbarcontrol = GetComponentInParent<HPBarControl>();
            hpbarcontrol.HPBarUpdate(hpbarcontrol.maxHp / 2);
        }

        //アイテムを取得すると回復する
        if(collider.tag == "Item")
        {
            HPBarControl hpbarcontrol = GetComponentInParent<HPBarControl>();
            hpbarcontrol.HPBarUpdate(-20);
            Destroy(collider.gameObject);
        }
    }
}