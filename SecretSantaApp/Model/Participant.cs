using System.Collections.Generic;

namespace SecretSantaApp.Model
{
    public class Participant : People
    {
        public string EmailAddress { get; set; }
        public string Team { get; set; }
        public IEnumerable<People> ExcludedNominees { get; set; }

        public bool Equals(Participant participant)
        {
            return (this.FirstName == participant.FirstName && this.LastName == participant.LastName && this.EmailAddress == participant.EmailAddress);
        }
    }
}    