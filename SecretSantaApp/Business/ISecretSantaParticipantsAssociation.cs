using System.Collections.Generic;
using SecretSantaApp.Model;

namespace SecretSantaApp.Business
{
    public interface ISecretSantaParticipantsAssociation
    {
        IEnumerable<Participant> RemoveDuplicateParticipants(IEnumerable<Participant> participants);
        IDictionary<Participant,People>  AssociateParticipantsTogether(IEnumerable<Participant> participants);
        IDictionary<Participant, People> AssociateParticipantsTogetherV2(IEnumerable<Participant> participants);
    }
}