using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject button;
    public GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        button.SetActive(false);
        door.SetActive(false);
    }
}
