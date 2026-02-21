using Data.Entities;

namespace Data.Candidate
{
    public interface ICandidateRepository
    {
        public Task<List<CandidateEntity>> SearchCandidatesAsync(string fullName);
    }
}