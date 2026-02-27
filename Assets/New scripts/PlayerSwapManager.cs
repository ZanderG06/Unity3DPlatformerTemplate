using UnityEngine;
using System.Collections.Generic;

public class PlayerSwapManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public static int activePlayerIndex { get; private set; }

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
        // Spawn the initial player
        SpawnPlayer();

        // Spawn the second player
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject playerObj = Instantiate(playerPrefab);
        PlayerController playerController = playerObj.GetComponent<PlayerController>();
        playerController.JoinedThroughGameManager = true;
    }

    public static void SwapToNextPlayer()
    {
        if (PlayerController.players.Count <= 1) return;

        // Disable the current active player and their camera
        PlayerController previousPlayer = PlayerController.players[activePlayerIndex];
        previousPlayer.enabled = false;
        previousPlayer.CameraFollower.gameObject.SetActive(false);

        // Switch to the next player
        activePlayerIndex++;
        if (activePlayerIndex >= PlayerController.players.Count)
            activePlayerIndex = 0;

        // Enable the new active player and their camera  
        PlayerController.players[activePlayerIndex].enabled = true;
        FocusCameraOnActivePlayer();
    }

    private static void FocusCameraOnActivePlayer()
    {
        PlayerController active = ActivePlayer;
        if (active == null || active.CameraFollower == null) return;

        active.CameraFollower.transform.position = active.transform.position;
        active.CameraFollower.gameObject.SetActive(true);
    }
}
