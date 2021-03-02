using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Transform originalParent;
    public Inventory myBag;

    public int currentItemID;

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        currentItemID = originalParent.GetComponent<Slot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        /*
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        }
        */
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject != null )
        {

            if (eventData.pointerCurrentRaycast.gameObject.name == "Item Image")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

                //swap itemID
                var temp = myBag.itemList[currentItemID];
                myBag.itemList[currentItemID] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;

                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

                //swap itemID with empty slot
                myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemList[currentItemID];

                if (eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID != currentItemID)
                {
                    myBag.itemList[currentItemID] = null;

                }
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
        }
        //其他位置都歸為

        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;






    }
}
