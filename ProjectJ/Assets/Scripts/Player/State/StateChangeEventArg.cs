using UnityEngine;
using System.Collections;

/// <summary>
/// 스테이트 변경시 전달되는 인자값
/// </summary>
public class StateChangeEventArg
{
    public PlayerState PreState { get; set; }    ///< 이전 상태
}
