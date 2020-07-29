using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    //オール漕ぐアニメーション
    Animator _animator;
    //public MoveAnimation moveAnimation;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAnimation()
    {
        //アニメーション最初から再生
        _animator.Play("Move", 0, 0.0f);
        //moveAnimation.SetAnimationStream();
    }
}
