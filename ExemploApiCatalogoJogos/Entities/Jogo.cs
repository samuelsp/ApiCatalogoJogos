using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExemploApiCatalogoJogos.Entities
{
    public class Jogo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Produtora { get; set; }
        public double Preco { get; set; }
    }
}
