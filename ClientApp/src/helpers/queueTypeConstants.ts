import { QueueType } from "../types/GameInfoApiResponse";

export const getQueueType = (id: QueueType): string => {
  switch (id) {
    case QueueType.Custom:
      return "Custom";
    case QueueType.NormalDraft:
      return "Normal Draft";
    case QueueType.RankedSoloDuo:
      return "Ranked Solo\\Duo";
    case QueueType.NormalBlind:
      return "Normal Blind";
    case QueueType.RankedFlex:
      return "Ranked Flex";
    case QueueType.ARAM:
      return "ARAM";
    case QueueType.Clash:
      return "Clash";
    case QueueType.BotGame1:
    case QueueType.BotGame2:
    case QueueType.BotGame3:
      return "Bot game";
    case QueueType.ARURF1:
    case QueueType.ARURF2:
      return "ARURF";
    case QueueType.OneForAll:
      return "One for All";
    case QueueType.NexusBlitz1:
    case QueueType.NexusBlitz2:
      return "Nexus Blitz";
    case QueueType.UltimateSpellBook:
      return "Ultimate Spellbook";
    case QueueType.PickUrf:
      return "Pick Urf";
    default:
      return "Unknown game mod";
  }
};
