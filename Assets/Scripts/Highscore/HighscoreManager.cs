using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Highscore
{
    [System.Serializable]
    public class Highscores
    {
        public float easy = -1f;
        public float normal = -1f;
        public float hard = -1f;
    }
    
    public class HighscoreManager : MonoBehaviour
    {
        public static HighscoreManager Instance;

        private Highscores _highscores;
        private static string HighscorePath => Path.Combine(Application.persistentDataPath, "highscores.dat");

        private string _currentDifficulty;
        

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadHighscores();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetDifficulty(string difficulty) => _currentDifficulty = difficulty;

        public string GetCurrentDifficulty() => _currentDifficulty;
        
        public void SaveBestTime(float timePlayed, string difficulty)
        {
            // Save based on difficulty level
            switch (difficulty)
            {
                case "easy":
                    if (timePlayed > _highscores.easy)
                        _highscores.easy = timePlayed;
                    break;
                case "normal":
                    if (timePlayed > _highscores.normal)
                        _highscores.normal = timePlayed;
                    break;
                case "hard":
                    if (timePlayed > _highscores.hard)
                        _highscores.hard = timePlayed;
                    break;
            }

            // Save the updated times
            var formatter = new BinaryFormatter();
            using var stream = new FileStream(HighscorePath, FileMode.Create);
            formatter.Serialize(stream, _highscores);
        }

        private void LoadHighscores()
        {
            if (File.Exists(HighscorePath))
            {
                var formatter = new BinaryFormatter();
                using var stream = new FileStream(HighscorePath, FileMode.Open);
                _highscores = formatter.Deserialize(stream) as Highscores;
            }
            else
            {
                _highscores = new Highscores();
            }
        }
    
        public string GetBestTimeString(string difficulty)
        {
            var time = difficulty switch
            {
                "easy" => _highscores.easy,
                "normal" => _highscores.normal,
                "hard" => _highscores.hard,
                _ => -1f
            };

            return time < 0 ? "N/A" : FormatTime(time);
        }

        private string FormatTime(float time)
        {
            var minutes = (int)(time / 60);
            var seconds = (int)(time % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}
