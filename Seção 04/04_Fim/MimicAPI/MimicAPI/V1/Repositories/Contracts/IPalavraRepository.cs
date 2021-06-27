﻿using MimicAPI.V1.Models;
using MimicAPI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Repositories.Contracts
{
    public interface IPalavraRepository
    {
        ListaPaginacao<Palavra> ObterPalavras(PalavraUrlQuery query);

        Palavra Obter(int id);

        void Cadastrar(Palavra palavra);

        void Atualizar(Palavra palavra);

        void Deletar(int id);

    }
}
