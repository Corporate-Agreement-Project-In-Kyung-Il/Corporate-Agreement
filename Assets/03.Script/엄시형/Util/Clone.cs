namespace _03.Script.엄시형.Util
{
    public interface ICloneable<T> where T : class
    {
        T Clone();
    }
}