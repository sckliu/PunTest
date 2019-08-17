using UnityEngine;

public class moveMOUSE : MonoBehaviour
{
    public GameObject Cam;//攝影機
    public Vector3 KeyposMouse;//滑鼠座標
    public float KeyDownposMouse;//滑鼠位置暫存

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            KeyposMouse = new Vector3(Input.mousePosition.x, 0, 0);
            KeyDownposMouse = KeyposMouse.x;
        }
        if (Input.GetMouseButton(0))//按住左鍵
        {
            //取得滑鼠座標 - 屬於螢幕座標
            if(KeyDownposMouse < Input.mousePosition.x)
            {
                print("右邊");
                Cam.transform.position += new Vector3(1 *Time.deltaTime, 0, 0);
            }
            else
            {
                print("左邊");
                Cam.transform.position -= new Vector3(1 * Time.deltaTime, 0, 0);
            }
        
        }
    }
}
