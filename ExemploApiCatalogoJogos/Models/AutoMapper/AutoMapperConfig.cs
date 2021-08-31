using AutoMapper;
using ExemploApiCatalogoJogos.Entities;
using ExemploApiCatalogoJogos.InputModel;
using ExemploApiCatalogoJogos.ViewModel;

namespace ExemploApiCatalogoJogos.Models.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            #region Model to View
            CreateMap<Jogo, JogoViewModel>();
            CreateMap<Jogo, JogoInputModel>();
            #endregion

            #region View to Model
            CreateMap<JogoInputModel, Jogo>();
            CreateMap<JogoViewModel, Jogo>();
            #endregion


        }
    }
}
