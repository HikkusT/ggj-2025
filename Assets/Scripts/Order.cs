using System;

public class Order
{
    private DateTime _endTime;
    public Recipe Recipe;

    public Order(Recipe recipe, DateTime endTime)
    {
        Recipe = recipe;
        _endTime = endTime;
    }
}
