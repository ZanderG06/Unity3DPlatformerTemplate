using UnityEngine;
using System.Collections.Generic;

public class PlayerSwapManager : MonoBehaviour
{
    public static int currentPlayerIndex { get; private set; }

    public static PlayerController ActivePlayer
    {
        get
        {
            if (PlayerController.players.Count == 0) return null;
            return PlayerController.players[currentPlayerIndex];
        }
    }
}
