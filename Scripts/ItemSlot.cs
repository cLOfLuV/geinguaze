using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ItemSlot : MonoBehaviour
{
    public SlotData SlotData;

    public event Action OnBuyItem;

    public string Rcode;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Price;
    public bool IsSold = false;

    private void Start()
    {
        InitializeSlot();
    }

    private void InitializeSlot()
    {
        Name.text = SlotData.Name;
        Description.text = SlotData.Description;
        Price.text = SlotData.Price.ToString();
        IsSold = SlotData.IsSold;
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        if (IsSold == true)
        {
            SlotData.IsSold = true;
            if (Rcode == "ITE00001") Price.text = "구매 완료";
            else Price.text = "Max 레벨";
        }
    }

    public void BuyItem()
    {
        if (Mining.instance.coin >= SlotData.Price && IsSold == false)
        {
            Mining.instance.coin -= SlotData.Price;
            Mining.instance.UpdateCoinText();
            switch (Rcode)
            {
                case "ITE00001":
                    Mining.instance.IsAuto = true;
                    IsSold = true;
                    break;
                case "ITE00002":
                    if (Mining.instance.speed >= 0.9f)
                    {
                        IsSold = true;
                    }
                    else Mining.instance.IncreaseSpeed();
                    break;
                case "ITE00003":
                    if (Mining.instance.amount >= 100)
                    {
                        IsSold = true;     
                    }
                    else Mining.instance.IncreaseAmount();
                    break;
            }
            UpdateSlotUI();
            OnBuyItem?.Invoke();
        }
    }
}