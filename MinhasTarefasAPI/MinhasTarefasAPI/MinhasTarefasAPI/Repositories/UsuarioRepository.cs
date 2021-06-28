using Microsoft.AspNetCore.Identity;
using MinhasTarefasAPI.Models;
using MinhasTarefasAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser Obter(string email, string senha)
        {
            var usuario = _userManager.FindByEmailAsync(email).Result;

            if (_userManager.CheckPasswordAsync(usuario, senha).Result)
            {
                return usuario;
            }
            else
            {
                throw new Exception("Usuário não localizado.");
            }
        }

        public void Cadastrar(ApplicationUser usuario, string senha)
        {
            var resultado = _userManager.CreateAsync(usuario, senha).Result;
            if (!resultado.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach(var erro in resultado.Errors)
                {
                    sb.Append(erro.Description);
                }

                throw new Exception("Usuário não cadastrado.");
            }
        }

        
    }
}
