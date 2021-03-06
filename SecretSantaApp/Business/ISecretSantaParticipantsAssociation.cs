using System.Collections.Generic;
using SecretSantaApp.Model;

namespace SecretSantaApp.Business
{
    public interface ISecretSantaParticipantsAssociation
    {
        IEnumerable<Participant> RemoveDuplicateParticipants(IEnumerable<Participant> participants);
        IDictionary<Participant,Participant>  AssociateParticipantsTogether(IEnumerable<Participant> participants);
    }
}