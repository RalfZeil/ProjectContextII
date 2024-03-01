using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 worldPosition;

    [SerializeField] private GameObject cursorObject;


    private void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000))
        {
            worldPosition = hitData.point;
            UpdateCursurObjectPosition(worldPosition);

            if (Input.GetMouseButtonDown(0))
            {
                TryInteract(hitData.transform.gameObject);
            }

            TryHighlight(hitData.transform.gameObject);
        }
    }

    private void UpdateCursurObjectPosition(Vector3 point)
    {
        if (cursorObject != null)
        {
            cursorObject.transform.position = point;
        }
    }

    private void TryInteract(GameObject target)
    {
        IInteractable interactable = target.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    private void TryHighlight(GameObject target)
    {
        IInteractable interactable = target.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Highlight();
        }
    }
}
