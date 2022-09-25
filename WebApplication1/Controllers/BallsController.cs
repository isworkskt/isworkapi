using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BallsController : ControllerBase
    {
        private readonly FirestoreDb db = FirestoreDb.Create("iswork-d8ed0");
        [Authorize]
        [HttpGet("Borrowed")]
        public async Task<BallPublic[]> GetBorrowed()
        {
            QuerySnapshot dataw = await db.Collection("ball").WhereArrayContains(new FieldPath("users"), User.FindFirstValue("user_id")).GetSnapshotAsync();
            return dataw.Documents.Select(x => x.ConvertTo<BallPublic>()).ToArray();
        }
        // GET: api/<BallsController>
        [HttpGet("Public")]
        public async Task<BallPublic[]> GetPublic()
        {
            QuerySnapshot dataw = await db.Collection("ball").GetSnapshotAsync();
            return dataw.Documents.Select(x => x.ConvertTo<BallPublic>()).ToArray();
        }
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<Ball[]> Get()
        {
            QuerySnapshot dataw = await db.Collection("ball").GetSnapshotAsync();

            return dataw.Documents.Select(x => x.ConvertTo<Ball>()).ToArray();
        }

        // GET api/<BallsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Authorize(Policy = "Admin")]
        // POST api/<BallsController>
        [HttpPost]
        public async Task<string> Post([FromBody] Ball value)
        {
            DocumentReference s = await db.Collection("ball").AddAsync(value);
            return s.Id;


        }
        // PUT api/<BallsController>/5
        [Authorize]
        [HttpPut("{type}/{amount}")]
        public async Task<IActionResult> Put(string type, int amount)
        {
            QuerySnapshot dataw = await db.Collection("ball").WhereEqualTo(new FieldPath("type"), type).GetSnapshotAsync();
            if (dataw.Count == 0)
            {
                return NotFound(new { message = "Object not found" });

            }
            if (dataw[0].ConvertTo<BallPublic>().StockLeft - amount < 0)
            {
                return NotFound(new { message = "No stock left." });
            }
            Dictionary<string, object> user = new()
            {
                { "user_id",User.FindFirstValue("user_id") },
                { "amount",amount },
                { "date",Timestamp.GetCurrentTimestamp() }
            };
            Dictionary<string, object> updates = new()
            {
    { "users", FieldValue.ArrayUnion(user) },
};



            _ = await dataw[0].Reference.UpdateAsync(updates);
            return Ok(new
            {
                message = "Ok"
            });

        }
        [Authorize]
        // DELETE api/<BallsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
