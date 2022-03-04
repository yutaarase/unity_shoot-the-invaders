using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Playerを操作するクラス
/// </summary>
public class PlayerControl : MonoBehaviour
{
    [Range(0, 200)]
    [Header("最高速度")]  public float maxspeed;
    [Header("速度")] public float speed;
    [Header("弾")] public GameObject bullet;
    [Header("弾の発射位置")] public Transform shotPoint;
    [Header("弾発射間隔")] public float shotInterval;
    [Header("熱発生量")] public float heat;
    [Header("爆弾積載量")] public int bombNum;
    [Header("爆弾")] public GameObject bomb;
    [Header("弾着弾地点サークル")] public GameObject circle;
    [Header("爆弾着弾地点サークル")] public GameObject Bcircle;
    [Header("発射音")] public AudioClip audioClip;

    [System.NonSerialized]
    public Vector3 horizontal_forward;//現在向いている方向のベクトル
    private GameObject sceneControler;//シーン遷移コントローラー
    private Text bombNumText; //爆弾の数を描画するテキスト
    private bool isBombShot; //爆弾を投下出来るかどうか
    private float minspeed; //最低速度
    private float time; //機関銃を発射してからの経過時間
    private AudioSource audioSource;
    private Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        //オブジェクトの取得
        //各変数インスタンス初期化
        sceneControler = GameObject.Find("SceneControler");
        var texts = GameObject.FindObjectsOfType<Text>();
        foreach (var text in texts)
        {
            if(text.name == "BombNum")
            {
                bombNumText = text;
            }
        }
        minspeed = speed;
        time = 0f;
        isBombShot = false;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //機関銃攻撃処理
        if (Input.GetMouseButton(0) && !isBombShot)
        {
            //熱量メーターのスクリプト取得
            HeatMeterControl HMC = GetComponentInParent<HeatMeterControl>();

            //オーバーヒートしたかどうか
            if (HMC.overHeat == false)
            {
                //経過時間加算
                //熱量メーター更新(加算)
                time += Time.deltaTime;
                HMC.HeatMeterUpdate(heat * Time.deltaTime);
                //経過時間が発射間隔以上だったら
                if (time >= shotInterval)
                {
                    //発射音再生
                    //弾のインスタンスを起こす
                    //経過時間初期化
                    audioSource.clip = audioClip;
                    audioSource.Play();
                    Instantiate(bullet, shotPoint.position, Quaternion.LookRotation(transform.forward * speed));
                    time = 0f;
                }
            }
            else
            {
                //オーバーヒートしてるので冷やす
                //熱量メーター更新(減算)
                HMC.HeatMeterUpdate(-1 * heat * Time.deltaTime);

            }
            
        }
        else
        {
            //弾を撃ってないので冷やす
            //熱量メーターのスクリプト取得
            //熱量メーター更新(減算)
            HeatMeterControl HMC = GetComponentInParent<HeatMeterControl>();
            HMC.HeatMeterUpdate(-1* heat * Time.deltaTime);
            
        }

        //爆弾攻撃処理
        if (Input.GetMouseButtonDown(0))
        {
            //爆弾を撃てるかどうか
            if (isBombShot)
            {
                //爆弾の在庫があるかどうか
                if (bombNum > 0)
                {
                    //爆弾投下
                    //爆弾の在庫を減らす
                    Instantiate(bomb, transform);
                    --bombNum;

                }
            }
        }

