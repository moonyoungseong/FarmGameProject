using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLAccess : MonoBehaviour
{
    public class URLAccess : MonoBehaviour
    {
        // �� ������ ����
        public void OpenWebPage(string url)
        {
            Application.OpenURL(url);
        }
    }

    //public void MainPage() // ����
    //{
    //    // �ͳ���� ����Ʈ
    //    Application.OpenURL("https://www.greendaero.go.kr/");
    //}

    //public void CornPage()
    //{
    //    // ������ ����Ʈ
    //    Application.OpenURL("https://terms.naver.com/entry.naver?docId=1993362&cid=48180&categoryId=48247");
    //}
}
