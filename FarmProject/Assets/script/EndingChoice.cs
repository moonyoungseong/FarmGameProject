using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EndingChoice.cs
/// 
/// - 게임의 엔딩 선택 결과를 저장하는 정적 클래스
/// </summary>
public static class EndingChoice
{
    /// <summary>
    /// 플레이어가 선택한 엔딩 타입을 저장하는 변수.
    /// 기본값은 EndingA.
    /// </summary>
    public static EndingType SelectedEnding = EndingType.EndingA;
}
