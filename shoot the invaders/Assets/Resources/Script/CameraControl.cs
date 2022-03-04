using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラを操作するクラス
/// </summary>
public class CameraControl : MonoBehaviour
{
    [Header("第一カメラ")] public GameObject FirstCamera;
    [Header("第二カメラ")] public GameObject SecondCamera;
    [Header("第三カメラ")] public GameObject ThirdCamera;
    [Header("第四カメラ")] public GameObject FourthCamera;

    //視点移動速度
    public float rotateSpeed = 2.0f;
    //Playerのオブジェクト
    private GameObject player;
    //カメラが動けるかどうか
    private bool cameraMove = false;

    // Start is called before the first frame update
    void Start()
    {
        //各変数初期化
        //カメラオブジェクト表示設定
        SecondCamera.SetActive(false);
        ThirdCamera.SetActive(false);
        FourthCamera.SetActive(false);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //三人称視点 第一カメラを表示
        if (Input.GetKey(KeyCode.F1) || Input.GetKey(KeyCode.Alpha1))
        {
            FirstCamera.SetActive(true);
            SecondCamera.SetActive(false);
            ThirdCamera.SetActive(false);
            FourthCamera.SetActive(false);
            cameraMove = false;
        }

        //爆弾投下視点　第四カメラを表示
        if (Input.GetKey(KeyCode.Alpha2))
            {
            FirstCamera.SetActive(false);
            SecondCamera.SetActive(false);
            ThirdCamera.SetActive(false);
            FourthCamera.SetActive(true);
            cameraMove = false;
        }

        //一人称視点 第二カメラを表示
        if (Input.GetKey(KeyCode.F2))
        {
            FirstCamera.SetActive(false);
            SecondCamera.SetActive(true);
            ThirdCamera.SetActive(false);
            FourthCamera.SetActive(false);
            cameraMove = false;
        }

        //フリールックカメラ 第三カメラ表示
        if (Input.GetMouseButton(1))
        {
            FirstCamera.SetActive(false);
            SecondCamera.SetActive(false);
            ThirdCamera.SetActive(true);
            FourthCamera.SetActive(false);
            cameraMove = true;
            //Vector3でX,Y方向の回転の度合いを定義
            Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed, Input.GetAxis("Mouse Y") * rotateSpeed, 0);

            //transform.RotateAround()をしようしてメインカメラを回転させる
            ThirdCamera.transform.RotateAround(player.transform.position, Vector3.up, angle.x);
            ThirdCamera.transform.RotateAround(player.transform.position, transform.right, angle.y);
        }

        //右クリックを話したのでフリールック解除第一カメラ表示
        if (Input.GetMouseButtonUp(1))
        {
            FirstCamera.SetActive(true);
            SecondCamera.SetActive(false);
            ThirdCamera.SetActive(false);
            FourthCamera.SetActive(false);
            cameraMove = false;
        }

        //cameraMoveがtrueじゃなかったら第三カメラを第一カメラの座標に移動
        if (!cameraMove)
        {
            ThirdCamera.transform.position = FirstCamera.transform.position;
            ThirdCamera.transform.rotation = FirstCamera.transform.rotation;
        }
    }
}
