using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class IconsRushingOverIconAnimation : MonoBehaviour
{
    // Settings
    public int valuePerCount;
    public string valueSaveKey;
    public float sourcePosDeviation = 10;
    public int numberOfRushingIcons = 20;
    // Connections
    public TextMeshProUGUI valueText = default;
    public RectTransform sourcePosition = default;
    public RectTransform destinationPosition = default;
    public GameObject iconPrefab = default;

    // State variables
    private int currentValue = 0;



    private void Start()
    {
        InitState();
    }

    private void InitState()
    {
        LoadValue();
    }

    void LoadValue()
    {
        if (valueSaveKey != null)
        {
            if (valueSaveKey != "")
            {
                currentValue = PlayerPrefs.GetInt(valueSaveKey,0);
                UpdateText(currentValue);
            }
        }
    }

    private void PlayIconRushAnimation(Vector3 screenPos)
    {
        screenPos += (Vector3)Random.insideUnitCircle * sourcePosDeviation;
       
        GameObject createdGO = Instantiate(iconPrefab, screenPos, Quaternion.identity, transform);
        RectTransform createdIconRect = createdGO.GetComponent<RectTransform>();
        createdIconRect.DOPunchScale(Vector3.one * 0.5f, 0.2f, 5);
        createdIconRect.DOMove(destinationPosition.position, 0.9f).OnComplete(() =>
        {
            Destroy(createdGO);
            currentValue += valuePerCount;
            //UpdateText(currentValue);
            //UpdateText(heartText, currentHeartCount.ToString());
        });
    }

    public void SpawnIcons(Vector3 worldPos, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            PlayIconRushAnimation(worldPos);
        }
    }

    public void UpdateText(int value)
    {
        valueText.text = "" + value + "k";
    }

    IEnumerator SomeFunctionWithMagicNumbersToTalkWithKemal(int number)
    {
        for (int i = 0; i < number; i++)
        {
            yield return new WaitForSeconds(0.06f);
            PlayIconRushAnimation(new Vector3(-400, -20, 400));
        }
    }

    void Update()
    {
        // For Test
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 screenPos = sourcePosition.position;
            
        //    SpawnIcons(screenPos, 20);
        //    currentValue = PlayerPrefs.GetInt(valueSaveKey, 0);

        //}
    }

    
}

