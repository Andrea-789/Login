using System;

namespace GestUser.Dtos
{
    public class JwtTokenDto
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public bool Expired { get; set; }
        public string Enabled { get; set; }

        public JwtTokenDto(string token, string id, bool expired, string enabled)
        {
            this.Token = token;
            this.Id = id;
            this.Expired = expired;
            this.Enabled = enabled;
        }

    }
}
