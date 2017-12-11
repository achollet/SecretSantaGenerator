using System.Collections.Generic;
using SecretSantaApp.Model;
using System.Linq;
using System;

namespace SecretSantaApp.Business
{
    public class SecretSantaPaticipantsAssociation : ISecretSantaParticipantsAssociation
    {
        public IDictionary<Participant, People> AssociateParticipantsTogether(IEnumerable<Participant> participants)
        {
            var secretSantaSelection = new Dictionary<Participant, People>();
            var alreadyAttibutedParticipants = new List<Participant>();
            // Choose one persone to gift for each gifter
            foreach(var gifter in participants)
            {
                var potentialGifted = GetPotentialNomineeForAParticipant(participants, alreadyAttibutedParticipants, gifter);

                var randomIndex = new Random().Next(potentialGifted.Length);

                var gifted = new People{ FirstName = potentialGifted[randomIndex].FirstName, LastName = potentialGifted[randomIndex].LastName};

                secretSantaSelection.Add(gifter, gifted);
                alreadyAttibutedParticipants.Add(potentialGifted[randomIndex]);
            }
            return secretSantaSelection;
        }

        public IEnumerable<Participant> RemoveDuplicateParticipants(IEnumerable<Participant> participants)
        {
            var cleanParticipantsList = new List<Participant>();

            foreach(var participant in participants)
            {
                if (!cleanParticipantsList.Any(p => p.FirstName == participant.FirstName && 
                                                    p.LastName == participant.LastName &&
                                                    p.EmailAddress == participant.EmailAddress))
                {
                    cleanParticipantsList.Add(participant);
                }
            }
            return cleanParticipantsList;
        }

        private Participant[] GetPotentialNomineeForAParticipant(IEnumerable<Participant> participants, IEnumerable<Participant> participantsAlreadyAttributed,Participant currentParticipant)
        {
            var participantsExceptCurrentParticipant = RemoveCurrentParticipantFromPotentialGifted(participants, currentParticipant);
            
            var participantsNotAlreadyAttributed = participantsExceptCurrentParticipant.Except(participantsAlreadyAttributed);

            var participantsNotExcluded = RemoveParticipantsInCurrentParticipantExclusionList(participantsNotAlreadyAttributed, currentParticipant);

            var participantsNotInTheCurrentOneTeam = ExcludedParticipantFromSameTeam(participantsNotExcluded, currentParticipant);

            if (participantsNotInTheCurrentOneTeam.Any())
            {
                return participantsNotInTheCurrentOneTeam.ToArray();
            }
            else 
            {
                return participantsNotExcluded.ToArray();
            }
        }

        private IEnumerable<Participant> RemoveCurrentParticipantFromPotentialGifted(IEnumerable<Participant> participants, Participant currentParticipant)
        {
            var participantsExceptCurrentOne = new List<Participant>();
            foreach(var participant in participants)
            {
                if (participant.FirstName != currentParticipant.FirstName && 
                    participant.LastName != currentParticipant.LastName && 
                    participant.EmailAddress != currentParticipant.EmailAddress)
                {
                    participantsExceptCurrentOne.Add(participant);
                }
            }
            return participantsExceptCurrentOne;
        }

        private IEnumerable<Participant> RemoveParticipantsInCurrentParticipantExclusionList(IEnumerable<Participant> participants, Participant currentParticipant)
        {
            if (currentParticipant.ExcludedNominee == null || !currentParticipant.ExcludedNominee.Any())
            {
                return participants;
            }

            var participantsWithoutCurrentParticipantExcludedOne = new List<Participant>();
            foreach(var participant in participants)
            {
                if (!currentParticipant.ExcludedNominee.Any(n => n.FirstName == participant.FirstName && 
                                                                 n.LastName == participant.LastName))
                {
                    participantsWithoutCurrentParticipantExcludedOne.Add(participant);
                }
            }         
            return participantsWithoutCurrentParticipantExcludedOne;
        } 

        private IEnumerable<Participant> ExcludedParticipantFromSameTeam(IEnumerable<Participant> potentialNominees, Participant gifter)
        {
            var potentialGifted = potentialNominees.Where( pn => pn != gifter && pn.Team != gifter.Team);
            return potentialGifted;
        } 
    }
}