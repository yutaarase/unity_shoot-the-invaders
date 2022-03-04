using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆弾操作するクラス
/// </summary>
public class Bomb : MonoBehaviour
{
    [Header("威力")] public int power;
    [Header("爆発")] public GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        //Player以外に当たると爆発する処理
        if (collision.collider.tag != "Player")
        {
            //衝突した座標に爆発エフェクトのインスタンスを起こす
            //起こし終わったら自身を消す
            foreach (ContactPoint collisionpoint in collision.contacts)
            {

                GameObject ex = Instantiate(explosion) as GameObject;
                ex.transform.position = collisionpoint.point;

            }
            Destroy(this.gameObject);
        }
    }

}
