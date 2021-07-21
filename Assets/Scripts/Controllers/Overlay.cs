using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    private TextMeshPro score;
    private TextMeshPro coins;
    private TextMeshPro world;
    private TextMeshPro time;
    private Image coinSprite;
    private Vector3 offset;
    private Vector2 dimensions;

    private static int scoreCount = 0, coinCount = 0;
    private int timeLeft = 400;

    public Sprite[] sprites = new Sprite[4];
    private int currSprite = 0;

    private void Start()
    {
        GetComponents();

        PositionUI();
        RescaleUI();

        StartCoroutine(SecondTimer());
        StartCoroutine(FourthOfSecondTimer());
    }

    private void Update()
    {
        UpdateOverlay();
    }

    private void GetComponents()
    {
        score = transform.Find("Score").GetComponent<TextMeshPro>();
        coins = transform.Find("Coins").GetComponent<TextMeshPro>();
        coinSprite = transform.Find("Coin Sprite").GetComponent<Image>();
        world = transform.Find("Current World").GetComponent<TextMeshPro>();
        time = transform.Find("Time").GetComponent<TextMeshPro>();
    }

    private IEnumerator SecondTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SubtractTime();
        }
    }

    private IEnumerator FourthOfSecondTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(.25f);
            ChangeCurrentSprite();
        }
    }

    private void SubtractTime()
    {
        timeLeft -= 1;
    }

    private void ChangeCurrentSprite()
    {
        if (currSprite < sprites.Length - 1) { currSprite += 1; }
        else { currSprite = 0; }
    }


    private void PositionUI()
    {
        dimensions = Camera.main.pixelRect.size / 2f;
        offset = new Vector3(dimensions.x / 8f, -dimensions.y / 10f, 0f);

        float xPos = Camera.main.pixelRect.size.x / 4f;
        score.rectTransform.localPosition += offset;
        coinSprite.rectTransform.localPosition += new Vector3(xPos - 32f, -32f) + offset;
        coins.rectTransform.localPosition += new Vector3(xPos, 0f) + offset;
        world.rectTransform.localPosition += new Vector3(xPos * 2 - coins.rectTransform.rect.width, 0f, 0f) + offset;
        time.rectTransform.localPosition += new Vector3(xPos * 3 - world.rectTransform.rect.width, 0f, 0f) + offset;
    }

    private void RescaleUI()
    {
        float fontSize = 10f;
    }

    private void UpdateOverlay()
    {
        UpdateScores();
        UpdateCoinSprite();
        UpdateCoins();
        UpdateTime();
    }

    // Updates the score in the overlay based on the scoreCount
    private void UpdateScores()
    {
        score.text = "MARIO\n" + scoreCount.ToString("000000");
    }

    // Updates the coin sprite in the overlay
    private void UpdateCoinSprite()
    {
        coinSprite.sprite = sprites[currSprite];
    }

    // Updates coin number in overlay using coinCount
    private void UpdateCoins()
    {
        if (coinCount < 100) { coins.text = "\u002a" + coinCount.ToString("00"); }
        else { /* ONE UP */ }
    }

    // Updates the time left in the overlay using timeLeft through a timer
    private void UpdateTime()
    {
        if (timeLeft >= 0) { time.text = "Time\n" + timeLeft.ToString("000"); }
        else { /* GAME OVER */ }
    }

    // Public function to add to score
    public static void AddToScores(int scoreToAdd)
    {
        scoreCount += scoreToAdd;
    }

    // Public function to add to coins
    public static void AddToCoins(int coinsToAdd)
    {
        coinCount += coinsToAdd;
    }
}
