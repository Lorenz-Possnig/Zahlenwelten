using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NumberSupplier
{
    public abstract bool hasNext();
    public abstract int getNext();
}

public class FixedNumberSupplier : NumberSupplier
{
    private int[] numbers;
    private int idx = 0;
    private int length;

    public FixedNumberSupplier(int[] numbers)
    {
        this.numbers = numbers;
        length = numbers.Length;
    }

    public override int getNext()
    {
        
        var res = numbers[idx];
        idx++;
        return res;
    }

    public override bool hasNext() => idx < length;
}

public class RandomNumberSupplier : NumberSupplier
{
    private const int amountOfTeensToSupply = 2;
    private int suppliedInts = 0;
    private int[] hundreds = new[] { 100, 200, 300, 400, 500, 600, 700, 800, 900 };
    private int digitsAmount;
    public int DigitsAmount {
        get => digitsAmount;
        set => digitsAmount = value;
    }
    public RandomNumberSupplier()
    {

    }

    public override int getNext() {
        return getNext(DigitsAmount);
        // if (this.DigitsAmount == 2 && suppliedInts < amountOfTeensToSupply)
        // {
        //     suppliedInts++;
        //     return Random.Range(11, 20);
        // }
        // if (this.DigitsAmount == 3 && suppliedInts < amountOfTeensToSupply)
        // {
        //     suppliedInts++;
        //     int tween = Random.Range(11, 20);
        //     int hundred = hundreds[Random.Range(0, 9)];
        //     return tween + hundred;
        // }
        // return NumberGenerator.GetRandom(this.DigitsAmount);
    }

    private int getNext(int level)
    {
        switch (level)
        {
            case 1:
                return Random.Range(1, 10);
            case 2:
                if (suppliedInts < amountOfTeensToSupply)
                {
                    suppliedInts++;
                    return Random.Range(10, 20);
                }
                return Random.Range(10, 100);
            case 3:
                return NumberGenerator.GetRandom(this.DigitsAmount);
            case 4:
                return int.Parse($"{NumberGenerator.GetDigit()}0{NumberGenerator.GetDigit()}");
            case 5:
                return int.Parse($"{NumberGenerator.GetDigit()}{NumberGenerator.GetDigit()}0");
            default:
                return Random.Range(100, 1000);
        }
    }

    public void Reset()
    {
        this.suppliedInts = 0;
    }

    public override bool hasNext() => true;
}
