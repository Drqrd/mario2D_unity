using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    private static int score = 0;

    // All point values
    private const int stompedLakitu = 800;
    private const int stompedHammerBro = 1000;
    private const int firedEnemy = 200;
    private const int pointsPerSecond = 50;
    private static readonly int[] stompedEnemy = { 100, 200, 400, 500, 800, 1000, 2000, 4000, 5000, 8000 };
    private static readonly int[] shelledEnemy = { 500, 800, 1000, 2000, 4000, 5000, 8000 };
    private static readonly int[] flagpole = { 100, 400, 800, 2000, 5000 };
    

    public static void AddScore(int val)
    {
        score += val;
    }

    public static int GetScore()
    {
        return score;
    }

    public static int GetStompedEnemyPoints(int i)
    {
        if (i < stompedEnemy.Length) { return stompedEnemy[i]; }
        else { return 0; }
    }

    public static int GetShelledEnemyPoints(int i)
    {
        if (i < shelledEnemy.Length) { return shelledEnemy[i]; }
        else { return 0; }
    }
}
