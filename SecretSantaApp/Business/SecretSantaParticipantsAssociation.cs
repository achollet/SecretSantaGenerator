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
            var potentialNominees = participants.ToList();
            var gifters = participants.ToList();

            while(potentialNominees.Any() && gifters.Any())
            {
                var randomIndexForGifter = new Random().Next(gifters.Count);
                var selectedGifter = gifters.ElementAt(randomIndexForGifter);

                var potentialNomineesForSelectedGifter = RemoveCurrentParticipantFromPotentialGifted(potentialNominees, selectedGifter);
                var potentialNomineesNotExcluded = RemoveParticipantsInCurrentParticipantExclusionList(potentialNomineesForSelectedGifter, selectedGifter).ToList();
                var potentialNomineesNotInSameTeam = RemovePotentialNomineesFromSelectedGifterTeam(potentialNomineesNotExcluded, selectedGifter).ToList();

                var nominee = new Participant();

                if (!potentialNomineesNotInSameTeam.Any())
                {
                    if (potentialNomineesNotExcluded.Count == 1)
                    {
                        nominee = potentialNomineesNotExcluded.FirstOrDefault();
                    }
                    else 
                    {
                        var randomNomineeIndex = new Random().Next(potentialNomineesNotExcluded.Count);
                        nominee = potentialNomineesNotExcluded.ElementAt(randomNomineeIndex);
                    }
                }
                else 
                {
                    if (potentialNomineesNotInSameTeam.Count == 1)
                    {
                        nominee = potentialNomineesNotInSameTeam.FirstOrDefault();
                    }
                    else
                    {
                        var randomNomineeIndex = new Random().Next(potentialNomineesNotInSameTeam.Count);
                        nominee = potentialNomineesNotInSameTeam.ElementAt(randomNomineeIndex);
                    }
                }
                
                if (nominee != null)
                {
                    secretSantaSelection.Add(selectedGifter, nominee as People);
                    gifters.Remove(selectedGifter);
                    potentialNominees.Remove(nominee);
                }
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

        private List<Participant> RemoveParticipantsInCurrentParticipantExclusionList(IEnumerable<Participant> participants, Participant currentParticipant)
        {
            if (currentParticipant.ExcludedNominees == null || !currentParticipant.ExcludedNominees.Any())
            {
                return participants.ToList();
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

        private IEnumerable<Participant> RemovePotentialNomineesFromSelectedGifterTeam(IEnumerable<Participant> participants, Participant currentParticipant)
        {
            var participantsNotInTeam = new List<Participant>();
            
            foreach(var participant in participants)
            {
                if (!currentParticipant.Equals(participant) && participant.Team != currentParticipant.Team)
                {
                    participantsNotInTeam.Add(participant);
                }
            }
            return participantsNotInTeam;
        }
    }
}