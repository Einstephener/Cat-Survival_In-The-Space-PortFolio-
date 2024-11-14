using UnityEngine;

public class HideCursor : MonoBehaviour
{
    void Start()
    {
        // 커서를 숨기고 게임 창 안에 고정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 게임 중에 ESC 키를 누르면 커서를 다시 보이게 함
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
