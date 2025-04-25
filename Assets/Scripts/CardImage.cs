using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardImage : MonoBehaviour
{
    public static CardImage instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public Sprite FindSprite(string imageID)
    {
        return sprites.Where(x => x.identifier == imageID).FirstOrDefault().sprite;
    }

    public List<SpriteEntry> sprites = new List<SpriteEntry>();
}

[Serializable]
public class SpriteEntry
{
    public string identifier;
    public Sprite sprite;
}