        //爆弾を撃てるかどうかを指定 通常視点の時
        if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.F1))
        {
            isBombShot = false;
        }

        //爆弾を撃てるかどうか指定 爆弾投下視点の時
        if (Input.GetKey(KeyCode.Alpha2))
        {
            isBombShot = true;
        }

        
        //着弾地点表示
        //変数初期化
        float height = 0f;
        float b_range = 0f;

        //爆弾投下出来るかどうか
        if (isBombShot)
        {
            //爆弾投下出来るなら
            //現在座標に対応したTerranのy座標を取得する
            height = Terrain.activeTerrain.terrainData.GetInterpolatedHeight(
                    transform.position.x / Terrain.activeTerrain.terrainData.size.x,
                    transform.position.z / Terrain.activeTerrain.terrainData.size.z);
            //爆弾着弾地点のサークルの位置を設定
            Bcircle.transform.position = new Vector3(transform.position.x, height+1f, transform.position.z);
        }
        else
        {
            //爆弾投下出来ないなら
            //弾のクラス取得
            Bullet b_code = bullet.GetComponent<Bullet>();

            //b_rangeが弾の飛距離以下の間ループを行う
            //弾の着弾地点の座標を取得する処理
            //now:2022/02/28 
            //今思うとRayCastで良かった気がする
            while (b_range <= b_code.range)
            {
                var position = transform.position + transform.forward * b_range;
                //Terrainの高さを取得
                height = Terrain.activeTerrain.terrainData.GetInterpolatedHeight(
                    position.x / Terrain.activeTerrain.terrainData.size.x,
                    position.z / Terrain.activeTerrain.terrainData.size.z);
                if (position.y <= height)
                {
                    //弾の着弾地点の座標設定
                    circle.transform.position = new Vector3(position.x, height + 0.1f, position.z);
                    break;
                }
                b_range += 1f;

            }
        }
        //Scene遷移を制御するクラスのPlayerが死んだかどうかを更新
        //爆弾数を更新
        sceneControler.GetComponent<SceneControl>().playerDeath
            = GetComponentInParent<HPBarControl>().death;
        bombNumText.text = bombNum.ToString();
    }

	private void FixedUpdate()
	{
        //垂直方向,平行方向の入力を取得 (-1~1)
        var hori = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");

        //入力によってスピードを変える
        if (vert > 0 || hori != 0 )
        {
            if(speed < maxspeed) ++speed;
        } else if(speed > minspeed)
        {
            --speed;
        }

        //トルクをかける
        if (rb.angularVelocity.magnitude < 0.5f)
        {
            rb.AddRelativeTorque(new Vector3(0, hori, -hori));
        }

        rb.AddRelativeTorque(new Vector3(vert, 0, 0));

        //x軸方向のトルク
        var left = transform.TransformVector(Vector3.left);
        var horizontal_left = new Vector3(left.x, 0f, left.z).normalized;
        rb.AddTorque(Vector3.Cross(left, horizontal_left) * 3);

        //z軸方向のトルク
        var forward = transform.TransformVector(Vector3.forward);
        horizontal_forward = new Vector3(forward.x, 0f, forward.z).normalized;
        rb.AddTorque(Vector3.Cross(forward, horizontal_forward) * 7);

        //正面方向への移動
        var force = (rb.mass * rb.drag * speed / 3.6f) / (1f - rb.drag * Time.fixedDeltaTime);
        rb.AddRelativeForce(new Vector3(0f, 0f, force));
    }

    private void OnCollisionStay(Collision collision)
    {
        //地面に衝突している間継続ダメージを与える  
        if (collision.collider.tag == "Terrain")
        {
            HPBarControl hpbarcontrol = GetComponentInParent<HPBarControl>();
            hpbarcontrol.HPBarUpdate(1);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //敵の弾に衝突したらHPを減らす
        if (collision.collider.tag == "E_Bullet")
        {
            Bullet bullet = collision.collider.gameObject.GetComponent<Bullet>();
            HPBarControl hpbarcontrol = GetComponentInParent<HPBarControl>();
            hpbarcontrol.HPBarUpdate(bullet.power);

        }

        //メインターゲットまたは敵に衝突したら一定ダメージを与え自身もダメージを受ける
        if (collision.collider.tag == "MainTarget" 
            || collision.collider.tag == "Enemy")
        {
            HPBarControl hpbarcontrol = GetComponentInParent<HPBarControl>();
            hpbarcontrol.HPBarUpdate(20);
        }

        //アイテムに衝突したらHPを回復させる
        //アイテムは消される
        if (collision.collider.tag == "Item")
        {
            HPBarControl hpbarcontrol = GetComponentInParent<HPBarControl>();
            hpbarcontrol.HPBarUpdate(-20);
            Destroy(collision.collider.gameObject);
        }
    }

}
