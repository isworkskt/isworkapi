using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class BallsController : ControllerBase
    {
        readonly FirestoreDb db = FirestoreDb.Create("iswork-d8ed0");


        // GET: api/<BallsController>
        [HttpGet("Public")]
        public async Task<BallPublic[]> GetPublic()
        {
            QuerySnapshot dataw = await db.Collection("ball").GetSnapshotAsync();

            return dataw.Documents.ToArray().Select(x => x.ConvertTo<BallPublic>()).ToArray();
        }
        [Authorize]
        [HttpGet]
        public async Task<Ball[]> Get()
        {
            QuerySnapshot dataw = await db.Collection("ball").GetSnapshotAsync();

            return dataw.Documents.ToArray().Select(x => x.ConvertTo<Ball>()).ToArray();
        }

        // GET api/<BallsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Authorize]
        // POST api/<BallsController>
        [HttpPost]
        public async Task<string> Post([FromBody] Ball value)
        {
            DocumentReference s = await db.Collection("ball").AddAsync(value);
            return s.Id;


        }
        [Authorize]
        // PUT api/<BallsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        [Authorize]
        // DELETE api/<BallsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
