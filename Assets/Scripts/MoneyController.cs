public class MoneyController
{
    private int Amount;

    public MoneyController(int startAmount = 0)
    {
        Amount = startAmount;
    }

    public int GetAmount()
    {
        return Amount;
    }

    public bool AddAmount(int amount)
    {
        if (Amount + amount >= 0)
        {
            Amount += amount;
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return Amount.ToString() + '$';
    }

    public static explicit operator int(MoneyController money)
    {
        return money.Amount;
    }

    public static implicit operator MoneyController(int money)
    {
        return new MoneyController(money);
    }
}