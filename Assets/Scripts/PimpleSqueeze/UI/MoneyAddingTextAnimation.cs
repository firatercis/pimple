using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Dreamteck.Splines.IO;
using Obi;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MoneyAddingTextAnimation : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation textAnim;
    [SerializeField] private TextMeshProUGUI text;

    private bool isOnPlay = false;
    
    public void EnableAnimation(int moneyValue)
    {
        if (!isOnPlay)
        {
            isOnPlay = true;
            gameObject.SetActive(true);
            text.text = "+" + moneyValue + "$";
            gameObject.SetActive(true);
            textAnim.DORestart();
            DOVirtual.DelayedCall(2f, () =>
            {
                isOnPlay = false;
                gameObject.SetActive(false);
            });
        }
    }
}
