using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckInformationGathering : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckInformationGathering(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.InformationGathering;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("AM NOW A SINGER") ||
            preProcessedBody.Trim().ToUpper().Contains("ANY PROBLEM IN LIFE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU STILL INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("BEEN TRYING TO REACH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL I DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL ME") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I ASK YOU A FAVOR") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN WE TALK") ||
            preProcessedBody.Trim().ToUpper().Contains("CHARITY PROPOSAL FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("COCA-COLA AWARD") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIDENTIAL BRIEF") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT ME") ||
            preProcessedBody.Trim().ToUpper().Contains("DEAR HOW ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY LAST EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY LAST E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET MY PREVIOUS E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE MY PREVIOUS E-MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS A IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS A ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS A VERY IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS AN IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS AN ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS AN VERY IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DISCUSS IMPORTANT ISSUE") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU SPEAK ENGLISH") ||
            preProcessedBody.Trim().ToUpper().Contains("FOR MORE DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("FROM NOW AND ONWARD") ||
            preProcessedBody.Trim().ToUpper().Contains("FROM NOW ONWARD") ||
            preProcessedBody.Trim().ToUpper().Contains("GET BACK TO ME FOR MORE INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("GET BACK TO US FOR MORE INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("GIVE ME A CALL") ||
            preProcessedBody.Trim().ToUpper().Contains("GIVE ME CALL") ||
            preProcessedBody.Trim().ToUpper().Contains("HELLO DEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP ME") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP NEEDED") ||
            preProcessedBody.Trim().ToUpper().Contains("HELP POWER PROGRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("HEY DEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("HI DEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU DOING") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU, I AM VERY GOOD") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW OLD ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HUMANITARIAN CHARITY OFFER") ||
            preProcessedBody.Trim().ToUpper().Contains("HW ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM HAVING A MEETING WITH MY CLIENT BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("I CAN GIVE YOU MORE DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE A VERY LUCRATIVE DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE SPECIAL PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE WAITED FOR YOU SO LONG") ||
            preProcessedBody.Trim().ToUpper().Contains("I LIKE US TO TALK") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED YOU URGENTLY") ||
            preProcessedBody.Trim().ToUpper().Contains("I REALLY NEED TO HEAR FROM YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL GIVE YOU DETAILS ON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL GIVE YOU DETAILS UPON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL PROVIDE YOU WITH DETAILS ON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I SHALL PROVIDE YOU WITH DETAILS UPON YOUR RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("I URGENTLY NEED YOUR ASSISTANCE") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO MEET YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("IF WE CAN MAKE A MUTUAL TRANS") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT I LIKE TO SHARE") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT INFORMATION FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("IMPORTANT TO DISCUSS WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("IN ORDER TO UPDATE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INORDER TO UPDATE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO ON WHY THE EMAIL IS COMING FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO ON WHY THE EMAIL IS COMING TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO ON WHY THIS EMAIL IS COMING TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFO WHY THIS EMAIL IS COMING TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("INFORMATION WE WISH TO SHARE") ||
            preProcessedBody.Trim().ToUpper().Contains("KNOW IF YOU RECEIVED MY PREVIOUS MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("MEET PEOPLE FOR DIFFERENT REASONS RELATIONSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("MISS SHARON RIVAS") ||
            preProcessedBody.Trim().ToUpper().Contains("MUSICAL ARTIST NAME IS SENATOR") ||
            preProcessedBody.Trim().ToUpper().Contains("LONG TIME I HEAR FROM YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED TO TALK ITS VERY IMPORTANT") ||
            preProcessedBody.Trim().ToUpper().Contains("NO TIME TO WASTE") ||
            preProcessedBody.Trim().ToUpper().Contains("ORDERS FROM MR. PRESIDENT") ||
            preProcessedBody.Trim().ToUpper().Contains("PERMISSION TO EMAIL YOU MY PROPOSAL") ||
            preProcessedBody.Trim().ToUpper().Contains("PERSONAL DISCUSSION") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE CONTACT MY SON") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE HELP ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE WRITE ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE I WANT YOU TO ASSIST") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE I WANT YOUR ASSIST") ||
            preProcessedBody.Trim().ToUpper().Contains("RANDOMLY SELECTED INDIVID") ||
            preProcessedBody.Trim().ToUpper().Contains("REALLY SURPRISE READING YOUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("REPLY FOR DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("RESPOND TO MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME CATALOG") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR CATALOG") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOU MY PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("SOME THING IMPORTANT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMETHING IMPORTANT FOR YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("SOMETHING URGENT") ||
            preProcessedBody.Trim().ToUpper().Contains("SORRY TO INFORM U MY NEW") ||
            preProcessedBody.Trim().ToUpper().Contains("SORRY TO INFORM YOU MY NEW") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("TELL HER TO SEND YOU THE MEMBERSHIP FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("THE EMAIL ADDRESS YOU USED TO CONTACT US IS NO LONGER VALID") ||
            preProcessedBody.Trim().ToUpper().Contains("THAT WAS HILARIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("THATS WAS HILARIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS PURE BUSINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS TO INFORM YOU THAT YOU HAVE BEEN PICKED") ||
            preProcessedBody.Trim().ToUpper().Contains("UPDATED MY CONTACT INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("VERY IMPORTANT THING TO TELL YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WE HAVE GOT SOME INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU REALLY SAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR AGE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE ARE YOU FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE A GOOD PROFILE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE GOOD PROFILE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE URGENT CALL") ||
            (preProcessedBody.Trim().ToUpper().Contains("SEND AN EMAIL") && preProcessedBody.Trim().ToUpper().Contains("FOR MORE INFO")) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOOD MORNING AND HOW ARE YOU") && preProcessedBody.Trim().ToUpper().Contains("MY NAME IS") && (preProcessedBody.Trim().Length - currentMessage.SubjectLine.Trim().Length) < 100) ||
            (preProcessedBody.Trim().ToUpper().Contains("GOOD NEW") && (preProcessedBody.Trim().Length - currentMessage.SubjectLine.Trim().Length) <= 20) ||
            (preProcessedBody.Trim().ToUpper().Contains("OK") && (preProcessedBody.Trim().Length - currentMessage.SubjectLine.Trim().Length) <= 10) ||
            ((preProcessedBody.Trim().ToUpper().Contains("HI") || preProcessedBody.Trim().ToUpper().Contains("HELLO") || preProcessedBody.Trim().ToUpper().Contains("GREETING") || preProcessedBody.Trim().ToUpper().Contains("DEAR FRIEND")) && (preProcessedBody.Length - currentMessage.SubjectLine.Length) <= 10))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}