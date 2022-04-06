using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全ての弾の挙動を統括するクラス
/// </summary>
public class Bullet : MonoBehaviour
{
    
    [Header("威力")] public int power; //弾攻撃力
    [Header("弾の速度")] [SerializeField] float speed; //弾の移動速度
    [Header("射程距離")] public int range; //射程距離
    [Header("射程設定有効")] [SerializeField] bool isRange; //射程距離を有効にする
    [Header("追尾目標")] [SerializeField] string targetname; //追尾目標
    [Header("バネ係数")] [SerializeField] float spring_ratio; //追尾目標
    [Header("弾種類")] [SerializeField] BulletType bullettype; //弾種類
    [Header("爆発エフェクト")] [SerializeField] GameObject explosion;
    
    
    private float TotalMovement = 0; //移動総量
    private string targetTag; //攻撃目標のタグ
    private string terrainTag = "Terrain"; //地面のタグ
    private GameObject target;
    Rigidbody rb;
    
    private float period = 4f;
    Vector3 velocity;
    

    private void Start()
    {
        //PlayerとEnemyどっちも流用するためタグでの指定をなくし、Stringで攻撃目標を入力するようにした
        //今思うとEnumで選択出来るようにした方が使いやすかったと思う
        if(targetname != null && targetname != "")
        {
            target = GameObject.Find(targetname);
            targetTag = target.tag;
        }
        //名前の指定がなくタグがnullだった時はEnemyを指定するエラー回避目的
        if(targetTag == null || targetTag == "")
        {
            targetTag = "Enemy";
        }
        rb = GetComponent<Rigidbody>();
        velocity = transform.forward.normalized * speed;
        
    }


    void FixedUpdate()
    {    
        //射程距離がオンだったら
        if (isRange == true)
        {
            //射程距離が移動総量以上だったら        
            if (range >= TotalMovement)
            {
                //弾の挙動
                Movement();            }
            else
            {
                //射程を超えたら
                //4分の1の確率で爆発エフェクトのインスタンスを起こす
                //起こしたら自身を消す
                if (Random.Range(0,3) == 1)
                {
                    GameObject ex = Instantiate(explosion) as GameObject;
                    ex.transform.position = transform.position;
                }
                Destroy(this.gameObject);
            }
        }
        else
        {
            //射程距離がオンじゃなかったら
            //弾の挙動
            Movement();
        }

    }
        
     
    /// <summary>
    /// 弾の移動を制御するメソッド
    /// </summary>
    void Movement()
    {
        //弾の種類によって挙動を変える
        switch (bullettype.ToString())
        {
            case "NomalBullet":
                //通常弾だったら
                //正面方向に力を加える
                rb.velocity = velocity;
                TotalMovement += velocity.magnitude;
                break;
            case "Missile1":
                //ミサイル種類1
                //バネみたいな動きをする追尾
                if (targetVector().magnitude > 20f)
                {
                    var rot = Quaternion.LookRotation(targetVector());
                    var q = rot * Quaternion.Inverse(transform.rotation);
                    if (q.w < 0f)
                    {
                        q.x = -q.x;
                        q.y = -q.y;
                        q.z = -q.z;
                        q.w = -q.w;
                    }
                    var torque = new Vector3(q.x, q.y, q.z) * spring_ratio;
                    rb.AddTorque(torque);
                    
                }
                rb.velocity = transform.forward * speed;
                TotalMovement += (transform.forward * speed).magnitude;
                break;
            case "Missile2":
                //ミサイル種類2
                //確定で当たる追尾
                //流石に強すぎるので衝突までの残り時間が0.1以下の時
                //追尾をやめるようにする
                var acceleration = Vector3.zero;
                
                acceleration += (targetVector() - velocity * period) * 2f / (period * period);
                period -= Time.fixedDeltaTime;
                if(period < 0.1f)
                {
                    rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
                    return;
                }
                if (acceleration.magnitude > 100f)
                {
                    acceleration += acceleration.normalized * 100f;
                }
                velocity += acceleration * Time.fixedDeltaTime;

                //transform.position += velocity * Time.fixedDeltaTime;
                rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
                transform.rotation = Quaternion.LookRotation(acceleration);
                TotalMovement += acceleration.magnitude * Time.fixedDeltaTime;
                break;
            default:
                //何もしていがないとき
                //通常弾と同じ挙動
                rb.velocity = velocity;
                TotalMovement += velocity.magnitude;
                break;
        }
    }

    //衝突判定
    private void OnCollisionEnter(Collision collision)
    {
        //ターゲット、メインターゲット、地面に衝突したら
        if (collision.collider.tag == targetTag || collision.collider.tag == terrainTag
            || collision.collider.tag == "MainTarget")
        {
            //衝突位置に爆発エフェクトオブジェクトのインスタンスを起こす
            //起こしたら自身を消す
            foreach (ContactPoint collisionpoint in collision.contacts)
            {

                GameObject ex = Instantiate(explosion) as GameObject;
                ex.transform.position = collisionpoint.point;
                
            }
            Destroy(this.gameObject);

        }

    }

    /// <summary>
    ///　ターゲットへのベクトルを取得するメソッド
    /// </summary>
    /// <returns>ターゲットへのベクトル</returns>
    private Vector3 targetVector()
    {
        return target.transform.position - transform.position;
    }

    /// <summary>
    /// 弾の種類
    /// </summary>
    public enum BulletType
    {
        NomalBullet,Missile1,Missile2
    }
}
