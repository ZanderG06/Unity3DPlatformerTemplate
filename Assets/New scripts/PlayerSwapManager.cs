using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwapManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public static int activePlayerIndex { get; private set; }

    //private static ThirdPersonCamera mainCamera;
    public static PlayerController ActivePlayer
    {
        get
        {
            if (PlayerController.players.Count == 0) return null;
            return PlayerController.players[activePlayerIndex];
        }
    }

    private void Start()
    {
        SpawnPlayer();
        SpawnPlayer();

        // Ensure only the first player starts active
        activePlayerIndex = 0;
        
        for (int i = 1; i < PlayerController.players.Count; i++)
        {
            PlayerController.players[i].enabled = false;
            if (PlayerController.players[i].CameraFollower != null)
                PlayerController.players[i].CameraFollower.gameObject.SetActive(false);
        }

        FocusCameraOnActivePlayer();
    }


    private void SpawnPlayer()
    {
        // Instantiate inactive so we can set the flag before Start() runs
        GameObject playerObj = Instantiate(playerPrefab);
        playerObj.SetActive(false);

        if (playerObj.TryGetComponent(out PlayerController pc))
        {
            pc.JoinedThroughGameManager = true;
        }

        playerObj.SetActive(true);
    }

    public static void SwapToNextPlayer()
    {
        if (PlayerController.players.Count <= 1) return;

        // Disable current
        PlayerController current = ActivePlayer;
        if (current != null)
        {
            current.enabled = false;
            if (current.CameraFollower != null)
                current.CameraFollower.gameObject.SetActive(false);
        }

        // Move index
        activePlayerIndex++;
        if (activePlayerIndex >= PlayerController.players.Count)
            activePlayerIndex = 0;

        // Enable next
        PlayerController next = ActivePlayer;
        if (next != null)
        {
            next.enabled = true;
            if (next.CameraFollower != null)
                next.CameraFollower.gameObject.SetActive(true);
        }

        current.GetComponent<PlayerInput>().enabled = false;
        next.GetComponent<PlayerInput>().enabled = true;
    }

    static void FocusCameraOnActivePlayer()
    {
        PlayerController active = ActivePlayer;
        if (active == null) return;

        if (active.CameraFollower != null)
        {
            active.CameraFollower.SetTarget(active.transform);
            active.CameraFollower.gameObject.SetActive(true);
        }
    }
}