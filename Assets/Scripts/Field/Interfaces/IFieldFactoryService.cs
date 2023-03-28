namespace Field.Interfaces
{
    public interface IFieldFactoryService
    {
        public Cell InstantiateCell();

        public Line InstantiateLine();
    }
}