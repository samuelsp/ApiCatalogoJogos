namespace ExemploApiCatalogoJogos.Settings.Interfaces
{
    public interface IJogosStoreDatabaseSettings
    {
        string JogosCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
