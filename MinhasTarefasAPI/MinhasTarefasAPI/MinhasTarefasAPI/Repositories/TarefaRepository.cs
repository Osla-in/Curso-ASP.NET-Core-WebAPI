using MinhasTarefasAPI.DataBase;
using MinhasTarefasAPI.Models;
using MinhasTarefasAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly MinhasTarefasContext _banco;

        public TarefaRepository(MinhasTarefasContext banco)
        {
            _banco = banco;
        }

        public List<Tarefa> Restauracao(ApplicationUser usuario, DateTime dataUltimaSincronizacao)
        {
            var query = _banco.Tarefas.Where(u => u.UsuarioId == usuario.Id).AsQueryable();

            if(dataUltimaSincronizacao == null)
            {
                query.Where(t => t.Criado >= dataUltimaSincronizacao || t.Atualizado >= dataUltimaSincronizacao);
            }

            return query.ToList<Tarefa>();
        }

        public List<Tarefa> Sincronizacao(List<Tarefa> tarefas)
        {
            var tarefasNovas = tarefas.Where(t => t.IdTarefaApi == 0);

            if(tarefasNovas.Count() > 0)
            {
                foreach (var tarefa in tarefasNovas)
                {
                    _banco.Tarefas.Add(tarefa);
                }
                _banco.SaveChanges();
            }

            var tarefasExcluidasAtualizadas = tarefas.Where(t => t.IdTarefaApi != 0);

            if (tarefasExcluidasAtualizadas.Count() > 0)
            {
                foreach (var tarefa in tarefasExcluidasAtualizadas)
                {
                    _banco.Tarefas.Update(tarefa);
                }
            }
            _banco.SaveChanges();

            return tarefasNovas.ToList();
        }
    }
}
