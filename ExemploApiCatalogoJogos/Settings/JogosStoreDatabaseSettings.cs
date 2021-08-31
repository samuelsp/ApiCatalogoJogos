using ExemploApiCatalogoJogos.Settings.Interfaces;

namespace ExemploApiCatalogoJogos.Settings
{
    public class JogosStoreDatabaseSettings : IJogosStoreDatabaseSettings
    {
        public string JogosCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
