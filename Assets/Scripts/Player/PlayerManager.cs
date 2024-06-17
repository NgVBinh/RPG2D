using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{
    public static PlayerManager instance;

    public Player player;

    public int currency;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance );
        }
        else
        {
            instance = this;
        }
    }
    
    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            Debug.Log("Not enough money");
            return false;
        }

        currency-=_price;
        return true;
    }

    public void LoadData(GameData gameData)
    {
        this.currency = gameData.currency;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.currency = this.currency;
    }
}
