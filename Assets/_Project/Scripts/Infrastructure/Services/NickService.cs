using System;
using UnityEngine;

namespace Assets._Project.Scripts.Infrastructure.Services
{
    public interface IMessageService
    {
        event Action<string> onSendMessage;
        string GetNick();
        void Save(string name);
        string GenerateNick();
        void Send(string message);
    }
    public class NickService : IMessageService
    {
        private const string PLAYER_NICK_PREFS = "PLAYER_NICK_PREFS";
        private readonly string[] _randomNames = {
            "Warrior", "Mage", "Ranger", "Knight", "Samurai", "Ninja", "Pirate", "Viking", "Paladin", "Archer",
            "Wizard", "Sorcerer", "Hunter", "Assassin", "Gladiator", "Champion", "Hero", "Legend", "Master", "Lord"
        };

        public event Action<string> onSendMessage;

        public void Save(string name)
        {
            PlayerPrefs.SetString(PLAYER_NICK_PREFS, $"{name}");
        }

        public string GenerateNick()
        {
            var index = UnityEngine.Random.Range(0, _randomNames.Length - 1);
            string name = $"{_randomNames[index]}";
            return name;
        }
        public string GetNick() => PlayerPrefs.GetString(PLAYER_NICK_PREFS);

        public void Send(string message)
        {
            onSendMessage?.Invoke(message);
        }
    }
}