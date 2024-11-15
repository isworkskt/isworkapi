﻿using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using System.Text.Json.Serialization;

namespace WebApplication1
{
    [FirestoreData]
    public class Ball
    {
        [FirestoreProperty("type")]
        public string? Type { get; set; }
        [FirestoreProperty("amount")]
        public int Amount { get; set; } = 0;
        [FirestoreProperty("users")]
        public User[] Users { get; set; } = Array.Empty<User>();
        public int StockLeft => Amount - Users.Sum(user => user.Amount);

    }
    [FirestoreData]
    public class BallPublic
    {

        [FirestoreProperty("type")]
        public string? Type { get; set; }
        [FirestoreProperty("amount")]
        [JsonIgnore]
        public int Amount { get; set; } = 0;
        [FirestoreProperty("users")]
        [JsonIgnore]
        public User[] Users { get; set; } = Array.Empty<User>();
        public int Used => Users.Sum(user =>
        {
            DateTime startday = DateTime.Today.ToUniversalTime();
            DateTime endday = DateTime.Today.AddDays(1).AddSeconds(-1).ToUniversalTime();
            if (user.Date < Timestamp.FromDateTime(endday) && user.Date > Timestamp.FromDateTime(startday))
            {
                return user.Amount;
            };
            return 0;
        });
        public int StockLeft => Amount - Used;

    }

    public class ClientResponse
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("amount")]
        public int? Amount { get; set; }
    }
    [FirestoreData]
    public class User
    {
        [FirestoreProperty("user_id")]
        [JsonPropertyName("user_id")]
        [JsonIgnore]
        public string Uid { get; set; } = "";
        public string Email => GetEmail(Uid).Result;

        public async Task<string> GetEmail(string Uid)
        {
            UserRecord d = await FirebaseAuth.DefaultInstance.GetUserAsync(Uid);
            return d.Email;
        }

        [FirestoreProperty("amount")]
        [JsonPropertyName("amount")]
        public int Amount { get; set; }
        [FirestoreProperty("date")]
        [JsonIgnore]
        public Timestamp Date { get; set; }
        public DateTime Time => Date.ToDateTime().ToLocalTime();
        public User()
        {
            Uid = Uid;
            Amount = Amount;
            Date = Date;
        }
    }

}
