using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

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
        SpawnPlayer();
        SpawnPlayer();

        activePlayerIndex = 0;
        
        for (int i = 1; i < PlayerController.players.Count; i++)
        {
            PlayerController.players[i].SetControlActive(false);
            if (PlayerController.players[i].CameraFollower != null)
                PlayerController.players[i].CameraFollower.gameObject.SetActive(false);
        }

        FocusCameraOnActivePlayer();
    }


    private void SpawnPlayer()
    {
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

        PlayerController current = ActivePlayer;
        if (current != null)
            current.SetControlActive(false);

        activePlayerIndex++;
        if (activePlayerIndex >= PlayerController.players.Count)
            activePlayerIndex = 0;

        PlayerController next = ActivePlayer;
        if (next != null)
            next.SetControlActive(true);

        FocusCameraOnActivePlayer();
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