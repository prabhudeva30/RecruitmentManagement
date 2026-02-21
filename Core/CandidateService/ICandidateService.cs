using Core.DTO;

namespace Core.CandidateService
{
    public interface ICandidateService
    {
        public Task<List<CandidateResponse>> SearchCandidatesAsync(string fullName);
    }
}