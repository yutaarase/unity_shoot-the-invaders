using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EnemyのAimから発射までを操作するクラス
/// </summary>
public class Enemy_Shot: MonoBehaviour
{
    [Header("弾")] public GameObject bullet; // 弾
    [Header("弾の発射位置")] public Transform shotPoint; //弾の発射位置
    [Header("弾発射間隔")] public float shotInterval;
    [Header("射撃有効")] public bool isShot;
    [Header("親")] public GameObject parent;
    [Header("敵との距離")] public float range; //射程
    [Range(0,100)][Header("追尾速度")] public float rate;
    [Header("追尾設定有効")] public bool isAutoAim; //追尾を有効にする
    [Header("偏差射撃有効")] public bool isDeviation; //偏差射撃を有効にする
    [Header("照準角詳細設定有効")] public bool isAimAngleMode;
    [Header("照準角:x")] public float angle_x;
    [Header("照準角:y")] public float angle_y;
    [Header("照準角:z")] public float angle_z;
    [Header("照準角:all")] public float angle_all;
    [Header("追尾軸")] public AimAxis aimaxis;
    [Header("発射音")] public AudioClip audioClip;
    
    private float time;//経過時間
    private AudioSource audioSource;//AudioSource
    private Quaternion shotAngle;//発射する方向
    private GameObject target; //攻撃目標
    void Start()
    {
        //変数インスタンスの初期化
        //0.1秒間隔でAutoAimメソッドを呼び出す
        InvokeRepeating("AutoAim", 0.1f, 0.1f);
        audioSource = gameObject.GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        //ターゲットへのベクトルの長さが射程以下だったら
        time += Time.fixedDeltaTime;
        if (targetDistance() <= range)
        {
            //撃てるようになったら
            if (isShot)
            {
                //経過時間が発射間隔以上だったら
                //弾を発射する
                if (time >= shotInterval)
                {
                    Shot();
                    time = 0f;

                }
            }
        }
        
    }

    /// <summary>
    /// 弾を発射するメソッド
    /// </summary>
    void Shot()
    {
        //照準角詳細設定が有効だったら
        if (isAimAngleMode)
        {
            //ターゲットへの角度の絶対値がそれぞれ設定値以下だったら
            //弾のインスタンスを起こす
            //音楽を再生する
            if (Mathf.Abs(targetAngle("x")) <= angle_x && Mathf.Abs(targetAngle("y")) <= angle_y
                && Mathf.Abs(targetAngle("z")) <= angle_z)
            {
                Instantiate(bullet, shotPoint.position, Quaternion.LookRotation(targetVector()));
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
        else
        {
            //有効じゃなかったら
            //ベクトルの外積の絶対値が設定値以下だったら
            if (Mathf.Abs(targetAngle("all")) <= angle_all)
            {
                //偏差撃ちを行うかどうか
                if (isDeviation)
                {
                    //有効だったら
                    //ターゲットへのベクトルとターゲットの移動ベクトルを合成しその方向に向ける
                    shotAngle = Quaternion.LookRotation(targetVector() + target.GetComponent<PlayerControl>().horizontal_forward);
                }
                else
                {
                    //有効じゃなかったら
                    //ターゲットへのベクトルに向ける
                    shotAngle = Quaternion.LookRotation(targetVector());
                }
                //弾のインスタンスを起こす
                //音楽を再生する
                Instantiate(bullet, shotPoint.position, shotAngle);
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
    }

    /// <summary>
    /// 自動で照準を合わせるメソッド
    /// </summary>
    void AutoAim()
    {
        //オートエイムが有効かどうか
        if (isAutoAim)
        {
            //有効だったら
            //見やすくするのと使いやすくするため辞書型に
            Dictionary<string, Vector3> AimAxisMode = new Dictionary<string, Vector3>()
            {
                {"x",Vector3.right},
                {"y",Vector3.up},
                {"z",Vector3.forward},
                {"all",targetCross().normalized}

            };

            //指定された軸を中心にターゲットへのQuaternionを出す
            Quaternion quat = Quaternion.Slerp(parent.transform.rotation,
            Quaternion.LookRotation(targetVector(), AimAxisMode[aimaxis.ToString()]),
            rate / 100);
            
            //オイラー角に変換
            Vector3 rot = quat.eulerAngles;

            //各角度制限
            if(angle_x != 0)
            {
                if(rot.x <= angle_x)
                {
                    if (rot.x <= 10)
                    {
                        rot.x = 360;
                    }
                    else
                    {
                        rot.x = angle_x;
                    }
                }
            }
            if (angle_y != 0)
            {
                if (rot.y <= angle_y)
                {
                    rot.y = angle_y;
                }
            }

            //出力したオイラー角をrotationに設定
            transform.rotation = Quaternion.Euler(rot);
        }
    }

    /// <summary>
    /// 自身の射程のベクトルを取得するメソッド
    /// </summary>
    /// <remarks>自身の射程のベクトル</remarks>
    private Vector3 MyVector()
    {
        return transform.forward * range;
    }

    /// <summary>
    /// ターゲットへのベクトルを取得するメソッド
    /// </summary>
    /// <returns>ターゲットへのベクトル</returns>
    private Vector3 targetVector()
    {
        return target.transform.position - transform.position;
    }


    /// <summary>
    /// 自身とターゲットへのベクトルの外積
    /// </summary>
    /// <returns>ターゲットへのベクトルの外積</returns>
    private Vector3 targetCross()
    {
        return Vector3.Cross(MyVector(), targetVector());
    }


    /// <summary>
    /// ターゲットとの角度を取得するメソッド
    /// </summary>
    /// <param name="mode">角度の種類</param>
    /// <returns>ターゲットとの角度</returns>
    private float targetAngle(string mode) 
    {
        var a = new Vector3(0f, 0f, targetDistance());
        var b = target.transform.position - transform.position;
        float angle = 0f;
        switch (mode)
        {
            case "x":
                angle = Vector3.SignedAngle(a, b, Vector3.right);
                break;
            case "y":
                angle = Vector3.SignedAngle(a, b, Vector3.up);
                break;
            case "z":
                angle = Vector3.SignedAngle(a, b, Vector3.forward);
                break;
            case "all":
                angle = Vector3.SignedAngle(MyVector(), targetVector(), targetCross().normalized);
                break;
            default:
                angle = 0f;
                break;
        }

        return angle; 
    }



    /// <summary>
    /// ターゲットとの距離
    /// </summary>
    /// <returns>ターゲットとの距離</returns>
    private float targetDistance() 
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    /// <summary>
    /// エイムの回転軸
    /// </summary>
    public enum AimAxis
    {
        x,y,z
    }


}
