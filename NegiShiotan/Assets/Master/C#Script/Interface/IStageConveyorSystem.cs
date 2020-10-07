using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//コンベアシステムで利用するインターフェース
public interface IStageConveyorSystem
{
    void OnEndLineSystem(GameObject obj);
    void OnFallLineSystem(float FallEndPositionY_);
}
