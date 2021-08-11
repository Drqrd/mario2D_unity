using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWarp : MonoBehaviour
{
    private const float dist = 1.25f;

    private string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H" };
    private string[] directions = { "Up", "Down", "Left", "Right" };

    public Sprite[] sprites;

    [Header("Letter")]
    public string letter;

    [Header("Direction")]
    public string inDirection;
    public string outDirection;

    public bool update;

    private void OnValidate()
    {
        if (update)
        {
            update = false;
            UpdateLetter();
            UpdateDirection("In");
            UpdateDirection("Out");
        }
    }

    private void UpdateLetter()
    {
        switch (letter)
        {
            case "A":
                transform.Find("Warp In").GetComponent<SpriteRenderer>().sprite = sprites[0];
                transform.Find("Warp Out").GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;

            case "B":
                transform.Find("Warp In").GetComponent<SpriteRenderer>().sprite = sprites[1];
                transform.Find("Warp Out").GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;

            case "C":
                transform.Find("Warp In").GetComponent<SpriteRenderer>().sprite = sprites[2];
                transform.Find("Warp Out").GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;

            case "D":
                transform.Find("Warp In").GetComponent<SpriteRenderer>().sprite = sprites[3];
                transform.Find("Warp Out").GetComponent<SpriteRenderer>().sprite = sprites[3];
                break;

            case "E":
                transform.Find("Warp In").GetComponent<SpriteRenderer>().sprite = sprites[4];
                transform.Find("Warp Out").GetComponent<SpriteRenderer>().sprite = sprites[4];
                break;

            case "F":
                transform.Find("Warp In").GetComponent<SpriteRenderer>().sprite = sprites[5];
                transform.Find("Warp Out").GetComponent<SpriteRenderer>().sprite = sprites[5];
                break;

            case "G":
                transform.Find("Warp In").GetComponent<SpriteRenderer>().sprite = sprites[6];
                transform.Find("Warp Out").GetComponent<SpriteRenderer>().sprite = sprites[6];
                break;

            case "H":
                transform.Find("Warp In").GetComponent<SpriteRenderer>().sprite = sprites[7];
                transform.Find("Warp Out").GetComponent<SpriteRenderer>().sprite = sprites[7];
                break;

            default:
                ThrowLettersError();
                break;
        }
    }

    private void UpdateDirection(string i_o)
    {
        string dir = i_o == "In" ? inDirection : outDirection;
        GameObject obj = i_o == "In" ? transform.Find("Warp In").GetChild(0).gameObject : transform.Find("Warp Out").GetChild(0).gameObject;

        switch (dir)
        {
            case "Up":
                obj.transform.localPosition = Vector3.up * dist;
                obj.transform.rotation =  Quaternion.Euler(0f, 0f, 180f);
                break;
            case "Down":
                obj.transform.localPosition = Vector3.down * dist;
                obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case "Left":
                obj.transform.localPosition = Vector3.left * dist;
                obj.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                break;
            case "Right":
                obj.transform.localPosition = Vector3.right * dist;
                obj.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            default:
                ThrowDirectionError(i_o);
                break;
        }
    }

    public string GetOutDirection()
    {
        return outDirection;
    }


    private void ThrowLettersError()
    {
        Debug.Log("ERR: Letter has to be a letter A-H");
    }

    private void ThrowDirectionError(string a)
    {
        Debug.Log("ERR: Direction needs to be up, down, left or right: " + a);
    }
}
