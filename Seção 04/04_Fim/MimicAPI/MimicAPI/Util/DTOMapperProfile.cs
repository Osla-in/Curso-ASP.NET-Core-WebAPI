using AutoMapper;
using MimicAPI.V1.Models;
using MimicAPI.V1.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Util
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            CreateMap<Palavra, PalavraDTO>();
            CreateMap<ListaPaginacao<Palavra>, ListaPaginacao<PalavraDTO>>();
        }
    }
}
