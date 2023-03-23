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
    public int DigitsAmount { get; set; }
    public RandomNumberSupplier()
    {

    }

    public override int getNext() => NumberGenerator.GetRandom(this.DigitsAmount);

    public override bool hasNext() => true;
}
