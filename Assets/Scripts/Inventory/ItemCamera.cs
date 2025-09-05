using UnityEngine;

public class ItemCamera : MonoBehaviour, IUsable
{
    public GameObject cameraScreen; // Reference to CameraScreen UI Canvas
    public ItemCameraEffects itemCameraEffects; // Item Camera Effects


    public void Use()
    {
        TakePhoto();
    }

    private void TakePhoto()
    {
        // When Press Attack
        itemCameraEffects.CameraAction();

        // Detect documents in the camera's view
        DetectDocument();
    }

    private void DetectDocument()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f)) // Adjust the range as needed
        {
            if (hit.collider.CompareTag("Document"))
            {
                Debug.Log($"Document Found: {hit.collider.name}");

                // Get the clue identifier from the document
                Document doc = hit.collider.GetComponent<Document>();
                if (doc != null)
                {
                    // Notify the Clue Board Controller
                    ClueBoardController.Instance.UpdateClueSlot(doc.clueID);
                }
            }
        }
    }
}
