using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWarp : MonoBehaviour
{
    // Distance of arrow from letter
    private const float dist = .75f;

    // Holds the sprites for the letters
    public Sprite[] sprites;

    // Changes letter based on editor input
    [Header("Letter")]
    public string letter;

    // Changes direction of arrows based on editor input
    [Header("Direction")]
    public string inDirection;
    public string outDirection;

    // Variables that change in camera when teleported
    [Header("Camera Parameters")]
    public Material cameraBackgroundColor;
    private Vector3 cameraPos;

    // For editing purposes
    public bool update;

    // Camera position that camera will move to in warp function
    public Vector3 CameraPos
    {
        get { return cameraPos; }
        set { cameraPos = value; }
    }



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

    private void Start()
    {
        // Disable sprite renderers so that they are essentially invisible warps
        Rec_DisableSpriteRenderer(transform);

        // Gets and adjusts desired camera position
        CameraPos = transform.GetChild(2).position;
        CameraPos = new Vector3(CameraPos.x, CameraPos.y, CameraPos.z - 10f);
    }

    // Changes the letters of the warp, only if A-H, else throw error
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

    // Changes the arrow position and rotation to reflect editor settings
    private void UpdateDirection(string i_o)
    {
        // So i can use function for in or out
        string dir = i_o == "In" ? inDirection : outDirection;
        GameObject obj = i_o == "In" ? transform.Find("Warp In").GetChild(0).gameObject : transform.Find("Warp Out").GetChild(0).gameObject;

        // Self explanatory part, if up, down, left or right change arrow pos and rotation, else throw error in editor
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

    // Recursive function for disabling sprite renderers
    private void Rec_DisableSpriteRenderer(Transform t)
    {
        // if has sprite renderer component, disable
        if (t.TryGetComponent(out SpriteRenderer sprite)) { sprite.enabled = false; }
        // If there are children, try to disable sprite renderer in them
        if (t.childCount > 0) { foreach (Transform child in t) { Rec_DisableSpriteRenderer(child); } }
    }

    // Editor error message 
    private void ThrowLettersError()
    {
        Debug.Log("ERR: Letter has to be a letter A-H");
    }

    // Editor error message
    private void ThrowDirectionError(string a)
    {
        Debug.Log("ERR: Direction needs to be up, down, left or right: " + a);
    }
}
