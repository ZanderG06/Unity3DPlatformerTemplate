using UnityEditor;
using UnityEngine;

public class PlayerSwapManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public static int activePlayerIndex { get; private set; }

    private static ThirdPersonCamera mainCamera;
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
    }

    public static void RegisterCamera(ThirdPersonCamera camera)
    {
        mainCamera = camera;
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

        PlayerController previous = PlayerController.players[activePlayerIndex];
        previous.enabled = false;
        if (previous.CameraFollower != null)
            previous.CameraFollower.gameObject.SetActive(false);

        activePlayerIndex = (activePlayerIndex + 1) % PlayerController.players.Count;

        PlayerController next = PlayerController.players[activePlayerIndex];
        next.enabled = true;
        FocusCameraOnActivePlayer();
    }

    private static void FocusCameraOnActivePlayer()
    {
        if (mainCamera == null) return;

        PlayerController active = ActivePlayer;
        if (active == null) return;

        mainCamera.SetTarget(active.transform);
    }
}