using Core.DTO;
using Data.Candidate;

namespace Core.CandidateService
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;

        public CandidateService(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<List<CandidateResponse>> SearchCandidatesAsync(string fullName)
        {
            var candidates = await _candidateRepository.SearchCandidatesAsync(fullName);
            return candidates.Select(c => new CandidateResponse
            {
                FullName = c.FullName,
                Email = c.Email
            }).ToList();
        }
    }
}