# Santa Project Generator

This Console App has been develloped to generate anominously and randomly a Secret Santa based on a list of participants. After pairing participants together, it will send an email explaining who is the person that will recieve the gift, how much to put in the gift (maximum) and the due date.

## Getting Started

To develop and test this app, just fork the project. Once you've cloned it on your machine, fill the SecretSanta.json file with informations needed to run the app.

### Prerequisites

This application has been developp using .Net Core 2.0.

### Installing

#### SecretSanta.json file 

To run properly this app needs the SecretSanta.json file filled with some mandatory informations. This file is structured as follow :

```
{
  "configuration" : {...},
  "participants" : [{...}]
}
```

##### configuration info

The configuration object is self-explainatory and will contains any informations regarding the general functioning of the app. It will contains values of **maximum amount** to spend and **due date**, 

```
{
  "configuration" : {
    "MaxAmount" : 10,
    "DeliveryDate" : "2018, January the 10th",
    "EmailSettings" : {}
    }
}
```

but also settings needed to send e-mails, all fields are mandatory :

```
"EmailSettings" : {
            "SmtpServer" : "",
            "EmailUserName" : "",
            "EmailPassword" : "",
            "EmailAddress" :"", 
            "EmailSubject" : "",
            "EmailBody" : "whatever you want to say in the email."
        }
```

The e-mail body is the core of the message and can be html + embeded css code to add some style at the email.

##### participants list

A participant is defined by 5 fields, **FirstName**, **LastName** and **EmailAddress** that are **mandatory** and need to be unique and 2 optionals fields, **Team** and **ExcludedNominees**.

```
{
  "FirstName" : "John", 
  "LastName" : "Doe" , 
  "Team" : "Red", 
  "EmailAddress" : "SecretSanta.Checkout@gmail.com",
  "ExcludedNominees" : [{...}]
  }
```

**ExcludedNominee** is a list of people with which the participant can't be associated and has the following structure :

```
ExcludedNominees : [{FirstName = "People1", LastName = "People1"},
                    {FirstName = "People12", LastName = "People2"}]
```

## Running the tests

:warning: In order to run test you need to change some part of the code in *SecretSantaPaticipantsAssociation.cs* at line 83 :

``` 
if (participant.FirstName != currentParticipant.FirstName &&
    participant.LastName != currentParticipant.LastName   && 
    participant.EmailAddress != currentParticipant.EmailAddress && 
    participant.Team != currentParticipant.Team)
```
should be replace by 
``` 
if (participant.FirstName != currentParticipant.FirstName &&
    participant.LastName != currentParticipant.LastName)
```
To allow to have the same email address for all participants. 

### Expected output 

```
**************************************************************
Retrieving SecretSanta.json file
          Deserializing configuration properties
          Deserializing participants list
          
          Shuffling and association gifter/gifted :
                  1/3 participant(s) associated
                  2/3 participant(s) associated
                  3/3 participant(s) associated
          
          Sending emails :
                  email 1/3
                  email 2/3
                  email 3/3
Secret Santa Shuffle Emailing terminated
**************************************************************
```

## Deployment

Add additional notes about how to deploy this on a live system

## Authors

* **Chollet Alexis** - *Initial work* - [achollet](https://github.com/achollet)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* A Job interview at Serensia : it was the subject of the technical test.
