using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyの移動を操作するクラス
/// </summary>
public class Enemy_Move : MonoBehaviour
{
    //今思うと群れで実装を行うことで良い感じの動きが実装出来た気がする
    //いい味の出た動きをしているのでそのままにする予定

    [Header("移動速度")] public float speed;
    [Header("旋回速度")] public float turn;
    [Header("移動有効")] public bool isMove;
    [Header("ランダム旋回有効")] public bool isRandTurn;
    //通常のベクトル
    private Vector3 normalVector = Vector3.zero;
    //回転有効
    private bool isTurn = false;
    //回転間隔
    private float turnTime;
    //回転経過時間
    private float currentTurnTime;

    private void Start()
    {
        //変数初期化
        turnTime = 10;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (isMove)
        {
            //移動が有効だったら

            //物に衝突したら回転する
            var contacts = collision.contacts[0];
            if (contacts.point.y >= transform.position.y + 2)
            {
                isTurn = true;
            }
            else isTurn = false;

            if (isTurn)
            {
                //回転が有効だったら
                //衝突x座標が現在x座標より大きかったら
                if (contacts.point.x > transform.position.x)
                {
                    //逆時計回りに回転する
                    transform.rotation = Quaternion.Euler(0f, -turn, 0f) * transform.rotation;
                }
                else
                {
                    //時計回りに回転する
                    transform.rotation = Quaternion.Euler(0f, turn, 0f) * transform.rotation;
                }
            }
            else
            {
                //ランダムな回転を行う
                if (isRandTurn)
                {
                    //ランダム回転が有効だったら
                    if(currentTurnTime >= turnTime)
                    {
                        transform.rotation *= Quaternion.Euler(0f, Random.Range(-5, 5), 0f);
                        currentTurnTime = 0;
                        turnTime = Random.Range(1, 10);
                    }
                    currentTurnTime += Time.deltaTime;
                }
                //接触した点における法線を取得
                normalVector = contacts.normal;
                Vector3 inputVector = Vector3.zero;
                //inputVector.x = Input.GetAxis("Horizontal");
                inputVector.z = 0.3f;

                // 平面に沿ったベクトルを計算
                Vector3 onPlane = Vector3.ProjectOnPlane(inputVector, normalVector);

                Rigidbody rb = GetComponent<Rigidbody>();
                rb.AddRelativeForce(onPlane * speed);
            }

        }
        
    }

}
