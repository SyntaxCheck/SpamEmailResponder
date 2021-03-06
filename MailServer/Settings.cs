﻿using System.Collections.Generic;

public class Settings
{
    private string emailAddress, password, myName, myFakeAddress, myFakeBirthdate, pathToMyFakeID, myFakePhoneNumber, myFakeOccupation, 
        myFakeGender, myFakeMaritalStatus, myFakeCountry, outgoingMessageIdDomainName, minutesDelayBeforeAnsweringAnEmail, enableLongMessageTypeReplies, enableShortMessageTypeReplies, enableSendingTransferReceiptsImageFiles;
    private int longMessageUpperLimit, shortMessageLowerLimit;
    private List<string> names, greeting, signoff, introduction, introductionOpening, introductionClosing, personDescriptionMale, personDescriptionFemale, memory, followupLine, acquaintance, 
        locations, products, paymentMethods, randomThoughts, devices, consignmentBoxImageIncluded, consignmentBoxImageNotIncluded, responseLongMessageType, responseShortMessageType, pathToTransferReceipts, emailAddressesToIgnore;
    private List<string> questionsNotListening, questionsJokingAround, questionsNotAnswering, questionsHowAreYou, questionsEmail, questionsPassword,
        questionsNotUnderstanding, questionsPermission, questionsSpokenLanguage, questionsTrust, questionsWhyNoAnswer,
        questionsPhoneNumber, questionsAddress, questionsID, questionsAlreadyIncludedID, questionsCannotOpenAttachment, questionsWeAreCaught,
        questionsBirthdate, questionsBetterPhoto, questionsOccupation, questionsGender, questionsMaritalStatus, questionsCountry, questionsName, questionsProvideDetails,
        questionsAreYouReady, questionsChangeContactMethod, questionsContactMeLater, questionsAreYouOnboard, questionsPayTheFee, questionsTheyConfused,
        questionsHowBigOfLoan, questionsMustPayBefore, questionsMeetUs, questionsFillOutForm, questionsGetBackToUs, questionsNeedBankDetails,
        questionsWhatTypeOfProof, questionsHowDoYouWantFundsReleased, questionsWeCantDoThat, questionsContactTheBank, questionsAreYouMember,
        questionsDidYouSeeOurMessage, questionsInvalidAddress, questionsTellUsWhatTheyAskedYouToDo, questionsTellUsAboutYourself, questionsContactThePerson,
        questionsAutomatedProgram, questionsUseWalmartToPay, questionsHowMuchMoneyDoIHave, questionsSendTransferReceipt, questionsAlreadyToldYou,
        questionsTrustUs;
    private List<string> responseOpeningBlankEmailWithAttachment, responseOpeningLottery, responseOpeningOilAndGas, responseOpeningOilAndGasQuestionList, responseOpeningIlluminati, 
        responseOpeningConsignmentBox, responseOpeningDeathOrDying, responseOpeningLoanOffer, responseOpeningMoneyStorage, responseOpeningAtmCard, responseOpeningPolice, 
        responseOpeningGenericPayment, responseOpeningInvestor, responseOpeningMoneyHack, responseOpeningJobOffer, responseOpeningSellingProducts, responseOpeningFreeMoney, 
        responseOpeningInformationGathering, responseOpeningPhishing, responseOpeningScamVictim, responseOpeningForeignLanguage, responseOpeningGenericAdvertisement, responseOpeningShipping,
        responseOpeningRefugee, responseOpeningAccountProblem, responseOpeningProductSupplier, responseOpeningClickTheLink;
    private List<string> responseContinuedBlankEmailWithAttachment, responseContinuedLottery, responseContinuedOilAndGas, responseContinuedIlluminati, responseContinuedConsignmentBox, 
        responseContinuedDeathOrDying, responseContinuedLoanOffer, responseContinuedMoneyStorage, responseContinuedAtmCard, responseContinuedPolice, responseContinuedGenericPayment, 
        responseContinuedInvestor, responseContinuedMoneyHack, responseContinuedBuildTrust, responseContinuedInheritance, responseContinuedBeneficiary, responseContinuedJobOffer, 
        responseContinuedSellingProducts, responseContinuedFreeMoney, responseContinuedInformationGathering, responseContinuedPhishing, responseContinuedScamVictim,
        responseContinuedForeignLanguage, responseContinuedGenericAdvertisement, responseContinuedShipping, responseContinuedRefugee, responseContinuedAccountProblem,
        responseContinuedProductSupplier, responseContinuedClickTheLink;

