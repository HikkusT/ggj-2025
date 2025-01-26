using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private Pulsable _orderSignUI;
    [SerializeField] private Text _orderText;
    [SerializeField] private float _orderDelay;
    public Order CurrentOrder = null;
    private int _orderNumber = 0;
    private int _score = 0;
    [SerializeField] private List<Recipe> _recipes;

    bool _readyForNewOrder = true;
    private void Update()
    {
        if (CurrentOrder == null && _readyForNewOrder)
        {
            var recipe = _recipes[_orderNumber % _recipes.Count];
            CurrentOrder = new Order(recipe, DateTime.Now + new TimeSpan(0, 2, 0));
            _orderSignUI.gameObject.SetActive(true);
            _orderText.text = recipe.RecipeName;
        }
    }

    public async UniTaskVoid CompleteOrder()
    {
        CurrentOrder = null;
        _score++;
        _orderNumber++;
        _orderSignUI.gameObject.SetActive(false);
        _readyForNewOrder = false;
        await UniTask.Delay(TimeSpan.FromSeconds(_orderDelay));
        _readyForNewOrder = true;
    }

    public void FailOrder()
    {
        _orderSignUI.FlashUI();
    }
}