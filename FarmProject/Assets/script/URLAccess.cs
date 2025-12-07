using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// URLAccess.cs
/// 특정 URL을 외부 브라우저에서 열어주는 기능을 담당하는 클래스.
/// 버튼 클릭 이벤트 등에 연결하여 사용한다.
/// </summary>
public class URLAccess : MonoBehaviour
{
    /// <summary>
    /// 전달받은 URL을 기본 웹 브라우저로 열어주는 함수.
    /// Unity의 Application.OpenURL을 사용
    /// </summary>
    /// <param name="url">열고자 하는 웹 페이지 주소</param>
    public void OpenWebPage(string url)
    {
        Application.OpenURL(url);
    }
}
