using Data.Candidate;
using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly RadancyDbContext _rDbContext;

        public CandidateRepository(RadancyDbContext radancyDbContext)
        {
            _rDbContext = radancyDbContext;
        }

        public async Task<List<CandidateEntity>> SearchCandidatesAsync(string fullName)
        {
            return await _rDbContext.Candidates
                .Where(candidate => candidate.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
    }
}