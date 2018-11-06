using AutoMapper;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Application;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Application.Data;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Identity;
using System;

namespace Insight.SaveTheBees.SelfServe.WebApi.Models
{
    /// <summary>
    /// This static class contains the methods to create the configurations used in the
    /// mapping mechanism.
    /// </summary>
    public static class MapperConfiguration
    {
        #region Methods

        /// <summary>
        /// Creates the mapper configurations used by an instance of a
        /// <see cref="Mapper" />.
        /// </summary>
        /// <returns>A mapper configuration as an <see cref="Action" /> expression.</returns>
        public static Action<IMapperConfigurationExpression> CreateConfiguration()
        {
            return config =>
            {
                config.CreateMap<UserDto, User>(MemberList.Source)
                    .ForSourceMember(src => src.Password, opts => opts.Ignore())
                    .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.EmailAddress))
                    .ForSourceMember(src => src.FirstName, opts => opts.Ignore())
                    .ForSourceMember(src => src.LastName, opts => opts.Ignore())
                    .ForSourceMember(src => src.FullName, opts => opts.Ignore());

                config.CreateMap<UserDto, ApplicationUser>(MemberList.Destination)
                    .ForMember(dest => dest.UserId, opts => opts.Ignore())
                    .ForMember(dest => dest.RowVersion, opts => opts.Ignore());

                config.CreateMap<ClusterDto, HiveCluster>(MemberList.Source);

                config.CreateMap<HiveCluster, ClusterOutputDto>(MemberList.Destination);

                config.CreateMap<HiveDto, Hive>(MemberList.Source);

                config.CreateMap<Hive, HiveOutputDto>(MemberList.Destination);
            };
        }

        #endregion
    }
}