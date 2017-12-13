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
            var alreadyAttributedParticipants = new List<Participant>();
            
            // Choose every potential persones to gift for each gifter
            foreach(var gifter in participants)
            {
                GetPotentialNomineeForAParticipant(participants, gifter);
            }
            //

            foreach(var participant in participants)
            {
                if (participant.PotentialNominees.Count() == 1)
                {
                    alreadyAttributedParticipants.Add(participant.PotentialNominees.FirstOrDefault());
                    RemoveAlreadyAttributedParticipants(participant, participants, alreadyAttributedParticipants);
                }
            }





                var randomIndex = new Random().Next(potentialGifted.Length);

                var gifted = new People{ FirstName = potentialGifted[randomIndex].FirstName, LastName = potentialGifted[randomIndex].LastName};

                secretSantaSelection.Add(gifter, gifted);
                alreadyAttibutedParticipants.Add(potentialGifted[randomIndex]);
            //}
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

        private void GetPotentialNomineeForAParticipant(IEnumerable<Participant> participants, Participant currentParticipant)
        {
            var participantsExceptCurrentParticipant = RemoveCurrentParticipantFromPotentialGifted(participants, currentParticipant);
            
            var participantsNotExcluded = RemoveParticipantsInCurrentParticipantExclusionList(participantsExceptCurrentParticipant, currentParticipant);

            var participantsNotInTheCurrentOneTeam = ExcludedParticipantFromSameTeam(participantsNotExcluded, currentParticipant);

            if (participantsNotInTheCurrentOneTeam.Any())
            {
                currentParticipant.PotentialNominees = participantsNotInTheCurrentOneTeam;
            }
            else 
            {
                currentParticipant.PotentialNominees = participantsNotExcluded;
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
            if (currentParticipant.ExcludedNominees == null || !currentParticipant.ExcludedNominees.Any())
            {
                return participants;
            }

            var participantsWithoutCurrentParticipantExcludedOne = new List<Participant>();
            foreach(var participant in participants)
            {
                if (!currentParticipant.ExcludedNominees.Any(n => n.FirstName == participant.FirstName && 
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

        private void RemoveAlreadyAttributedParticipants(Participant currentParticipant, IEnumerable<Participant> participants, IEnumerable<Participant> alreadyAttributedParticipants)
        {
            foreach(var participant in participants)
            {
                
                if (!participant.Equals(currentParticipant) && participant.ExcludedNominees.Count() > 1)
                {
                    var excludeAlreadyAttributedParticipantFromPotentialNomineeList = participant.PotentialNominees.Except(alreadyAttributedParticipants);
                    if (excludeAlreadyAttributedParticipantFromPotentialNomineeList.Any())
                    {
                        participant.PotentialNominees = excludeAlreadyAttributedParticipantFromPotentialNomineeList;
                    }
                }
            }
        } 
    }
}