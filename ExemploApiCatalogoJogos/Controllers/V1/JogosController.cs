using ExemploApiCatalogoJogos.Exceptions;
using ExemploApiCatalogoJogos.InputModel;
using ExemploApiCatalogoJogos.Services;
using ExemploApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExemploApiCatalogoJogos.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Buscar todos os jogos com paginação
        /// </summary>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="404">Caso não haja jogos</response>   
        [HttpGet("paginacao")]
        public async Task<ActionResult<List<JogoViewModel>>> GetAllWithPage([FromQuery] int pagina, [FromQuery] int quantidade)
        {
            var jogos = await _jogoService.GetAllWithPage(pagina, quantidade);

            if (jogos == null)
            {
                return NotFound();
            }

            return Ok(jogos);
        }


        /// <summary>
        /// Buscar todos os jogos
        /// </summary>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="404">Caso não haja jogos</response>   
        [HttpGet]
        public async Task<ActionResult<List<JogoViewModel>>> GetAll()
        {
            var jogos = await _jogoService.GetAll();

            if(jogos == null)
            {
                return NotFound();
            }

            return Ok(jogos);
        }

        /// <summary>
        /// Buscar um jogo pelo seu Id
        /// </summary>
        /// <param name="id">Id do jogo buscado</param>
        /// <response code="200">Retorna o jogo filtrado</response>
        /// <response code="404">Caso não haja jogo com este id</response>   
        [HttpGet("id")]
        public async Task<ActionResult<JogoViewModel>> GetById([FromQuery] string id)
        {
            var jogo = await _jogoService.GetById(id);

            if (jogo == null)
            {
                return NotFound();
            }

            return Ok(jogo);
        }

        /// <summary>
        /// Inserir um jogo no catálogo
        /// </summary>
        /// <param name="jogoInputModel">Dados do jogo a ser inserido</param>
        /// <response code="201">Caso o jogo seja inserido com sucesso</response>
        /// <response code="422">Caso já exista um jogo com mesmo nome para a mesma produtora</response>   
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> Create([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Create(jogoInputModel);
                return CreatedAtAction("GetById", new { id = jogo.Id }, jogo);
            }

            catch (JogoJaCadastradoException e)
            {
                return UnprocessableEntity(e.Message);
            }
        }

        /// <summary>
        /// Atualizar um jogo no catálogo
        /// </summary>
        /// <param name="idJogo">Id do jogo a ser atualizado</param>
        /// <param name="jogoInputModel">Novos dados para atualizar o jogo indicado</param>
        /// <response code="200">Caso o jogo seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>   
        [HttpPut]
        public async Task<ActionResult> Update([FromQuery] string idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Update(idJogo, jogoInputModel);
                return Ok();
            }

            catch (JogoNaoCadastradoException e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Atualizar o preço de um jogo
        /// </summary>
        /// <param name="idJogo">Id do jogo a ser atualizado</param>
        /// <param name="preco">Novo preço do jogo</param>
        /// <response code="200">Caso o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>   
        [HttpPatch("preco")]
        public async Task<ActionResult> UpdatePriceGame([FromQuery] string idJogo, [FromQuery] double preco)
        {
            try
            {
                await _jogoService.UpdatePrice(idJogo, preco);
                return Ok();
            }

            catch (JogoNaoCadastradoException e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Excluir um jogo pelo seu Id
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser excluído</param>
        /// <response code="200">Cao o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>   
        [HttpDelete("id")]
        public async Task<ActionResult> Delete([FromQuery] string idJogo)
        {
            try
            {
                await _jogoService.Delete(idJogo);
                return Ok();
            }
            catch (JogoNaoCadastradoException e)
            {
                return NotFound(e.Message);
            }
        }

    }
}
