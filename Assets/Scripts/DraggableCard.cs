using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public float xDifferenceThreshold = 250f;
    public float xPositionRange = 350f;
    public float yPositionRange = 150f;
    public float zRotationRange = 20f;

    private RectTransform rectTransform;
    private bool isBeingDragged = false;
    private bool isDraggable = true;
    private float xOrigin = 0;

    public CardData cardData;
    public string cardKey;
    //public List<EffectData> leftEffects;
    //public List<EffectData> rightEffects;

    private Vector2 originalPosition;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void Initialize(CardData cardData, string cardKey)
    {
        this.cardData = cardData;
        this.cardKey = cardKey;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        isBeingDragged = true;
        xOrigin = Input.mousePosition.x;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        if (rectTransform.anchoredPosition.x > xDifferenceThreshold || rectTransform.anchoredPosition.x < -xDifferenceThreshold)
        {
            StartCoroutine(AcceptCard());
            return;
        }

        isBeingDragged = false;
        xOrigin = 0;

        StartCoroutine(ResetPosition());
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!isBeingDragged)
            return;

        float difference = Mathf.Clamp(Input.mousePosition.x - xOrigin, -xPositionRange, xPositionRange);
        float lerpValue = difference / xPositionRange;

        float newXPosition = Mathf.Lerp(0, xPositionRange, Mathf.Abs(lerpValue)) * Mathf.Sign(lerpValue);
        float newYPosition = Mathf.Lerp(0, -yPositionRange, Mathf.Abs(lerpValue));
        float newZRotation = Mathf.Lerp(0, zRotationRange, Mathf.Abs(lerpValue)) * -Mathf.Sign(lerpValue);

        rectTransform.anchoredPosition = originalPosition + new Vector2(newXPosition, newYPosition);
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, newZRotation);
    }

    private IEnumerator ResetPosition()
    {
        isDraggable = false;
        isBeingDragged = false;
        float currentLerp = rectTransform.anchoredPosition.x / xPositionRange;
        float xPositionIncrement;
        float yPositionIncrement = yPositionRange / 30f;
        float zRotationIncrement;
        if (currentLerp <= 0)
        {
            xPositionIncrement = xPositionRange / 30f;
            zRotationIncrement = -zRotationRange / 30f;
        }
        else
        {
            xPositionIncrement = -xPositionRange / 30f;
            zRotationIncrement = zRotationRange / 30f;
        }

        while (rectTransform.anchoredPosition.x != 0)
        {
            Vector2 currentPosition = rectTransform.anchoredPosition;
            Vector2 newPosition;
            if (currentLerp <= 0)
                newPosition = new Vector2(Mathf.Min(currentPosition.x + xPositionIncrement, 0f), Mathf.Min(currentPosition.y + yPositionIncrement, 0f));
            else
                newPosition = new Vector2(Mathf.Max(currentPosition.x + xPositionIncrement, 0f), Mathf.Min(currentPosition.y + yPositionIncrement, 0f));

            float currentRotation = rectTransform.transform.eulerAngles.z;
            float newRotation;
            if (currentLerp <= 0)
                newRotation = Math.Max(currentRotation + zRotationIncrement, 0f);
            else
                newRotation = Math.Min(currentRotation + zRotationIncrement, 360f);

            rectTransform.anchoredPosition = newPosition;
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, newRotation);
            yield return new WaitForSeconds(1f / 120f);
        }
        isDraggable = true;
    }

    private IEnumerator AcceptCard()
    {
        isDraggable = false;
        isBeingDragged = false;
        float currentLerp = rectTransform.anchoredPosition.x / xPositionRange * 4;
        float highXPositionRange = xPositionRange * 4;
        float highYPositionRange = yPositionRange * 4;
        float highZRotationRange = zRotationRange * 4;

        float xPositionIncrement;
        float yPositionIncrement = -highYPositionRange / 60f;
        float zRotationIncrement;
        if (currentLerp >= 0)
        {
            xPositionIncrement = highXPositionRange / 60f;
            zRotationIncrement = -highZRotationRange / 60f;
        }
        else
        {
            xPositionIncrement = -highXPositionRange / 60f;
            zRotationIncrement = highZRotationRange / 60f;
        }

        while (rectTransform.anchoredPosition.x != highXPositionRange && rectTransform.anchoredPosition.x != -highXPositionRange)
        {
            Vector2 currentPosition = rectTransform.anchoredPosition;
            Vector2 newPosition;
            if (currentLerp >= 0)
                newPosition = new Vector2(Mathf.Min(currentPosition.x + xPositionIncrement, highXPositionRange), Mathf.Max(currentPosition.y + yPositionIncrement, -highYPositionRange));
            else
                newPosition = new Vector2(Mathf.Max(currentPosition.x + xPositionIncrement, -highXPositionRange), Mathf.Max(currentPosition.y + yPositionIncrement, -highYPositionRange));

            float currentRotation = rectTransform.transform.eulerAngles.z;
            float newRotation;
            if (currentLerp >= 0)
                newRotation = Math.Max(currentRotation + zRotationIncrement, -highZRotationRange);
            else
                newRotation = Math.Min(currentRotation + zRotationIncrement, highZRotationRange);

            rectTransform.anchoredPosition = newPosition;
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, newRotation);
            yield return new WaitForSeconds(1f / 120f);
        }
        if (currentLerp >= 0)
        {
            GameHandler.instance.AdvanceTurn(cardData.right);
            GameHandler.instance.ChooseCard(cardKey, cardData, true);
        }
        else
        {
            GameHandler.instance.AdvanceTurn(cardData.left);
            GameHandler.instance.ChooseCard(cardKey, cardData, false);
        }
        isDraggable = true;
        Destroy(this.gameObject, 0.1f);
    }
}
