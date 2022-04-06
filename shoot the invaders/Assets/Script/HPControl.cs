using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵やメインターゲットのHPを制御するクラス
/// </summary>
public class HPControl : MonoBehaviour
{
    [Header("体力")] public float hp;
    [Header("獲得ポイント")] public int point;
    [Header("爆発エフェクト")] public GameObject explosion;
    private GameObject controler;//遷移のコントローラー


    private void Start()
    {
        //オブジェクトを取得
        controler = GameObject.Find("SceneControler");
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Playerの爆弾に衝突したらHPを減らす
        if (collision.collider.tag == "P_Bullet")
        {
            hp -= collision.gameObject.GetComponent<Bullet>().power;
        }

        //Playerに衝突したらHPを0にする
        if (collision.collider.tag == "Player")
        {
            hp = 0;
            GameObject ex = Instantiate(explosion) as GameObject;
            ex.transform.position = this.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Playerの爆弾の中に入ったら継続ダメージを与える
        //衝撃波の実装
        if (other.tag == "P_Bomb")
        {
            hp -= other.gameObject.GetComponent<Bomb>().power;
        }
    }

    private void FixedUpdate()
    {
        //HPが0以下になったら爆発エフェクトのインスタンスを起こす
        //オブジェクトを削除する
        //自身のタグがMainTargetだったらTaskManagerのターゲット数を減らす
        if (hp <= 0f)
        {
            GameObject ex = Instantiate(explosion) as GameObject;
            ex.transform.position = this.transform.position;
            Destroy(this.gameObject);
            controler.GetComponent<SceneControl>().score += point;
            if (this.tag == "MainTarget")
            {
                GameObject.FindGameObjectWithTag("TaskManager").GetComponent<TaskManager>().currentCnt--;
            }

        }
    }

}
