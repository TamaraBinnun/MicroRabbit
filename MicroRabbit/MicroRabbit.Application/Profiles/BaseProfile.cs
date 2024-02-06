using AutoMapper;
using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Application.Profiles
{
    public class BaseProfile<T, TResponse, AddTRequest, UpdateTRequest> : Profile
        where T : BaseModel
        where TResponse : BaseResponse
        where AddTRequest : class
        where UpdateTRequest : UpdateBaseRequest
    {
        public BaseProfile()
        {
            //Service.GetAll + GetByIdAsync + AddAsync
            CreateMap<T, TResponse>();

            //Service.AddAsync
            CreateMap<AddTRequest, T>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}