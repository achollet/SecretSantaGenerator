using System.Collections.Generic;

namespace SecretSantaApp.Model
{
    public class Participant : People
    {
        public string EmailAddress { get; set; }
        public int Team { get; set; }
        public List<People> ExcludedNominee { get; set; }
    }
}    