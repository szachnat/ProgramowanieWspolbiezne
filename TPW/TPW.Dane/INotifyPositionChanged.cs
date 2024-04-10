namespace TPW.Dane
{
    /// <summary>
    /// Interfejs do zmiany położenia kulki
    /// </summary>
    public interface INotifyPositionChanged
    {
        public event PositionChangeEventHandler OnPositionChange;
    }
}
