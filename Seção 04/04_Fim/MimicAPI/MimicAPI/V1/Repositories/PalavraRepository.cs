using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.V1.Models;
using MimicAPI.V1.Repositories.Contracts;
using MimicAPI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Repositories
{
    public class PalavraRepository : IPalavraRepository
    {
        private readonly MimicContext _banco;

        public PalavraRepository(MimicContext banco)
        {
            _banco = banco;
        }
        public ListaPaginacao<Palavra> ObterPalavras(PalavraUrlQuery query)
        {
            var lista = new ListaPaginacao<Palavra>();
            var item = _banco.Palavras.AsNoTracking().AsQueryable();

            //Busca as palavras com data de criação ou atualização maiores que a data informada
            if (query.Data.HasValue)
            {
                item = item.Where(a => a.Criado > query.Data.Value || a.Atualizado > query.Data.Value);
            }

            //Paginação
            if (query.NumPagina.HasValue)
            {
                var totalRegistros = item.Count();
                item = item.Skip((query.NumPagina.Value - 1) * query.QtdRegistrosPorPagina.Value).Take(query.QtdRegistrosPorPagina.Value);

                var paginacao = new Paginacao();
                paginacao.NumeroPagina = query.NumPagina.Value;
                paginacao.RegistrosPorPagina = query.QtdRegistrosPorPagina.Value;
                paginacao.TotalRegistros = totalRegistros;
                paginacao.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / query.QtdRegistrosPorPagina.Value);

                lista.Paginacao = paginacao;
            }
            lista.Resultados.AddRange(item.ToList());

            return lista;
        }

        public Palavra Obter(int id)
        {
            return _banco.Palavras.AsNoTracking().FirstOrDefault(a => a.Id == id);
        }

        public void Cadastrar(Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
        }

        public void Atualizar(Palavra palavra)
        {
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
        }
        
        public void Deletar(int id)
        {
            var palavra = Obter(id);
            palavra.Ativo = false;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
        }

        
    }
}
