using Cards.CustomType;

namespace Network.Interfaces
{
    public interface ICardEventNetworkService
    {
        public void RaiseEventCardInvoke(CardInfo card);
    }
}