using ExemploApiCatalogoJogos.Entities;
using ExemploApiCatalogoJogos.InputModel;
using ExemploApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExemploApiCatalogoJogos.Services
{
    public interface IJogoService 
    {
        Task<List<JogoViewModel>> GetAllWithPage(int pagina, int quantidade);
        Task<List<JogoViewModel>> GetAll();
        Task<JogoViewModel> GetById(string id);
        Task<JogoViewModel> Create(JogoInputModel jogoInputModel);
        Task Update(string id, JogoInputModel jogoInputModel);
        Task UpdatePrice(string id, double preco);
        Task Delete(string id);
    }
}
