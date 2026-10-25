using UnityEngine;

public class GameController : MonoBehaviour
{

    void Start()
    {
        //center cursor and hide it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
