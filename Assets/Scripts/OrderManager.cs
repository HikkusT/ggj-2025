using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private GameObject _orderSignUI;
    [SerializeField] private Text _orderText;
    private Order _currentOrder = null;
    private int _orderNumber = 0;
    [SerializeField] private List<Recipe> _recipes;

    private void Update()
    {
        if (_currentOrder == null)
        {
            var recipe = _recipes[_orderNumber];
            _currentOrder = new Order(recipe, DateTime.Now + new TimeSpan(0, 1, 0));
            _orderSignUI.SetActive(true);
            _orderText.text = recipe.RecipeName;
        }
    }
}