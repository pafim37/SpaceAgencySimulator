using AutoMapper;
using Sas.BodySystem.Service.Profiles;
using Xunit;

namespace Sas.BodySystem.Tests
{
    public class MappingTest
    {
        [Fact]
        public void BodySystemMappingTest()
        {
            BodySystemProfile bodySystemprofile = new();
            BodyProfile bodyProfile = new();
            VectorProfile vectorProfile = new();
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(bodySystemprofile);
                cfg.AddProfile(bodyProfile);
                cfg.AddProfile(vectorProfile);
            });
            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void BodyMappingTest()
        {
            BodyProfile bodyProfile = new();
            VectorProfile vectorProfile = new();
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(bodyProfile);
                cfg.AddProfile(vectorProfile);
            });
            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void VectorMappingTest()
        {
            VectorProfile vectorProfile = new();
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(vectorProfile);
            });
            configuration.AssertConfigurationIsValid();
        }
    }
}
