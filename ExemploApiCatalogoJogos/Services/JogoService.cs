using AutoMapper;
using ExemploApiCatalogoJogos.Entities;
using ExemploApiCatalogoJogos.Exceptions;
using ExemploApiCatalogoJogos.InputModel;
using ExemploApiCatalogoJogos.Settings.Interfaces;
//using ExemploApiCatalogoJogos.Repositories;
using ExemploApiCatalogoJogos.ViewModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExemploApiCatalogoJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IMongoCollection<Jogo> _collection;
        private readonly IMapper _mapper;        

        public JogoService(IJogosStoreDatabaseSettings settings,
                           IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<Jogo>(settings.JogosCollectionName);
            _mapper = mapper;
        }

        public async Task<List<JogoViewModel>> GetAllWithPage(int pagina, int quantidade)
        {
            return _mapper.Map<List<JogoViewModel>>(await _collection.Find(jogo => true)
                                    .Skip((pagina - 1) * quantidade)
                                    .Limit(quantidade)
                                    .ToListAsync());            
        }

        public async Task<List<JogoViewModel>> GetAll()
        {
            var jogos = await _collection.Find(jogo => true).ToListAsync();
            return _mapper.Map<List<JogoViewModel>>(jogos);            
        }
        
        public async Task<JogoViewModel> GetById(string id)
        {
            var jogo = await _collection.Find(jogo => jogo.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<JogoViewModel>(jogo);
        }

        public async Task<JogoViewModel> Create(JogoInputModel jogoInputModel)
        {
            var jogoEntitie = _mapper.Map<Jogo>(jogoInputModel);

            var resultado = await _collection.Find(jogo => jogo.Nome == jogoEntitie.Nome &&
                                                jogo.Produtora == jogoEntitie.Produtora)
                                                .FirstOrDefaultAsync();
            
            if(resultado == null)
            {
                await _collection.InsertOneAsync(jogoEntitie);
                return _mapper.Map<JogoViewModel>(jogoEntitie);
            }

            throw new JogoJaCadastradoException();
        }

        public async Task Update(string id, JogoInputModel jogoInputModel)
        {
            var resultado = await GetById(id);

            if (resultado == null)
                throw new JogoNaoCadastradoException();

            var jogoEntitie = _mapper.Map<Jogo>(jogoInputModel);
            jogoEntitie.Id = id;
            
            await _collection.ReplaceOneAsync(jogo => jogo.Id == id, jogoEntitie);            
        }

        public async Task UpdatePrice(string id, double novoPreco)
        {
            var resultado = await GetById(id);

            if (resultado == null)
                throw new JogoNaoCadastradoException();

            var filter = Builders<Jogo>.Filter.Eq("Nome", resultado.Nome);
            var price = Builders<Jogo>.Update.Set("Preco", novoPreco);
            await _collection.UpdateOneAsync(filter, price);
        }

        public async Task Delete(string id)
        {
            var resultado = await GetById(id);

            if (resultado == null)
                throw new JogoNaoCadastradoException();

            await _collection.DeleteOneAsync(jogo => jogo.Id == id);
        }
      
    }
}
