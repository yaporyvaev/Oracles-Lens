using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Contracts.Score;
using LeagueActivityBot.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeagueActivityBot.Services
{
    public class WeightsService
    {
        private readonly IRepository<ScoreWeights> _scoreWeightsRepository;

        public WeightsService(IRepository<ScoreWeights> scoreWeightsRepository)
        {
            _scoreWeightsRepository = scoreWeightsRepository;
        }
        
        public async Task AddScore(AddWeightsRequest request)
        {
            var entity = new ScoreWeights
            {
                Gold = request.Gold!.Value,
                Kda = request.Kda!.Value,
                Level = request.Level!.Value,
                CcTime = request.CcTime!.Value,
                DmgHealed = request.DmgHealed!.Value,
                DmgMitigated = request.DmgMitigated!.Value,
                DmgShielded = request.DmgShielded!.Value,
                DmgTaken = request.DmgTaken!.Value,
                DmgToChampions = request.DmgToChampions!.Value
            };

            await _scoreWeightsRepository.Add(entity);
        }
        
        public async Task<ScoreWeights> GetScore()
        {
            return await _scoreWeightsRepository.GetAll()
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();
        }
    }
}