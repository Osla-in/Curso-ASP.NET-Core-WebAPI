using MinhasTarefasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.Repositories.Contracts
{
    public interface IUsuarioRepository
    {
        ApplicationUser Obter(string email, string senha);

        void Cadastrar(ApplicationUser usuario, string senha);
    }
}
