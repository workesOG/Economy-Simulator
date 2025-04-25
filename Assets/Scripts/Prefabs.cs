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
    public GameObject _effect;
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
        public static GameObject Positive(string text)
        {
            GameObject go = instance._effect;
            go.transform.Find("Indicator").GetComponent<TMP_Text>().text = "<color=#139F27ff>▲</color>";
            go.transform.Find("Text").GetComponent<TMP_Text>().text = $"<color=#139F27ff>{text}</color>";
            return go;
        }

        public static GameObject Negative(string text)
        {
            GameObject go = instance._effect;
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
        Transform contentWindow = go.transform.Find("Scroll View/Viewport/Content").transform;

        // Right effects
        Instantiate(Arrow.Right, contentWindow);
        foreach (EffectData effect in cardData.right.Where(e => e.positive))
        {
            Instantiate(Effect.Positive(effect.text), contentWindow);
        }
        foreach (EffectData effect in cardData.right.Where(e => !e.positive))
        {
            Instantiate(Effect.Negative(effect.text), contentWindow);
        }

        // Left effects
        Instantiate(Arrow.Left, contentWindow);
        foreach (EffectData effect in cardData.left.Where(e => e.positive))
        {
            Instantiate(Effect.Positive(effect.text), contentWindow);
        }
        foreach (EffectData effect in cardData.left.Where(e => !e.positive))
        {
            Instantiate(Effect.Negative(effect.text), contentWindow);
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
