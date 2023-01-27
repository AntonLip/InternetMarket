namespace InternetMarket.Interfaces
{
    public interface IEntity<TId> 
    {
        public TId Id{ get; set; }
    }
}
