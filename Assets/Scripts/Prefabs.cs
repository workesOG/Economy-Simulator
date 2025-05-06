using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Prefabs : MonoBehaviour
{
    public static Prefabs instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public GameObject _arrowholder;
    public GameObject _effectLeft;
    public GameObject _effectRight;
    public GameObject _card;

    public static class Arrow
    {
        public static GameObject Right
        {
            get
            {
                GameObject go = instance._arrowholder;
                go.transform.Find("Arrow").GetComponent<TMP_Text>().text = "→";
                return go;
            }
            private set { }
        }
        public static GameObject Left
        {
            get
            {
                GameObject go = instance._arrowholder;
                go.transform.Find("Arrow").GetComponent<TMP_Text>().text = "←";
                return go;
            }
            private set { }
        }
    }

    public static class Effect
    {
        public static GameObject Positive(bool isRight, string text)
        {
            GameObject go = isRight ? instance._effectRight : instance._effectLeft;
            go.transform.Find("Indicator").GetComponent<TMP_Text>().text = "<color=#139F27ff>▲</color>";
            go.transform.Find("Text").GetComponent<TMP_Text>().text = $"<color=#139F27ff>{text}</color>";
            return go;
        }

        public static GameObject Negative(bool isRight, string text)
        {
            GameObject go = isRight ? instance._effectRight : instance._effectLeft;
            go.transform.Find("Indicator").GetComponent<TMP_Text>().text = "<color=#f6354fff>▼</color>";
            go.transform.Find("Text").GetComponent<TMP_Text>().text = $"<color=#f6354fff>{text}</color>";
            return go;
        }
    }

    public static GameObject Card(Transform window, string title, string description, CardData cardData, string cardKey)
    {
        GameObject go = Instantiate(instance._card, window, false);
        go.transform.Find("Title").GetComponent<TMP_Text>().text = title;
        go.transform.Find("Description").GetComponent<TMP_Text>().text = description;
        go.transform.Find("Image Mask/Image").GetComponent<Image>().sprite = CardImage.instance.FindSprite(cardData.imageID);
        go.GetComponent<DraggableCard>().Initialize(cardData, cardKey);
        Transform contentWindowLeft = go.transform.Find("Scroll View/Viewport/Content/Left").transform;
        Transform contentWindowRight = go.transform.Find("Scroll View/Viewport/Content/Right").transform;
        // Right effects
        //Instantiate(Arrow.Right, contentWindowRight);
        foreach (EffectData effect in cardData.right.Where(e => e.positive))
        {
            Instantiate(Effect.Positive(true, effect.text), contentWindowRight);
        }
        foreach (EffectData effect in cardData.right.Where(e => !e.positive))
        {
            Instantiate(Effect.Negative(true, effect.text), contentWindowRight);
        }

        // Left effects
        //Instantiate(Arrow.Left, contentWindowLeft);
        foreach (EffectData effect in cardData.left.Where(e => e.positive))
        {
            Instantiate(Effect.Positive(false, effect.text), contentWindowLeft);
        }
        foreach (EffectData effect in cardData.left.Where(e => !e.positive))
        {
            Instantiate(Effect.Negative(false, effect.text), contentWindowLeft);
        }

        return go;
    }
}

[Serializable]
public class EffectData
{
    public bool positive;
    public string text;

    public Action action;
}
