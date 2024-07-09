using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Vonage;
using Vonage.Request;
using Vonage.Video.Sessions.CreateSession;
using Vonage.Video.Authentication;
using System.Threading.Tasks;
using System.Linq;

namespace BasicVideoChatAngular.Controllers
{
    public class SessionController : Controller
    {
        private IConfiguration _Configuration;
        VonageClient client;
        Credentials creds;

        public SessionController(IConfiguration config)
        {
            _Configuration = config;
            creds = Credentials.FromAppIdAndPrivateKeyPath(_Configuration["ApiId"], _Configuration["PrivateKeyPath"]);
        }

        public class RoomForm
        {
            public string RoomName { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> GetSession([FromBody]RoomForm roomForm)
        {
            var roomName = roomForm.RoomName;
            string sessionId;
            string token;
            using (var db = new OpentokContext())
            {
                var room = db.Rooms.Where(r => r.RoomName == roomName).FirstOrDefault();
                if (room != null)
                {
                    sessionId = room.SessionId;

                    VideoTokenGenerator tokenGenerator = new VideoTokenGenerator();
                    var tokenres = tokenGenerator.GenerateToken(creds, TokenAdditionalClaims.Parse(sessionId));
                    token = tokenres.GetSuccessUnsafe().Token;
                    room.Token = token;
                    db.SaveChanges();
                }
                else
                {
   
                    var request = CreateSessionRequest.Default;

                    var response = await client.VideoClient.SessionClient.CreateSessionAsync(request);

                    if (response.IsSuccess)
                    {
                        sessionId = response.GetSuccessUnsafe().SessionId;
                    }
                    else
                    {
                        return BadRequest("Error");
                    }

                    //New way to generate token
                    VideoTokenGenerator tokenGenerator = new VideoTokenGenerator();
                    var tokenres = tokenGenerator.GenerateToken(creds, TokenAdditionalClaims.Parse(sessionId));

                    token = tokenres.GetSuccessUnsafe().Token;
                    var roomInsert = new Room
                    {
                        SessionId = sessionId,
                        Token = token,
                        RoomName = roomName
                    };
                    db.Add(roomInsert);
                    db.SaveChanges();
                }
            }
            return Json(new { sessionId = sessionId, token = token, appId = _Configuration["AppId"] });
        }
    }
}