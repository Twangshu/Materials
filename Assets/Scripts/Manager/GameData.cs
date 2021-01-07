using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData 
{
    public static int currentWeight = 0;
    public static int maxWeight = 9999;
    private static int coins = 0;
    public static int exps = 0;

    public static int Coins { get => coins;}

    public static void GetCoins(int count)
    {
        coins += count;
        EventCenter.Broadcast(EventDefine.ShowGetItemMessage, "Sprites/other/coin", count);
    }

    public static void UseCoins(int count)
    {
        coins -= count;
    }

}
