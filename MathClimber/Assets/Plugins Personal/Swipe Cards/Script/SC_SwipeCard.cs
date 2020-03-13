using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_SwipeCard : MonoBehaviour
{
    public SpriteRenderer background;
    public Color backgroundColor;
    public SpriteRenderer swipeCardImage;
    public Transform associatedIndicatorCircle;

    [HideInInspector] public int index;
}
