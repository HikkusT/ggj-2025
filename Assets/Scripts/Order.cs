using System;

public class Order
{
    private DateTime _endTime;
    private Recipe _recipe;

    public Order(Recipe recipe, DateTime endTime)
    {
        _recipe = recipe;
        _endTime = endTime;
    }
}
