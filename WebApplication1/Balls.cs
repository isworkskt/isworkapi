using Google.Cloud.Firestore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication1
{
    [FirestoreData]
    public class Ball
    {
        [FirestoreProperty("name")]
        public string? Name { get; set; }
        [FirestoreProperty("amount")]
        public int Amount { get; set; } = 0;
        [FirestoreProperty("users")]
        public string[] Users { get; set; } = Array.Empty<string>();
        public int StockLeft => Amount - Users.Length;
        
    }
    [FirestoreData]
    public class BallPublic
    {

        [FirestoreProperty("name")]
        public string? Name { get; set; }
        [FirestoreProperty("amount")]
        [JsonIgnore]
        public int Amount { get; set; } = 0;
        [FirestoreProperty("users")]
        [JsonIgnore]
        public string[] Users { get; set; } = Array.Empty<string>();
        public int StockLeft => Amount - Users.Length;

    }
}
