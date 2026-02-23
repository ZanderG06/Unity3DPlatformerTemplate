using UnityEngine;
using System.Collections.Generic;

public class PlayerSwapManager : MonoBehaviour
{
    public static int activePlayerIndex { get; private set; }

    public static PlayerController ActivePlayer
    {
        get
        {
            if (PlayerController.players.Count == 0) return null;
            return PlayerController.players[activePlayerIndex];
        }
    }

    public static void SwapToNextPlayer()
    {
        if (PlayerController.players.Count <= 1) return;

        //Disables the current active player
        PlayerController.players[activePlayerIndex].enabled = false;

        activePlayerIndex++;
        if(activePlayerIndex >= PlayerController.players.Count) activePlayerIndex = 0;

    }
}
