using MMapper.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MMapper.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new MapperConfiguration();

            config.CreateMap<UsuarioDto, UsuarioModel>();
            config.CreateMap<ProdutoDto, ProdutoEntity>(cfg =>
            {
                cfg.ForMember("Titulo", "NomeProduto");
            });
            config.CreateMap<PedidoDto, PedidoEntity>();
            config.CreateMap<BlobSource, TextoDestino>();
            config.CreateMap<ClienteDto, ClienteModel>();
            config.CreateMap<EnderecoDto, EnderecoModel>();
            config.CreateMap<Evento, EventoDto>();

            var mapper = new MMapper(config);

            var usuarioDto = new UsuarioDto { Nome = "Carlos", Idade = 30 };
            var usuarioModel = mapper.Map<UsuarioDto, UsuarioModel>(usuarioDto);
            Console.WriteLine(usuarioModel.Nome + " " + usuarioModel.Idade);

            var produto = new ProdutoDto { Titulo = "Notebook Gamer" };
            var entidade = mapper.Map<ProdutoDto, ProdutoEntity>(produto);
            Console.WriteLine(entidade.NomeProduto);

            var pedido = new PedidoDto { NumeroPedido = "12345" };
            var pedidoEnt = mapper.Map<PedidoDto, PedidoEntity>(pedido);
            Console.WriteLine(pedidoEnt.NumeroPedido);

            var blob = new BlobSource { Dados = Encoding.UTF8.GetBytes("Documento secreto") };
            var texto = mapper.Map<BlobSource, TextoDestino>(blob);
            if (texto?.Dados != null)
                Console.WriteLine(texto.Dados);
            else
                Console.WriteLine("Dados está nulo.");

            var novoDto = mapper.Map<UsuarioModel, UsuarioDto>(usuarioModel);
            Console.WriteLine(novoDto.Nome);

            var evento = new Evento { DataEvento = DateTime.Now };
            var eventoDto = mapper.Map<Evento, EventoDto>(evento);
            Console.WriteLine(eventoDto.DataEvento);

            var lista = new List<Source>
            {
                new Source { Nome = "João" },
                new Source { Nome = "Maria" }
            };
            var listaDestino = lista.Select(src => mapper.Map<Source, Destination>(src)).ToList();
            foreach (var item in listaDestino)
            {
                Console.WriteLine(item.Nome);
            }
        }
    }
}
