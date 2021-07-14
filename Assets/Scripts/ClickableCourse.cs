using UnityEngine;

public class ClickableCourse : MonoBehaviour
{
    public int index = -1;
    void OnMouseDown()
    {
        OldMenu menu = GameObject.Find("Menu").GetComponent<OldMenu>();
        menu.onClick(index);
    }

    public void onclickInput()
    {
        CourseCodeController cc = GameObject.Find("Canvas").GetComponent<CourseCodeController>();
        cc.onclickInput();
    }
}

