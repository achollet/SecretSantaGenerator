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
                var potentialNomineesNotInSameTeam = RemovePotentialNomineesFromSameTeam(participant);
                var potentialNominees = participant.PotentialNominees;
                var rand = new Random();

                var nominee = potentialNomineesNotInSameTeam.Any() 
                            ? (potentialNomineesNotInSameTeam.ToArray())[rand.Next(potentialNomineesNotInSameTeam.Count()-1)] 
                            : (potentialNominees.ToArray())[rand.Next(potentialNominees.Count()-1)];

                secretSantaSelection.Add(participant, nominee);
                alreadyAttributedParticipants.Add(nominee);
                RemoveAlreadyAttributedParticipants(participant, participants, nominee);
            }

            return secretSantaSelection;
        }

        public IDictionary<Participant, People> AssociateParticipantsTogetherV2(IEnumerable<Participant> participants)
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
                    var randomNomineeIndex = new Random().Next(potentialNomineesNotExcluded.Count);
                    nominee = potentialNomineesNotExcluded.ElementAt(randomNomineeIndex);
                }

                if (potentialNomineesNotInSameTeam.Count == 1)
                {
                    nominee = potentialNomineesNotInSameTeam.FirstOrDefault();
                }
                else
                {
                    var randomNomineeIndex = new Random().Next(potentialNomineesNotInSameTeam.Count);
                    nominee = potentialNomineesNotInSameTeam.ElementAt(randomNomineeIndex);
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

        private void GetPotentialNomineeForAParticipant(IEnumerable<Participant> participants, Participant currentParticipant)
        {
            var participantsExceptCurrentParticipant = RemoveCurrentParticipantFromPotentialGifted(participants, currentParticipant);
            
            currentParticipant.PotentialNominees = RemoveParticipantsInCurrentParticipantExclusionList(participantsExceptCurrentParticipant, currentParticipant);
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

        private void RemoveAlreadyAttributedParticipants(Participant currentParticipant, IEnumerable<Participant> participants, Participant nominee)
        {
            foreach(var participant in participants)
            {
                if (!participant.Equals(currentParticipant) && participant.PotentialNominees.Count() > 1)
                {
                    if (participant.PotentialNominees.Any(n => n.Equals(nominee)))
                    {
                        participant.PotentialNominees.Remove(nominee);
                    }
                }
            }
        } 

        private IEnumerable<Participant> RemovePotentialNomineesFromSameTeam(Participant currentParticipant)
        {
            var potentialNomineesNotInSameTeam = new List<Participant>();

            foreach(var nominee in currentParticipant.PotentialNominees)
            {
                if (nominee.Team != currentParticipant.Team)
                {
                    potentialNomineesNotInSameTeam.Add(nominee);
                }
            }

            return potentialNomineesNotInSameTeam;
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