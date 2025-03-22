using AutoMapper;
using IKEA.BLL.Models.Departments;
using IKEA.PL.Models.Departments;

namespace IKEA.PL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Department
            CreateMap<DepartmentDetailsToReturnDto, DepartmentEditViewModel>().ReverseMap();
            CreateMap<DepartmentEditViewModel, UpdateDepartmentDto>().ReverseMap();
            CreateMap<DepartmentEditViewModel, CreatedDepartmentDto>();

            #endregion
            #region Employee

            #endregion
        }

    }
}
