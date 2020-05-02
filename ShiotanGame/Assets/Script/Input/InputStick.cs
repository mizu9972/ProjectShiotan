using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// このスクリプトを使う場合、毎フレームUpdateを呼び出してください
/// </summary>
public class InputStick
{
    public float Operationlimit = 0.5f;
    Vector2 Stick = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
    bool Right = false;
    bool Left = false;
    bool Up = false;
    bool Down = false;

    public  void StickUpdate() {
        Stick = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        // トリガー状態の更新
        {
            if (Right) {
                if (Stick.x < Operationlimit) {
                    Right = false;
                }
            }
            if (Left) {
                if (Stick.x > -Operationlimit) {
                    Left = false;
                }
            }
            if (Up) {
                if (Stick.y < Operationlimit) {
                    Up = false;
                }
            }
            if (Down) {
                if (Stick.y > -Operationlimit) {
                    Down = false;
                }
            }
        }
    }

    public bool GetRightStick() {
        // トリガー状態の確認
        if (!Right) {
            if (Stick.x >= Operationlimit) {
                Right = true;
                return true;
            }
        }
        return false;
    }

    public bool GetLeftStick() {
        // トリガー状態の確認
        if (!Left) {
            if (Stick.x <= -Operationlimit) {
                Left = true;
                return true;
            }
        }
        return false;
    }

    public bool GetUpStick() {
        // トリガー状態の確認
        if (!Up) {
            if (Stick.y >= Operationlimit) {
                Up = true;
                return true;
            }
        }
        return false;
    }

    public bool GetDownStick() {
        // トリガー状態の確認
        if (!Down) {
            if (Stick.y <= -Operationlimit) {
                Down = true;
                return true;
            }
        }
        return false;
    }
}
