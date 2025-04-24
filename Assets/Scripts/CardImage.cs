using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardImage : MonoBehaviour
{
    public static CardImage instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public Sprite _cafe;
    public static Sprite Cafe
    {
        get { return instance._cafe; }
        private set { }
    }
}
