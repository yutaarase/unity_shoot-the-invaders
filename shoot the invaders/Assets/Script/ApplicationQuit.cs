using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アプリケーションを終了させるクラス
/// </summary>
public class ApplicationQuit : MonoBehaviour
{
    //押されたとき
    public void OnClick()
    {
        //UNITY_EDITORだった時UniryEditorを終了させる
        //じゃなかったらアプリケーションを終了させる
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
