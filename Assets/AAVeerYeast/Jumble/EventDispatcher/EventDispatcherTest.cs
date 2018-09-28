using UnityEngine;
using UnityEngine.EventSystems;
public class EventDispatcherTest : MonoBehaviour, IPointerClickHandler, ISelectHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        //  print(gameObject.name);
        //  print("name : " + EventSystem.current.name);
        //  print("currentSelectedGameObject : " + EventSystem.current.currentSelectedGameObject.name);
        //  print("name : " + EventSystem.current.currentInputModule.name);
        //  //print("name : " + EventSystem.current.firstSelectedGameObject.name);
        //  print("name : " + EventSystem.current.alreadySelecting);
    }

    public void OnSelect(BaseEventData eventData)
    {
        print("sel" + gameObject.name);
    }
}