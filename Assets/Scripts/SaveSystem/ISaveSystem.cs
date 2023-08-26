namespace SaveSystem
{
    public interface ISaveSystem
    {
        void Save(ISaveble data);

        ISaveble Load();
    }
}