using UnityEngine;

public class GameController : MonoBehaviour
{
    public CropFactory cropFactory;   // CropFactory�� ����
    public CropAttributes CornAttributes;  // ������ �۹� �Ӽ� ������

    void Start()
    {
        // ����: ���� ���� �� �� �۹��� ����
        cropFactory.CreateCrop(CornAttributes, new Vector3(0, 0, 0));
    }
}
