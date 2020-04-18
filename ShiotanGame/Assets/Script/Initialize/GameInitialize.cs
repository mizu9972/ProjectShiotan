using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialize : MonoBehaviour
{
    private static int GameWidth = 1920;//横幅
    private static int GameHeight = 1080;//縦幅
    private static int FrameRate = 60;//フレームレート
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(GameWidth, GameHeight, false, FrameRate);//1920×1080のウインドウモードでfps60に設定
        Debug.Log("画面サイズ:"+GameWidth+"×"+GameWidth+"FPS:"+FrameRate);
    }
}