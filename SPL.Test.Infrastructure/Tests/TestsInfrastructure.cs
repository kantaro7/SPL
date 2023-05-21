using AutoMapper;

using Microsoft.EntityFrameworkCore.Storage;

using SPL.Tests.Infrastructure.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SPL.Tests.Infrastructure.Tests
{
    public class TestsInfrastructure : ITestsInfrastructure
    {

        private readonly dbTestsSPLContext _dbContext;
        private readonly IMapper _Mapper;

        public TestsInfrastructure(IMapper Map, dbTestsSPLContext dbContext)
        {
            this._Mapper = Map;
            _dbContext = dbContext;
        }


        #region Methods

        public Task<List<SPL.Domain.SPL.Tests.Tests>> GetTests()
        {
           return Task.FromResult(this._Mapper.Map<List<SPL.Domain.SPL.Tests.Tests>>(_dbContext.SplReportes.AsNoTracking().AsEnumerable()));

        }
       
        #endregion


    }
}