    #region Get/sets
    public string EmailAddress
    {
        get { return emailAddress; }
        set { emailAddress = value; }
    }
    public string Password
    {
        get { return password; }
        set { password = value; }
    }
    public string MyName
    {
        get { return myName; }
        set { myName = value; }
    }
    public string MyFakeAddress
    {
        get { return myFakeAddress; }
        set { myFakeAddress = value; }
    }
    public string MyFakePhoneNumber
    {
        get { return myFakePhoneNumber; }
        set { myFakePhoneNumber = value; }
    }
    public string MyFakeBirthdate
    {
        get { return myFakeBirthdate; }
        set { myFakeBirthdate = value; }
    }
    public string MyFakeCountry
    {
        get { return myFakeCountry; }
        set { myFakeCountry = value; }
    }
    public string MyFakeGender
    {
        get { return myFakeGender; }
        set { myFakeGender = value; }
    }
    public string MyFakeMaritalStatus
    {
        get { return myFakeMaritalStatus; }
        set { myFakeMaritalStatus = value; }
    }
    public string MyFakeOccupation
    {
        get { return myFakeOccupation; }
        set { myFakeOccupation = value; }
    }
    public string PathToMyFakeID
    {
        get { return pathToMyFakeID; }
        set { pathToMyFakeID = value; }
    }
    public string OutgoingMessageIdDomainName
    {
        get { return outgoingMessageIdDomainName; }
        set { outgoingMessageIdDomainName = value; }
    }
    public string MinutesDelayBeforeAnsweringAnEmail
    {
        get { return minutesDelayBeforeAnsweringAnEmail; }
        set { minutesDelayBeforeAnsweringAnEmail = value; }
    }
    public string EnableLongMessageTypeReplies
    {
        get { return enableLongMessageTypeReplies; }
        set { enableLongMessageTypeReplies = value; }
    }
    public string EnableShortMessageTypeReplies
    {
        get { return enableShortMessageTypeReplies; }
        set { enableShortMessageTypeReplies = value; }
    }
    public string EnableSendingTransferReceiptsImageFiles
    {
        get { return enableSendingTransferReceiptsImageFiles; }
        set { enableSendingTransferReceiptsImageFiles = value; }
    }
    public int LongMessageUpperLimit
    {
        get { return longMessageUpperLimit; }
        set { longMessageUpperLimit = value; }
    }
    public int ShortMessageLowerLimit
    {
        get { return shortMessageLowerLimit; }
        set { shortMessageLowerLimit = value; }
    }
    public List<string> Acquaintance
    {
        get { return acquaintance; }
        set { acquaintance = value; }
    }
    public List<string> PaymentMethods
    {
        get { return paymentMethods; }
        set { paymentMethods = value; }
    }
    public List<string> FollowupLine
    {
        get { return followupLine; }
        set { followupLine = value; }
    }
    public List<string> Greeting
    {
        get { return greeting; }
        set { greeting = value; }
    }
    public List<string> Introduction
    {
        get { return introduction; }
        set { introduction = value; }
    }
    public List<string> Products
    {
        get { return products; }
        set { products = value; }
    }
    public List<string> Devices
    {
        get { return devices; }
        set { devices = value; }
    }
    public List<string> IntroductionClosing
    {
        get { return introductionClosing; }
        set { introductionClosing = value; }
    }
    public List<string> IntroductionOpening
    {
        get { return introductionOpening; }
        set { introductionOpening = value; }
    }
    public List<string> Memory
    {
        get { return memory; }
        set { memory = value; }
    }
    public List<string> Names
    {
        get { return names; }
        set { names = value; }
    }
    public List<string> PersonDescriptionMale
    {
        get { return personDescriptionMale; }
        set { personDescriptionMale = value; }
    }
    public List<string> PersonDescriptionFemale
    {
        get { return personDescriptionFemale; }
        set { personDescriptionFemale = value; }
    }
    public List<string> Locations
    {
        get { return locations; }
        set { locations = value; }
    }
    public List<string> Signoff
    {
        get { return signoff; }
        set { signoff = value; }
    }
    public List<string> EmailAddressesToIgnore
    {
        get { return emailAddressesToIgnore; }
        set { emailAddressesToIgnore = value; }
    }
    public List<string> PathToTransferReceipts
    {
        get { return pathToTransferReceipts; }
        set { pathToTransferReceipts = value; }
    }
    public List<string> ResponseLongMessageType
    {
        get { return responseLongMessageType; }
        set { responseLongMessageType = value; }
    }
    public List<string> ResponseShortMessageType
    {
        get { return responseShortMessageType; }
        set { responseShortMessageType = value; }
    }
    public List<string> ResponseOpeningAtmCard
    {
        get { return responseOpeningAtmCard; }
        set { responseOpeningAtmCard = value; }
    }
    public List<string> ResponseOpeningBlankEmailWithAttachment
    {
        get { return responseOpeningBlankEmailWithAttachment; }
        set { responseOpeningBlankEmailWithAttachment = value; }
    }
    public List<string> ResponseOpeningConsignmentBox
    {
        get { return responseOpeningConsignmentBox; }
        set { responseOpeningConsignmentBox = value; }
    }
    public List<string> ResponseOpeningDeathOrDying
    {
        get { return responseOpeningDeathOrDying; }
        set { responseOpeningDeathOrDying = value; }
    }
    public List<string> ResponseOpeningGenericPayment
    {
        get { return responseOpeningGenericPayment; }
        set { responseOpeningGenericPayment = value; }
    }
    public List<string> ResponseOpeningIlluminati
    {
        get { return responseOpeningIlluminati; }
        set { responseOpeningIlluminati = value; }
    }
    public List<string> ResponseOpeningInvestor
    {
        get { return responseOpeningInvestor; }
        set { responseOpeningInvestor = value; }
    }
    public List<string> ResponseOpeningLoanOffer
    {
        get { return responseOpeningLoanOffer; }
        set { responseOpeningLoanOffer = value; }
    }
    public List<string> ResponseOpeningLottery
    {
        get { return responseOpeningLottery; }
        set { responseOpeningLottery = value; }
    }
    public List<string> ResponseOpeningMoneyHack
    {
        get { return responseOpeningMoneyHack; }
        set { responseOpeningMoneyHack = value; }
    }
    public List<string> ResponseOpeningMoneyStorage
    {
        get { return responseOpeningMoneyStorage; }
        set { responseOpeningMoneyStorage = value; }
    }
    public List<string> ResponseOpeningOilAndGasQuestionList
    {
        get { return responseOpeningOilAndGasQuestionList; }
        set { responseOpeningOilAndGasQuestionList = value; }
    }
    public List<string> ResponseOpeningOilAndGas
    {
        get { return responseOpeningOilAndGas; }
        set { responseOpeningOilAndGas = value; }
    }
    public List<string> ResponseOpeningPolice
    {
        get { return responseOpeningPolice; }
        set { responseOpeningPolice = value; }
    }
    public List<string> ResponseContinuedAtmCard
    {
        get { return responseContinuedAtmCard; }
        set { responseContinuedAtmCard = value; }
    }
    public List<string> ResponseContinuedBlankEmailWithAttachment
    {
        get { return responseContinuedBlankEmailWithAttachment; }
        set { responseContinuedBlankEmailWithAttachment = value; }
    }
    public List<string> ResponseContinuedConsignmentBox
    {
        get { return responseContinuedConsignmentBox; }
        set { responseContinuedConsignmentBox = value; }
    }
    public List<string> ResponseContinuedDeathOrDying
    {
        get { return responseContinuedDeathOrDying; }
        set { responseContinuedDeathOrDying = value; }
    }
    public List<string> ResponseContinuedGenericPayment
    {
        get { return responseContinuedGenericPayment; }
        set { responseContinuedGenericPayment = value; }
    }
    public List<string> ResponseContinuedIlluminati
    {
        get { return responseContinuedIlluminati; }
        set { responseContinuedIlluminati = value; }
    }
    public List<string> ResponseContinuedInvestor
    {
        get { return responseContinuedInvestor; }
        set { responseContinuedInvestor = value; }
    }
    public List<string> ResponseContinuedLoanOffer
    {
        get { return responseContinuedLoanOffer; }
        set { responseContinuedLoanOffer = value; }
    }
    public List<string> ResponseContinuedLottery
    {
        get { return responseContinuedLottery; }
        set { responseContinuedLottery = value; }
    }
    public List<string> ResponseContinuedMoneyHack
    {
        get { return responseContinuedMoneyHack; }
        set { responseContinuedMoneyHack = value; }
    }
    public List<string> ResponseContinuedMoneyStorage
    {
        get { return responseContinuedMoneyStorage; }
        set { responseContinuedMoneyStorage = value; }
    }
    public List<string> ResponseContinuedOilAndGas
    {
        get { return responseContinuedOilAndGas; }
        set { responseContinuedOilAndGas = value; }
    }
    public List<string> ResponseContinuedPolice
    {
        get { return responseContinuedPolice; }
        set { responseContinuedPolice = value; }
    }
    public List<string> ResponseContinuedBeneficiary
    {
        get { return responseContinuedBeneficiary; }
        set { responseContinuedBeneficiary = value; }
    }
    public List<string> ResponseContinuedBuildTrust
    {
        get { return responseContinuedBuildTrust; }
        set { responseContinuedBuildTrust = value; }
    }
    public List<string> ResponseContinuedInheritance
    {
        get { return responseContinuedInheritance; }
        set { responseContinuedInheritance = value; }
    }
    public List<string> ResponseOpeningJobOffer
    {
        get { return responseOpeningJobOffer; }
        set { responseOpeningJobOffer = value; }
    }
    public List<string> ResponseContinuedJobOffer
    {
        get { return responseContinuedJobOffer; }
        set { responseContinuedJobOffer = value; }
    }
    public List<string> ResponseContinuedSellingProducts
    {
        get { return responseContinuedSellingProducts; }
        set { responseContinuedSellingProducts = value; }
    }
    public List<string> ResponseOpeningSellingProducts
    {
        get { return responseOpeningSellingProducts; }
        set { responseOpeningSellingProducts = value; }
    }
    public List<string> ResponseOpeningFreeMoney
    {
        get { return responseOpeningFreeMoney; }
        set { responseOpeningFreeMoney = value; }
    }
    public List<string> ResponseContinuedFreeMoney
    {
        get { return responseContinuedFreeMoney; }
        set { responseContinuedFreeMoney = value; }
    }
    public List<string> ResponseOpeningInformationGathering
    {
        get { return responseOpeningInformationGathering; }
        set { responseOpeningInformationGathering = value; }
    }
    public List<string> ResponseContinuedInformationGathering
    {
        get { return responseContinuedInformationGathering; }
        set { responseContinuedInformationGathering = value; }
    }
    public List<string> ResponseOpeningPhishing
    {
        get { return responseOpeningPhishing; }
        set { responseOpeningPhishing = value; }
    }
    public List<string> ResponseContinuedPhishing
    {
        get { return responseContinuedPhishing; }
        set { responseContinuedPhishing = value; }
    }
    public List<string> ResponseOpeningScamVictim
    {
        get { return responseOpeningScamVictim; }
        set { responseOpeningScamVictim = value; }
    }
    public List<string> ResponseContinuedScamVictim
    {
        get { return responseContinuedScamVictim; }
        set { responseContinuedScamVictim = value; }
    }
    public List<string> ResponseContinuedForeignLanguage
    {
        get { return responseContinuedForeignLanguage; }
        set { responseContinuedForeignLanguage = value; }
    }
    public List<string> ResponseContinuedGenericAdvertisement
    {
        get { return responseContinuedGenericAdvertisement; }
        set { responseContinuedGenericAdvertisement = value; }
    }
    public List<string> ResponseOpeningForeignLanguage
    {
        get { return responseOpeningForeignLanguage; }
        set { responseOpeningForeignLanguage = value; }
    }
    public List<string> ResponseOpeningGenericAdvertisement
    {
        get { return responseOpeningGenericAdvertisement; }
        set { responseOpeningGenericAdvertisement = value; }
    }
    public List<string> ResponseOpeningShipping
    {
        get { return responseOpeningShipping; }
        set { responseOpeningShipping = value; }
    }
    public List<string> ResponseContinuedShipping
    {
        get { return responseContinuedShipping; }
        set { responseContinuedShipping = value; }
    }
    public List<string> ResponseOpeningRefugee
    {
        get { return responseOpeningRefugee; }
        set { responseOpeningRefugee = value; }
    }
    public List<string> ResponseContinuedRefugee
    {
        get { return responseContinuedRefugee; }
        set { responseContinuedRefugee = value; }
    }
    public List<string> ResponseOpeningAccountProblem
    {
        get { return responseOpeningAccountProblem; }
        set { responseOpeningAccountProblem = value; }
    }
    public List<string> ResponseContinuedAccountProblem
    {
        get { return responseContinuedAccountProblem; }
        set { responseContinuedAccountProblem = value; }
    }
    public List<string> ResponseOpeningProductSupplier
    {
        get { return responseOpeningProductSupplier; }
        set { responseOpeningProductSupplier = value; }
    }
    public List<string> ResponseContinuedProductSupplier
    {
        get { return responseContinuedProductSupplier; }
        set { responseContinuedProductSupplier = value; }
    }
    public List<string> ResponseOpeningClickTheLink
    {
        get { return responseOpeningClickTheLink; }
        set { responseOpeningClickTheLink = value; }
    }
    public List<string> ResponseContinuedClickTheLink
    {
        get { return responseContinuedClickTheLink; }
        set { responseContinuedClickTheLink = value; }
    }
    public List<string> QuestionsHowAreYou
    {
        get { return questionsHowAreYou; }
        set { questionsHowAreYou = value; }
    }
    public List<string> QuestionsJokingAround
    {
        get { return questionsJokingAround; }
        set { questionsJokingAround = value; }
    }
    public List<string> QuestionsNotAnswering
    {
        get { return questionsNotAnswering; }
        set { questionsNotAnswering = value; }
    }
    public List<string> QuestionsNotListening
    {
        get { return questionsNotListening; }
        set { questionsNotListening = value; }
    }
    public List<string> QuestionsNotUnderstanding
    {
        get { return questionsNotUnderstanding; }
        set { questionsNotUnderstanding = value; }
    }
    public List<string> QuestionsPermission
    {
        get { return questionsPermission; }
        set { questionsPermission = value; }
    }
    public List<string> QuestionsSpokenLanguage
    {
        get { return questionsSpokenLanguage; }
        set { questionsSpokenLanguage = value; }
    }
    public List<string> QuestionsTrust
    {
        get { return questionsTrust; }
        set { questionsTrust = value; }
    }
    public List<string> QuestionsAddress
    {
        get { return questionsAddress; }
        set { questionsAddress = value; }
    }
    public List<string> QuestionsID
    {
        get { return questionsID; }
        set { questionsID = value; }
    }
    public List<string> QuestionsAlreadyIncludedID
    {
        get { return questionsAlreadyIncludedID; }
        set { questionsAlreadyIncludedID = value; }
    }
    public List<string> QuestionsPhoneNumber
    {
        get { return questionsPhoneNumber; }
        set { questionsPhoneNumber = value; }
    }
    public List<string> QuestionsWhyNoAnswer
    {
        get { return questionsWhyNoAnswer; }
        set { questionsWhyNoAnswer = value; }
    }
    public List<string> QuestionsCannotOpenAttachment
    {
        get { return questionsCannotOpenAttachment; }
        set { questionsCannotOpenAttachment = value; }
    }
    public List<string> QuestionsBirthdate
    {
        get { return questionsBirthdate; }
        set { questionsBirthdate = value; }
    }
    public List<string> QuestionsBetterPhoto
    {
        get { return questionsBetterPhoto; }
        set { questionsBetterPhoto = value; }
    }
    public List<string> QuestionsCountry
    {
        get { return questionsCountry; }
        set { questionsCountry = value; }
    }
    public List<string> QuestionsGender
    {
        get { return questionsGender; }
        set { questionsGender = value; }
    }
    public List<string> QuestionsMaritalStatus
    {
        get { return questionsMaritalStatus; }
        set { questionsMaritalStatus = value; }
    }
    public List<string> QuestionsOccupation
    {
        get { return questionsOccupation; }
        set { questionsOccupation = value; }
    }
    public List<string> QuestionsName
    {
        get { return questionsName; }
        set { questionsName = value; }
    }
    public List<string> QuestionsEmail
    {
        get { return questionsEmail; }
        set { questionsEmail = value; }
    }
    public List<string> QuestionsPassword
    {
        get { return questionsPassword; }
        set { questionsPassword = value; }
    }
    public List<string> QuestionsWeAreCaught
    {
        get { return questionsWeAreCaught; }
        set { questionsWeAreCaught = value; }
    }
    public List<string> QuestionsProvideDetails
    {
        get { return questionsProvideDetails; }
        set { questionsProvideDetails = value; }
    }
    public List<string> QuestionsAreYouReady
    {
        get { return questionsAreYouReady; }
        set { questionsAreYouReady = value; }
    }
    public List<string> QuestionsChangeContactMethod
    {
        get { return questionsChangeContactMethod; }
        set { questionsChangeContactMethod = value; }
    }
    public List<string> QuestionsContactMeLater
    {
        get { return questionsContactMeLater; }
        set { questionsContactMeLater = value; }
    }
    public List<string> QuestionsAreYouOnboard
    {
        get { return questionsAreYouOnboard; }
        set { questionsAreYouOnboard = value; }
    }
    public List<string> QuestionsPayTheFee
    {
        get { return questionsPayTheFee; }
        set { questionsPayTheFee = value; }
    }
    public List<string> QuestionsTheyConfused
    {
        get { return questionsTheyConfused; }
        set { questionsTheyConfused = value; }
    }
    public List<string> QuestionsHowBigOfLoan
    {
        get { return questionsHowBigOfLoan; }
        set { questionsHowBigOfLoan = value; }
    }
    public List<string> QuestionsMustPayBefore
    {
        get { return questionsMustPayBefore; }
        set { questionsMustPayBefore = value; }
    }
    public List<string> QuestionsMeetUs
    {
        get { return questionsMeetUs; }
        set { questionsMeetUs = value; }
    }
    public List<string> QuestionsFillOutForm
    {
        get { return questionsFillOutForm; }
        set { questionsFillOutForm = value; }
    }
    public List<string> QuestionsGetBackToUs
    {
        get { return questionsGetBackToUs; }
        set { questionsGetBackToUs = value; }
    }
    public List<string> QuestionsNeedBankDetails
    {
        get { return questionsNeedBankDetails; }
        set { questionsNeedBankDetails = value; }
    }
    public List<string> QuestionsWhatTypeOfProof
    {
        get { return questionsWhatTypeOfProof; }
        set { questionsWhatTypeOfProof = value; }
    }
    public List<string> QuestionsHowDoYouWantFundsReleased
    {
        get { return questionsHowDoYouWantFundsReleased; }
        set { questionsHowDoYouWantFundsReleased = value; }
    }
    public List<string> QuestionsWeCantDoThat
    {
        get { return questionsWeCantDoThat; }
        set { questionsWeCantDoThat = value; }
    }
    public List<string> QuestionsContactTheBank
    {
        get { return questionsContactTheBank; }
        set { questionsContactTheBank = value; }
    }
    public List<string> QuestionsContactThePerson
    {
        get { return questionsContactThePerson; }
        set { questionsContactThePerson = value; }
    }
    public List<string> QuestionsAreYouMember
    {
        get { return questionsAreYouMember; }
        set { questionsAreYouMember = value; }
    }
    public List<string> QuestionsDidYouSeeOurMessage
    {
        get { return questionsDidYouSeeOurMessage; }
        set { questionsDidYouSeeOurMessage = value; }
    }
    public List<string> QuestionsInvalidAddress
    {
        get { return questionsInvalidAddress; }
        set { questionsInvalidAddress = value; }
    }
    public List<string> QuestionsTellUsWhatTheyAskedYouToDo
    {
        get { return questionsTellUsWhatTheyAskedYouToDo; }
        set { questionsTellUsWhatTheyAskedYouToDo = value; }
    }
    public List<string> QuestionsTellUsAboutYourself
    {
        get { return questionsTellUsAboutYourself; }
        set { questionsTellUsAboutYourself = value; }
    }
    public List<string> QuestionsAutomatedProgram
    {
        get { return questionsAutomatedProgram; }
        set { questionsAutomatedProgram = value; }
    }
    public List<string> QuestionsUseWalmartToPay
    {
        get { return questionsUseWalmartToPay; }
        set { questionsUseWalmartToPay = value; }
    }
    public List<string> QuestionsHowMuchMoneyDoIHave
    {
        get { return questionsHowMuchMoneyDoIHave; }
        set { questionsHowMuchMoneyDoIHave = value; }
    }
    public List<string> QuestionsSendTransferReceipt
    {
        get { return questionsSendTransferReceipt; }
        set { questionsSendTransferReceipt = value; }
    }
    public List<string> QuestionsAlreadyToldYou
    {
        get { return questionsAlreadyToldYou; }
        set { questionsAlreadyToldYou = value; }
    }
    public List<string> QuestionsTrustUs
    {
        get { return questionsTrustUs; }
        set { questionsTrustUs = value; }
    }

    public List<string> RandomThoughts
    {
        get { return randomThoughts; }
        set { randomThoughts = value; }
    }
    public List<string> ConsignmentBoxImageIncluded
    {
        get { return consignmentBoxImageIncluded; }
        set { consignmentBoxImageIncluded = value; }
    }
    public List<string> ConsignmentBoxImageNotIncluded
    {
        get { return consignmentBoxImageNotIncluded; }
        set { consignmentBoxImageNotIncluded = value; }
    }
    #endregion

    public Settings()
    {
    }

    public static bool ParseBooleanSetting(string str)
    {
        bool rtn = false;

        if (str.ToUpper().Trim() == "T" || str.ToUpper().Trim() == "TRUE" || str.ToUpper().Trim() == "Y" || str.ToUpper().Trim() == "YES")
            rtn = true;

        return rtn;
    }
}