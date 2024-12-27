using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLAccess : MonoBehaviour
{
    // 웹 페이지 열기
    public void OpenWebPage(string url)
    {
        Application.OpenURL(url);
    }
}
