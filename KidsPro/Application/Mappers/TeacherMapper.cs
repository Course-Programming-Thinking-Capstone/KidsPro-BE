using Application.Dtos.Request.Teacher;
using Application.Dtos.Response.Teacher;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class TeacherMapper : Profile
    {
        public TeacherMapper()
        {
            #region Request
            CreateMap<TeacherContactInformation, TeacherContactRequest>().ReverseMap();
            CreateMap<TeacherResource, TeacherResourceRequest>().ReverseMap();
            CreateMap<TeacherProfile, TeacherProfileRequest>().ReverseMap();
            #endregion

            #region Response
            CreateMap<Teacher, TeacherResponse>()
                .ForMember(x => x.Profile, otp => otp.MapFrom(src => src.TeacherProfiles!=null && src.TeacherProfiles.Any()
                                                    ?src.TeacherProfiles.ToList(): new List<TeacherProfile>()))
                .ForMember(x => x.Resource, otp => otp.MapFrom(src => src.TeacherResources != null && src.TeacherResources.Any()
                                                    ? src.TeacherResources.ToList() : new List<TeacherResource>()))
                .ForMember(x => x.Contact, otp => otp.MapFrom(src => src.TeacherContactInformations != null && src.TeacherContactInformations.Any()
                                                    ? src.TeacherContactInformations.ToList() : new List<TeacherContactInformation>()))
                .ReverseMap();

            #endregion
        }
    }
}