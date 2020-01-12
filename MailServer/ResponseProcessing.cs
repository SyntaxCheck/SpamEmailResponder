using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ResponseProcessing
{
    private Settings settings;
    private ResponseSettings responseSettings;
    private List<EmailTypeBase> EmailTypeParseLit;
    public string MyName;
    public bool IsOverridAdmin { get; set; }

    public enum EmailType
    {
        Unknown = 0,
        Test = 1,
        BlankWithAttachment = 2,
        Inheritance = 3,
        Lottery = 4,
        OilAndGas = 5,
        Illuminati = 6,
        ConsignmentBox = 7,
        Beneficiary = 8,
        DeathOrDying = 9,
        LoanOffer = 10,
        MoneyStorage = 11,
        AtmCard = 12,
        Police = 13,
        GenericPayment = 14,
        SellingServices = 15,
        OnlineMarketingConsult = 16,
        BuildTrust = 17,
        Investor = 18,
        MoneyHack = 19,
        JobOffer = 20,
        SellingProducts = 21,
        FreeMoney = 22,
        InformationGathering = 23,
        Phishing = 24,
        ScamVictim = 25,
        ForeignLanguage = 26,
        GenericAdvertisement = 27,
        MessageTooLong = 28,
        MessageTooShort = 29,
        Shipping = 30,
        Refugee = 31,
        AccountProblem = 32,
        ProductSupplier = 33
    };

    public ResponseProcessing(Settings settings)
    {
        this.settings = settings;

        MyName = settings.MyName;

        string overrideAdminfullPath = Path.Combine(Directory.GetCurrentDirectory(), StaticVariables.ADMIN_FILENAME);
        if (File.Exists(overrideAdminfullPath))
        {
            IsOverridAdmin = true;
        }
        else
        {
            IsOverridAdmin = false;
        }

        responseSettings = new ResponseSettings() { IsAdmin = false };
    }

    private string HandleDirectQuestions(string body, ref MailStorage currentMessage, Random rand)
    {
        string response = String.Empty;
        string preProcessedBody = body.Replace("\r\n", " ").Replace("---", "...").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
        bool alreadyRepliedNotAnswering = false;
        bool askedForDetails = false;

        if (preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU?") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU DOING") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU THIS MORNING") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU THIS AFTER") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU THIS EVEN") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU THIS NIGHT") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU THIS DAY") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU TODAY") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW DO YOU DO") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW YOU DOING") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU OVER THERE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE ALL IS WELL WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU ARE VERY FINE TODAY") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW ARE YOU AND YOUR FAMILY TODAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT YOU ARE UP TO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE DOING WONDERFULLY WELL TODAY") ||
            preProcessedBody.Trim().ToUpper().Contains("IM DOING GOOD AND YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I HOPE YOU ARE GOOD") ||
            preProcessedBody.Trim().ToUpper().Contains("TOP OF THE DAY TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE A NICE DAY BROTHER"))
        {
            response += GetRandomQuestionsHowAreYou(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR 419 FORMAT IS TOO MUCH") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR A SCAM BAITER") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING USELESS MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE VERY STUPID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A MAD MAN") ||
            preProcessedBody.Trim().ToUpper().Contains("SEEMS YOU JUST CAME BACK FROM A PSYCHIATRIC") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP CONTACTING ME PLEASE") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL GET YOU ARREST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A BIG FOOL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A FOOLISH") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING GARBAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR A SCAM BETER"))
        {
            response += GetRandomQuestionsWeAreCaught(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("INDEED YOU ARE A HUMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED TO KNOW YOU ARE HUMAN") ||
            preProcessedBody.Trim().ToUpper().Contains("QUIT ACTING BOT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A BOT") ||
            preProcessedBody.Trim().ToUpper().Contains("QUIT ACTING ROBOT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A ROBOT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU A ROBOT") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU A ROBOT") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU ROBOT") ||
            preProcessedBody.Trim().ToUpper().Contains("AUTOMATED PROGRAM SENDING") ||
            preProcessedBody.Trim().ToUpper().Contains("IS THE MESSAGE AUTOMATE") ||
            preProcessedBody.Trim().ToUpper().Contains("AUTOMATIC MESSAGE YOU ALWAYS") ||
            preProcessedBody.Trim().ToUpper().Contains("AUTOMATED REPLY PROGRAM"))
        {
            response += GetRandomQuestionsAutomatedProgram(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ALL KIND OF STUPID QUESTION") ||
            preProcessedBody.Trim().ToUpper().Contains("ANY KIND OF JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU A CLOWN") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU HERE FOR JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU JOKING") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU MAKING A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU MAKING FUN") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU MAKING JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU PLAYING WITH US") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU SURE YOU ARE MENTALLY") ||
            preProcessedBody.Trim().ToUpper().Contains("BE SERIOUS IF YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CANNOT AFFORD TO JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("DO NOT CONTACT ME") ||
            preProcessedBody.Trim().ToUpper().Contains("DO NOT WRITE ANY FURTHER") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU TAKE US FOR A FOOL") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU THINK THIS IS JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU THINK YOU ARE FUNNY") ||
            preProcessedBody.Trim().ToUpper().Contains("DONT EMAIL TO ME AGAIN") ||
            preProcessedBody.Trim().ToUpper().Contains("DONT WRITE ME AGAIN") ||
            preProcessedBody.Trim().ToUpper().Contains("FINAL WARNING") ||
            preProcessedBody.Trim().ToUpper().Contains("FUCK YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("FUCK YOUR FAMILY") ||
            preProcessedBody.Trim().ToUpper().Contains("FUK YOUR FAMILY") ||
            preProcessedBody.Trim().ToUpper().Contains("GETTING ANGRY OR ANNOYED") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE GOTTEN SOMEONE THAT KNOWS THE VALUE OF MY MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU ARE NOT A SERIOUS TYPE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU ARE NOT PLAYING GAME") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM NOT HERE TO DO FAKE BUSINES") ||
            preProcessedBody.Trim().ToUpper().Contains("I DO NOT KNOW IF YOU ARE A JOKER") ||
            preProcessedBody.Trim().ToUpper().Contains("I DONT KNOW IF YOU ARE A JOKER") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE NO TIME TO WASTE") ||
            preProcessedBody.Trim().ToUpper().Contains("I PERSONALLY DO NOT HAVE TIME FOR") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL GET YOU ARREST") ||
            preProcessedBody.Trim().ToUpper().Contains("IF YOU KNOW YOU ARE NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("IF YOU WRITE SUCH EMAIL TO US AGAIN, YOUR ACCOUNT WILL BE SUSPEND") ||
            preProcessedBody.Trim().ToUpper().Contains("IF YOUR SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("IS THIS A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("IT SEEMS YOU ARE DRUNK") ||
            preProcessedBody.Trim().ToUpper().Contains("IT SEEMS YOURE DRUNK") ||
            preProcessedBody.Trim().ToUpper().Contains("JOKER PLEASE DONT") ||
            preProcessedBody.Trim().ToUpper().Contains("JOKE WITH") ||
            preProcessedBody.Trim().ToUpper().Contains("JOKES WITH") ||
            preProcessedBody.Trim().ToUpper().Contains("JOKER WITH") ||
            preProcessedBody.Trim().ToUpper().Contains("JOKING WITH") ||
            preProcessedBody.Trim().ToUpper().Contains("KIND OF GAME PLAY") ||
            preProcessedBody.Trim().ToUpper().Contains("KIND OF PLAY GAME") ||
            preProcessedBody.Trim().ToUpper().Contains("PLAY WITH EVERYBODY") ||
            preProcessedBody.Trim().ToUpper().Contains("PLAY WITH EVERYONE") ||
            preProcessedBody.Trim().ToUpper().Contains("POOR BRAT") ||
            preProcessedBody.Trim().ToUpper().Contains("MAY YOUR FATHER DIE") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT HERE TO BE PLAYING ALL THE TIME") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT HERE TO BE PLAYING GAMES") ||
            preProcessedBody.Trim().ToUpper().Contains("SERIOUS ONES SCALE THROUGH") ||
            preProcessedBody.Trim().ToUpper().Contains("SOUND LIKE A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP ACTING FUNNY") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SCAMING YOURSELF") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SCAMMING YOURSELF") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING MAILS THAT DO NOT HAVE ANY MEANING") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING MAILS THAT DO NOT HAVE MEANING") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING ME AN EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING ME EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP SENDING THIS NONSENSE") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP THIS YOUR MADNESS SPEECH") ||
            preProcessedBody.Trim().ToUpper().Contains("STOP WRITING NONSENCE") ||
            preProcessedBody.Trim().ToUpper().Contains("TALKING SHIT") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS A SERIOUS TRANSACTION AND NOT A CHILDS PLAY") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS NOT A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS NOT CHILD PLAY") ||
            preProcessedBody.Trim().ToUpper().Contains("TO JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("USELEE FOOL FUCK YOU OFF") ||
            preProcessedBody.Trim().ToUpper().Contains("USELESS FOOL FUCK OFF") ||
            preProcessedBody.Trim().ToUpper().Contains("USELESS FOOL FUCK YOU OFF") ||
            preProcessedBody.Trim().ToUpper().Contains("WARN YOU TO STOP CONTACTING ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WASTING MY TIME ANYMORE") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE NOT HERE FOR PLAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE NOT HERE TO PLAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE NOT JOKING") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE NOT PLAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WILL PUNISH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT EXACTLY IS YOU PROBLEM") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS WRONG WITH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT YOUR PROBLEM IS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A DRUG ADDICT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A FUCHING JOKER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE A FUCKING JOKER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE FUNNY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE FRUSTRATING") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE NEVER READY OR SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE NOT IN THIS PLANET") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE JUST A JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE JOKE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE MAD MAN") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE SO NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE SERIOUS ABOUT THIS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE VERY STUPID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU DO NOT LOOK SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE TIME FOR RUBBISH I DONT HAVE YOUR TIME MY TIME IS MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU MUST BE A CRAZY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SEEM NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SEEMS NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SOUND LIKE A JOKER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SHOULD STOP VISITING MY BOX") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU TALKS OUT OF SENSE AND POINT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOURE NOT SERIOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ACCOUNT HAS ALREADY BEEN SUSPEND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ARE NOT A SERIOUS PERSON"))
        {
            response += GetRandomQuestionsJokingAround(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("LETS TALK ON THE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("LETS TALK OVER TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DONT FORGET TO TEXT ME") ||
            preProcessedBody.Trim().ToUpper().Contains("DON,T FORGET TO TEXT ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE IF POSSIBLE CALL ME") ||
            preProcessedBody.Trim().ToUpper().Contains("TEXT ME YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("TEXT THROUGH MY MOBILE") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK NEXT VIA PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK WITH YOU ON THE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVERSATION WITH YOU ON PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("CONVERSATION WITH YOU ON THE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR WHATSAPP NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE CALLED") ||
            preProcessedBody.Trim().ToUpper().Contains("TO CALL YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("LETS DEAL ON PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("KINDLY TEXT OR CALL US") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU CAN TEXT ME ON MY PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("GMAIL HANGOUT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME SMS TEXT") ||
            preProcessedBody.Trim().ToUpper().Contains("SO WE CAN TALK BETTER") ||
            preProcessedBody.Trim().ToUpper().Contains("COME OVER IN FACE BOOK"))
        {
            response += GetRandomQuestionsChangeContactMethod(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ARE YOU READY?") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU READY TO WORK") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU READY TO PAY THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I HAVE A WORD") ||
            preProcessedBody.Trim().ToUpper().Contains("LET ME KNOW YOUR READINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR READINESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR DECISION FOR YOUR MONEY RECEPTION") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU READY TO MAKE THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SHOULD INFORM ME") ||
            preProcessedBody.Trim().ToUpper().Contains("NOW ARE YOU READY") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU READY TO RECEIVE YOUR TRANSFER"))
        {
            response += GetRandomQuestionsAreYouReady(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("I WILL WRITE MORE TOMORROW") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL MESSAGE MORE TOMORROW") ||
            preProcessedBody.Trim().ToUpper().Contains("WILL GET BACK TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I WILL WRITE MORE LATER"))
        {
            response += GetRandomQuestionsContactMeLater(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ARE YOU HELPING ME") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU WILLING") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU NO LONGER INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU HANDLE THE DEAL") ||
            preProcessedBody.Trim().ToUpper().Contains("INTEREST TO WORK WITH ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WILLINGNESS TO WORK WITH ME") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU INTERESTED TO RECEIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("NOTIFY US IF YOU ARE NOT INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU REALLY INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE NOT INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("IF YOU ARE WILLING") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN WE DO BUSINES") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU STILL WILLING") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE STILL INTERESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU WANT TO PROCEED"))
        {
            response += GetRandomQuestionsAreYouOnboard(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ACTIVATION FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("BECAUSE OF FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("BECAUSE OF THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("BUY ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPLETE THE TAX PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM THE REMAIN FES") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM THE REMAINING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY FEE OF") ||
            preProcessedBody.Trim().ToUpper().Contains("FAST ABOUT GETTING THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("FEE FOR THE SIGN IS FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("FEE YOU HAVE TO PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("GET A ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("GET AN ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("GET ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("GO AHEAD AND MAKE THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("GO AHEAD AND PAY THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("GO AHEAD AND SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("GO AND PAY THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("GO AND SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("HAS THE MONEY BEEN SENT") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE TO SEND $") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE TO SEND USD") ||
            preProcessedBody.Trim().ToUpper().Contains("IF THE FEE OF") ||
            preProcessedBody.Trim().ToUpper().Contains("IF THE FEES OF") ||
            preProcessedBody.Trim().ToUpper().Contains("IM WAITING FOR YOUR PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("MAKE THE DELIVERY FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("MENTIONED FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("MENTION FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("NEEDED REQUIRED $") ||
            preProcessedBody.Trim().ToUpper().Contains("ONLY THING DELAYING NOW IS THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("PAY FOR THE CHARGES") ||
            preProcessedBody.Trim().ToUpper().Contains("PAY THE DELIVERY FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("PAY THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("PAY THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("PURCHASE ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("PURCHASE A ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("PURCHASE AN ITUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("READY TO PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND HER THE $") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND HIM THE $") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND IT THROUGH MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND IT THROUGH WESTERN") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND REQUESTED FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND SHIPING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND SHIPPING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE $") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE ACCOUNT OPENING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE CHARGE TO US") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE CLEARANCE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE DELIVERY FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE FEE SO WE CAN PROCEED") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE NEEDED FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE REMAINING") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE REQUESTED FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE REQUIRED FEE") |
            preProcessedBody.Trim().ToUpper().Contains("SEND THE SHIPING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE SHIPPING FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THIS CASH") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THIS MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THIS FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND TO MAKE THE TOTAL OF THE AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND TO US THE COURIER SERVICE CHARGE") ||
            preProcessedBody.Trim().ToUpper().Contains("SENDING THE DELIVERY CHARGE") ||
            preProcessedBody.Trim().ToUpper().Contains("SENDING THE DELIVERY FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("SENDING THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SENT THE CLEARANCE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("TAKE CARE OF THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("THE COURIER FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("TO PAY THE $") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ONLY NEED THE FEE FROM YOU NOW") ||
            preProcessedBody.Trim().ToUpper().Contains("WHEN ARE YOU MAKING THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHEN YOU ARE SENDING THE REQUIRED AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE ADVISED THE REQUESTED FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE TO SEND THE FEE NOW") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE SENDING THE NEEDED AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU CAN MAKE USE OF MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE TO PAY FOR OWNERSHIP") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU NEED TO SEND HIM SOME MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU NEED TO SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU PAY $") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU TO SEND THE $") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU TO SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU TO SEND THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU TO SEND THE US") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU WILL SEND THE FEE THROUGH") ||
            (preProcessedBody.Trim().ToUpper().Contains("THE FEE") && preProcessedBody.Trim().ToUpper().Contains("HOLDING YOUR FUND")) ||
            (preProcessedBody.Trim().ToUpper().Contains("BUY") && preProcessedBody.Trim().ToUpper().Contains("GIFT CARD")) ||
            (preProcessedBody.Trim().ToUpper().Contains("JUST MAKE THE") && preProcessedBody.Trim().ToUpper().Contains("DELIVERY PAYMENT")))
        {
            response += GetRandomQuestionsPayTheFee(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ARE YOU TALKING ABOUT") ||
            preProcessedBody.Trim().ToUpper().Contains("I AM CONFUSED") ||
            preProcessedBody.Trim().ToUpper().Contains("DONT SEEM TO UNDERSTAND") ||
            preProcessedBody.Trim().ToUpper().Contains("DONT SEEMS TO UNDERSTAND") ||
            preProcessedBody.Trim().ToUpper().Contains("BUT CAN NOT COMPREHEND THE CONTENT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU SAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT DO YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I DID NOT UNDERSTAND YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I DID NOT UNDERSTAND WHAT YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I DID NOT UNDERSTAND WHAT YOU ARE") ||
            preProcessedBody.Trim().ToUpper().Contains("I DONT UNDERSTAND YOUR") ||
            preProcessedBody.Trim().ToUpper().Contains("I DONT UNDERSTAND WHAT YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I DONT UNDERSTAND WHAT YOU ARE") ||
            preProcessedBody.Trim().ToUpper().Contains("I DO NOT UNDERSTAND YOUR") ||
            preProcessedBody.Trim().ToUpper().Contains("I DO NOT UNDERSTAND WHAT YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I DO NOT UNDERSTAND WHAT YOU ARE") ||
            preProcessedBody.Trim().ToUpper().Contains("I REALLY DO NOT UNDERSTAND YOUR") ||
            preProcessedBody.Trim().ToUpper().Contains("I REALLY DO NOT UNDERSTAND WHAT YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("I REALLY DO NOT UNDERSTAND WHAT YOU ARE") ||
            preProcessedBody.Trim().ToUpper().Contains("I CAN NOT UNDERSTAND YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU SAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU JUST SAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU TALKING ABOUT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS GOING ON") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW DO YOU MEAN") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT EVEN A WORD I CAN PICK") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU EXPLAIN WHAT THIS MESSAGE MEANS") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU EXPLAIN WHAT THIS MEANS") ||
            preProcessedBody.Trim().ToUpper().Contains("?????") ||
            preProcessedBody.Trim().ToUpper().Contains("I DONT UNDERSTAND WHAT YOU ARE SAYING"))
        {
            response += GetRandomQuestionsTheyConfused(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("AS SOON AS YOU MAKE THE PAYMENT WE WILL SEND YOU THE PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("AS SOON AS YOU MAKE THE PAYMENT WE WILL SEND THE PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("WE WILL SEND THE PICTURE WHEN THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE WILL SEND THE PICTURE WHEN YOU MAKE THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("THE PICTURE WILL BE SENT WHEN YOU MAKE THE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU NEED TO PROVIDE THE INFORMATION FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU MUST PROVIDE THE INFORMATION FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU NEED TO PROVIDE FULL DETAILS FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU MUST PROVIDE FULL DETAILS FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU NEED TO PROVIDE THE DETAILS FIRST") ||
            preProcessedBody.Trim().ToUpper().Contains("ONCE YOU SEND THE FEE WE WILL DELIVER IT") ||
            preProcessedBody.Trim().ToUpper().Contains("WITHOUT SEND THE FEE YOU CANNOT") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT ALLOWED NOW TO SHOW YOU THE IMAGE AT THIS MOMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT ALLOWED TO SHOW YOU THE IMAGE AT THIS MOMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR PERSONAL INFORMATION FIRST BEFORE") ||
            preProcessedBody.Trim().ToUpper().Contains("AFTER YOUR PAYMENT IS CONFIRMED") ||
            (preProcessedBody.Trim().ToUpper().Contains("IF YOU DONT WANT TO SEND THE") && preProcessedBody.Trim().ToUpper().Contains("USD")) ||
            (preProcessedBody.Trim().ToUpper().Contains("IF YOU DONT WANT TO SEND THE") && preProcessedBody.Trim().ToUpper().Contains("FEE")) ||
            (preProcessedBody.Trim().ToUpper().Contains("IF YOU DONT WANT TO SEND THE") && preProcessedBody.Trim().ToUpper().Contains("PAYMENT")) ||
            preProcessedBody.Trim().ToUpper().Contains("YOU MUST PROVIDE THE DETAILS FIRST"))
        {
            response += GetRandomQuestionsMustPayBefore(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("POSSIBLE THAT YOU COME MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU BE ABLE TO COME MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("COULD YOU MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU BE WILLING TO COME MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU HAVE TIME TO COME MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("POSSIBLE THAT YOU MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU BE ABLE TO MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU BE WILLING TO MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("APPEARING HERE IN PERSON") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE TO COME DOWN TO NIGERIA") ||
            preProcessedBody.Trim().ToUpper().Contains("COME DOWN HERE IN BENIN REPUBLIC") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU WANT TO COME TO MY HOUSE") ||
            preProcessedBody.Trim().ToUpper().Contains("WANT TO COME TO DUBAI") ||
            preProcessedBody.Trim().ToUpper().Contains("WHEN ARE YOU COMING") ||
            preProcessedBody.Trim().ToUpper().Contains("INTEND TO MEET US") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR FLIGHT DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("MEET YOU UP THERE") ||
            preProcessedBody.Trim().ToUpper().Contains("WOULD YOU HAVE TIME TO MEET US"))
        {
            response += GetRandomQuestionsMeetUs(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("I HAVE ATTACHED A FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("DOCUMENT AND RETURN BACK TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL OUT THE ATTACHED FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL OUT THE INCLUDED FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL OUT THE ATTACHED DOCUMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL OUT THE FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("HERE IS THE APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE INCLUDED THE APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE THE ATTACHED APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("OFFICIAL APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("THE APPLICATION FORM IS ATTACHED") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL IN THE APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL IN THE DOCUMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL THE LOAN APPLICATION FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("ENDORSE ATTACHED COPY") ||
            preProcessedBody.Trim().ToUpper().Contains("SIGN THE FORM AS REQUESTED") ||
            preProcessedBody.Trim().ToUpper().Contains("IT IS REQUESTED OF YOU TO FILL OUT AND SIGN") ||
            preProcessedBody.Trim().ToUpper().Contains("OUR OFFICIAL FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("SCAN BACK THE FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("SCAN BACK FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("RETURN BACK THE FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("FILL THE IMF FORM") ||
            preProcessedBody.Trim().ToUpper().Contains("THE APPLICATION FORM IS INCLUDED"))
        {
            response += GetRandomQuestionsFillOutForm(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("GET BACK TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU UPDATE ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WAITING TO HEAR FROM YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("UPDATE ME BACK") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ME BACK") ||
            preProcessedBody.Trim().ToUpper().Contains("GET BACK TOME") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE YOU RECEIVED THE") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE YOU GOTTEN THE") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE YOU HEARD FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE CONFIRM") ||
            preProcessedBody.Trim().ToUpper().Contains("PLEASE LET ME KNOW") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE YOU SEEN THE") ||
            preProcessedBody.Trim().ToUpper().Contains("REPLY ME BACK") ||
            preProcessedBody.Trim().ToUpper().Contains("REPLY US BACK") ||
            preProcessedBody.Trim().ToUpper().Contains("NOTIFY US AS SOON AS YOU HEAR FROM"))
        {
            response += GetRandomQuestionsGetBackToUs(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR BANK DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR BANKING DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR BANK INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR BANKING INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL BANK DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL BANKING DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR BANK ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR BANK ACCOUNT"))
        {
            response += GetRandomQuestionsNeedBankDetails(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DO YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DO YOU WANT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DO YOU SEEK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DO YOU ASK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DID YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DID YOU WANT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DID YOU SEEK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF DID YOU ASK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT PROOF DO YOU ASK OF") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT KIND OF PROOF DO YOU ASK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT KIND OF PROOF DO YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT KIND OF PROOF DO YOU WANT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT KIND OF PROOF DO YOU SEEK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT KIND OF PROOF?") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT CAN OF PROOF DID YOU WANT") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT MORE CONVICTION DO YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT TYPE OF PROOF?"))
        {
            response += GetRandomQuestionsWhatTypeOfProof(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("HOW DO YOU WANT YOUR FUNDS TO BE RELEASED") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW DO YOU WANT US TO RELEASE THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW YOU WANT YOUR FUNDS PRESENTED") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW YOU WISH TO HAVE YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("OPTION YOU WISH TO RECEIVE THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW DO WE SEND THE FUNDS TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE OF THIS FUND") ||
            preProcessedBody.Replace(" ", "").Trim().ToUpper().Contains("ATMCARD,BANKTOBANKWIRETRANSFERORDIP") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW DO YOU WANT THE FUNDS RELEASED"))
        {
            response += GetRandomQuestionsHowDoYouWantFundsReleased(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WE CANNOT DO THAT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE CANT DO THAT") ||
            preProcessedBody.Trim().ToUpper().Contains("WE ARE UNABLE TO DO THAT") ||
            preProcessedBody.Trim().ToUpper().Contains("THAT IS NOT SOMETHING WE CAN DO") ||
            preProcessedBody.Trim().ToUpper().Contains("WE DONT TAKE PICTURES OF CUSTOMERS LUGGAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("IT IS NOT POSSIBLE FOR ME") ||
            preProcessedBody.Trim().ToUpper().Contains("ITS NOT POSSIBLE FOR ME") ||
            preProcessedBody.Trim().ToUpper().Contains("THE LOAN CANNOT BE PROCESSED") ||
            preProcessedBody.Trim().ToUpper().Contains("WE DONT TAKE PICTURES OF CUSTOMERS LUGGAGE"))
        {
            response += GetRandomQuestionsWeCantDoThat(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("BANK CONTACT INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT MY BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT OUR BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACTED THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("MESSAGE THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("MESSAGED THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT WITH THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS THE BANK EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("ABLE TO REACH THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("GO TO YOUR BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("TAKE IT TO YOUR BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("HERE FROM THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("HERE FROM BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("HEAR FROM THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("HEAR FROM BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("HEARD FROM THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("HEARD FROM BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS THE BANK WHATSAPP") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS THE BANK NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS THE BANK PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("THIS IS THE BANK CONTACT") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT HIM NOW") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT HER NOW") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT THEM NOW") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK WITH THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("TALK TO THE BANK"))
        {
            response += GetRandomQuestionsContactTheBank(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("CONTACT THE BANKER") ||
            preProcessedBody.Trim().ToUpper().Contains("CANTACT THE DIPLOMAT") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT THE LAWYER") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT THE PRIEST") ||
            preProcessedBody.Trim().ToUpper().Contains("STAGE ARE YOU AT WITH THE LAWYER") ||
            preProcessedBody.Trim().ToUpper().Contains("STAGE ARE YOU WITH THE LAWYER"))
        {
            response += GetRandomQuestionsContactThePerson(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ARE YOU A MEMBER OF OUR") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU MEMBER OF OUR") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU PART OF OUR CLUB") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU PART OF OUR GROUP") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU HAVE MEMBERSHIP"))
        {
            response += GetRandomQuestionsAreYouMember(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR LAST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR LAST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR LAST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED OUR LAST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR LAST TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR LAST TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR LAST TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED OUR TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED OUR LAST TEXT MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR LAST PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU GET OUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR LAST PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU SEE OUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE OUR LAST PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED OUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED OUR LAST PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE MY EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE MY LAST EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED MY EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED MY LAST EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE THE EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE THE LAST EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE THE PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED THE EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED THE LAST EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED THE PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE THE MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE THE LAST MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVE THE PREVIOUS MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED THE MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED THE LAST MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU RECEIVED THE PREVIOUS MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU GOT MY EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU GOT OUR EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU GOT MY LETTER") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU GOT OUR LETTER") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU GOT MY MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOPE YOU GOT OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("I EMAILED YOU EARLIER WITHOUT") ||
            preProcessedBody.Trim().ToUpper().Contains("I HOPE YOU SAW OUR MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("I HOPE YOU SAW OUR MASSAGE"))
        {
            response += GetRandomQuestionsDidYouSeeOurMessage(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("NOT THE RIGHT ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("INVALID ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("FAKE ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT REAL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("PRETEND ADDRESS"))
        {
            response += GetRandomQuestionsInvalidAddress(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WHAT DID THEY TELL YOU TO DO") ||
            preProcessedBody.Trim().ToUpper().Contains("ALL MESSAGE THAT YOU WILL RECEIVE FROM THEM, YOU MUST FORWARD TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("ALWAYS FORWARD IT TO ME TO MAKE SURE THAT YOU ARE PROTECTED") ||
            preProcessedBody.Trim().ToUpper().Contains("TELL US WHAT THEY TOLD YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("TELL US WHAT THEY SAID TO DO") ||
            preProcessedBody.Trim().ToUpper().Contains("LET ME KNOW WHAT THEY ASKED YOU TO DO") ||
            preProcessedBody.Trim().ToUpper().Contains("WHATEVER THEY SEND YOU FORWARD IT TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT EVER THEY SEND YOU FORWARD IT TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHATEVER THEY SENT YOU FORWARD IT TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT EVER THEY SENT YOU FORWARD IT TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT THE BANK ASK YOU TO DO") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT THE BANK NEEDS YOU TO DO") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT THE BANK HAS YOU DO") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT WAS THE RESPONSE FROM THE BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT THE BANK ASKS YOU TO DO"))
        {
            response += GetRandomQuestionsTellUsWhatTheyAskedYouToDo(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("USE WALMART TO PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART TO SEND THE FEE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU CAN DO THIS AT WALMART") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART TO MAKE PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART TO DO PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART TO HANDLE") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART TO TAKE CARE OF") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART WITH THIS PAY") ||
            preProcessedBody.Trim().ToUpper().Contains("USE WALMART TO WALMART") ||
            preProcessedBody.Trim().ToUpper().Contains("VIA WALMART TO WALMART") ||
            preProcessedBody.Trim().ToUpper().Contains("WALMART STORE CLOSE") ||
            preProcessedBody.Trim().ToUpper().Contains("FEE AT WALMART"))
        {
            response += GetRandomQuestionsUseWalmartToPay(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("HOW MUCH MONEY DO YOU HAVE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MUCH FUNDS DO YOU HAVE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MANY FUNDS DO YOU HAVE") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MUCH DO YOU HAVE RIGHT NOW") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MANY MONEY DO YOU HAVE"))
        {
            response += GetRandomQuestionsHowMuchMoneyDoIHave(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("SEND THE TRANSFER RECIEPT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE TRANSFER RECEIPT") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE TRANSFER RECIEPT") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE TRANSFER RECEIPT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND TRANSFER RECIEPT") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND TRANSFER RECEIPT") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE TRANSFER RECIEPT") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE TRANSFER RECEIPT"))
        {
            currentMessage.IncludePaymentReceipt = true;
            response += GetRandomQuestionsSendTransferReceipt(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("CHECK MY LAST EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("CHECK MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("CHECK MY FIRST EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("CHECK MY LAST MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("CHECK MY PREVIOUS MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("CHECK MY FIRST MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("CHECK MY LAST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("CHECK MY PREVIOUS MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("CHECK MY FIRST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("LOOK AT MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("LOOK AT MY PREVIOUS MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("LOOK AT MY PREVIOUS MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("I ALREADY TOLD YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE ALREADY TOLD YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE RESPONDED ALREADY") ||
            preProcessedBody.Trim().ToUpper().Contains("I JUST TOLD YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I TOLD YOU ALREADY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE MY LAST EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE MY PREVIOUS EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE MY FIRST EMAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE MY LAST MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE MY PREVIOUS MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE MY FIRST MAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE MY LAST MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE MY PREVIOUS MESSAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEE MY FIRST MESSAGE"))
        {
            response += GetRandomQuestionsAlreadyToldYou(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("NO PICK UP WHEN") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU NOT PICK UP") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU WAS NOT PICK UP YOU CALL WHY") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE NOT REACHABLE BY PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("COULD NOT GET YOU ON PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("COULD NOT GET YOU ON THE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("TRIED TO CALL YOU NO RESPONSE") ||
            preProcessedBody.Trim().ToUpper().Contains("TRIED CALLING YOU BUT UNSUCCESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NUMBER IS NOT A WORKING") ||
            preProcessedBody.Trim().ToUpper().Contains("TRIED CALLING YOU ON THE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE TRIED CALLING YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE CALL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PHONE NUMBER IS ANSWER VOICE CALL ONLY") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT PICKING UP") ||
            (
                (preProcessedBody.Trim().ToUpper().Contains("CALL") ||
                    preProcessedBody.Trim().ToUpper().Contains("PHONE") ||
                    preProcessedBody.Trim().ToUpper().Contains("RANG") ||
                    preProcessedBody.Trim().ToUpper().Contains("RING")
                ) &&
                (preProcessedBody.Trim().ToUpper().Contains("NOT ANSWERING ME") ||
                    preProcessedBody.Trim().ToUpper().Contains("IGNORING MY QUESTION") ||
                    preProcessedBody.Trim().ToUpper().Contains("NOT ANSWER MY QUESTION") ||
                    preProcessedBody.Trim().ToUpper().Contains("DID YOU NOT ANSWER") ||
                    preProcessedBody.Trim().ToUpper().Contains("YOU DID NOT ANSWER") ||
                    preProcessedBody.Trim().ToUpper().Contains("NO ANSWER WHEN") ||
                    preProcessedBody.Trim().ToUpper().Contains("YOU DID NO ANSWER") ||
                    preProcessedBody.Trim().ToUpper().Contains("FAILED TO CONNECT") ||
                    preProcessedBody.Trim().ToUpper().Contains("COULD NOT GET YOU ON THE PHONE") ||
                    preProcessedBody.Trim().ToUpper().Contains("DID NOT PICKUP") ||
                    preProcessedBody.Trim().ToUpper().Contains("DIDNT PICKUP") ||
                    preProcessedBody.Trim().ToUpper().Contains("NOT ANSWER ME")
                )
            ) ||
            preProcessedBody.Trim().ToUpper().Contains("YOU DID NOT PICK UP"))
        {
            response += GetRandomQuestionsWhyNoAnswer(rand) + " ";
        }
        else if (preProcessedBody.Trim().ToUpper().Contains("NOT ANSWERING ME") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT ANSWER MY QUESTION") ||
            preProcessedBody.Trim().ToUpper().Contains("DID YOU NOT ANSWER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU DID NOT ANSWER") ||
            preProcessedBody.Trim().ToUpper().Contains("NO ANSWER WHEN") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU DID NO ANSWER") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT ANSWER ME"))
        {
            alreadyRepliedNotAnswering = true;
            response += GetRandomQuestionsNotAnswering(rand) + " ";
        }
        if (!alreadyRepliedNotAnswering)
        {
            if (preProcessedBody.Trim().ToUpper().Contains("A LOT OF DELAY") ||
                preProcessedBody.Trim().ToUpper().Contains("ALOT OF DELAY") ||
                preProcessedBody.Trim().ToUpper().Contains("IGNORING MY QUESTION") ||
                preProcessedBody.Trim().ToUpper().Contains("BUT NO RESPONSE") ||
                preProcessedBody.Trim().ToUpper().Contains("NOT RESPONDING MY QUESTION") ||
                preProcessedBody.Trim().ToUpper().Contains("NO RESPONDING MY QUESTION") ||
                preProcessedBody.Trim().ToUpper().Contains("NOT RESPONDING QUESTION") ||
                preProcessedBody.Trim().ToUpper().Contains("NO RESPONDING QUESTION") ||
                preProcessedBody.Trim().ToUpper().Contains("NO GETTING BACK TO ME") ||
                preProcessedBody.Trim().ToUpper().Contains("NOT GETTING BACK TO ME") ||
                preProcessedBody.Trim().ToUpper().Contains("DIDT RESPOND TO MY MAIL") ||
                preProcessedBody.Trim().ToUpper().Contains("GET BACK TO US") ||
                preProcessedBody.Trim().ToUpper().Contains("WITHOUT RESPONDING TO MY MESSAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("DIDNT RESPOND TO MY MAIL") ||
                preProcessedBody.Trim().ToUpper().Contains("YOU SHOULD COMPLY") ||
                preProcessedBody.Trim().ToUpper().Contains("YOU STOP COMMUNICAT") ||
                preProcessedBody.Trim().ToUpper().Contains("ARE YOU ABANDONING") ||
                preProcessedBody.Trim().ToUpper().Contains("WAITING FOR YOUR RESPONSE") ||
                preProcessedBody.Trim().ToUpper().Contains("WAITING FOR YOU TO REPLY") ||
                preProcessedBody.Trim().ToUpper().Contains("WAITING YOUR REPLY") ||
                preProcessedBody.Trim().ToUpper().Contains("IGNORING QUESTION"))
            {
                response += GetRandomQuestionsNotAnswering(rand) + " ";
            }
        }

        if (preProcessedBody.Trim().ToUpper().Contains("NOT LISTENING ME") ||
            preProcessedBody.Trim().ToUpper().Contains("LISTEN TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("PAY ATTENTION TO ME") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED TO FOLLOW INSTRUCTION") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED TO FOLLOW MY INSTRUCTION") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED TO FOLLOW OUR INSTRUCTION") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED TO FOLLOW THE INSTRUCTION") ||
            preProcessedBody.Trim().ToUpper().Contains("OBEY MY INSTRUCTION") ||
            preProcessedBody.Trim().ToUpper().Contains("OBEY THE INSTRUCTION") ||
            preProcessedBody.Trim().ToUpper().Contains("OBEY OUR INSTRUCTION") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR COMPLIANCE IS AWAITED") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT PAYING ATTENTION"))
        {
            response += GetRandomQuestionsNotListening(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("DID NOT UNDERSTAND") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT UNDERSTANDING MY") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT UNDERSTANDING ME") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT UNDERSTAND WHAT YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFUSED BY MY") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT ARE YOU REALLY SAYING") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT EVER IS YOUR COMPLAIN") ||
            preProcessedBody.Trim().ToUpper().Contains("WHY SO CONFUSED") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE STILL CONFUSE") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU NEED CLARIFICATION"))
        {
            response += GetRandomQuestionsNotUnderstanding(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("HAVE YOUR PERMISSION") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN I HAVE PERMISSION") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED PERMISSION") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED YOUR PERMISSION") ||
            preProcessedBody.Trim().ToUpper().Contains("DO YOU GIVE PERMISSION"))
        {
            response += GetRandomQuestionsPermission(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("DO YOU SPEAK ENGLISH") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT LANGUAGE DO YOU SPEAK") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU TALK ENGLISH") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU SPEAK ENGLISH"))
        {
            response += GetRandomQuestionsSpokenLanguage(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("CAN I TRUST YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE TO TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW CAN I TRUST YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN YOU BE TRUSTED") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED FROM YOU IS YOUR TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED FROM YOU IS YOU TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED TO HAVE YOUR TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED TO KNOW I CAN TRUST") ||
            preProcessedBody.Trim().ToUpper().Contains("CAPITAL WILL BE SAFE") ||
            preProcessedBody.Trim().ToUpper().Contains("MONEY WILL BE SAFE") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS WILL BE SAFE") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND WILL BE SAFE") ||
            preProcessedBody.Trim().ToUpper().Contains("INVESTMENT WILL BE SAFE") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE UNDER YOUR MANAG") ||
            preProcessedBody.Trim().ToUpper().Contains("SAFE DO YOU WANT IT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU WILL NOT BETRAY") ||
            preProcessedBody.Trim().ToUpper().Contains("BETRAY THIS GREAT CONFID") ||
            preProcessedBody.Trim().ToUpper().Contains("IT IS SECURED") ||
            preProcessedBody.Trim().ToUpper().Contains("YES IS SECURED") ||
            preProcessedBody.Trim().ToUpper().Contains("MAKING IT A CERTIFIED TRANSACTION") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSACTION A GUARANTEE SUCCESS") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSACTION A GUARANTEED SUCCESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CAN BE TRUSTED"))
        {
            response += GetRandomQuestionsTrust(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR LEGAL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR LEGAL FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR FIRST AND LAST NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOU NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("STATING YOUR NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("STATING YOUR FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("STATING YOUR LEGAL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("STATING YOUR LEGAL FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("STATING YOUR FIRST AND LAST NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME.....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME .....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME___") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME:___") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME----") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME====") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME :") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME:") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES.....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES .....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES___") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES:___") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES----") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES====") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES :") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMES:") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME INFULL.....") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME INFULL .....") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT NAME DO YOU GO BY") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("RECONFIRM YOUR FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("WHO DO YOU REALLY ARE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAME AND FIRST NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("1. FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("[FULL NAMES]") ||
            preProcessedBody.Trim().ToUpper().Contains("[FULL NAME]") ||
            preProcessedBody.Trim().ToUpper().Contains("FULL NAMES") ||
            preProcessedBody.Trim().ToUpper().Contains("-FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAME AND ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAMES AND ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAME AND MOBILE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAMES AND MOBILE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR COMPLETE NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPLETE FULL NAME") ||
            preProcessedBody.Trim().ToUpper().Contains("NAME\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL NAME AND YOUR RESIDENT ADDRESS AND PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL NAME , RESIDENT ADDRESS AND PHONE NUMBER") ||
            preProcessedBody.Replace(" ", "").ToUpper().Contains("NAME,ADDRESS,EMAIL,TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL NAME"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsName(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("2:YOUR CURRENT RESIDENCE ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE DO YOU LIVE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PROVIDE YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE CAN I SEND") ||
            preProcessedBody.Trim().ToUpper().Contains("MAILING ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("POSTAL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR EXACT ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR HOME AND OFFICE ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("BILLING ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("RESIDENTIAL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("HOME ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS.....") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS .....") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS___") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS:___") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS----") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS====") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS :") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS:") ||
            preProcessedBody.Trim().ToUpper().Contains("OFFICE ADDRESS.") ||
            preProcessedBody.Trim().ToUpper().Contains(", ADDRESS,") ||
            preProcessedBody.Trim().ToUpper().Contains(", MAILING ADDRESS,") ||
            preProcessedBody.Trim().ToUpper().Contains(", POSTAL ADDRESS,") ||
            preProcessedBody.Trim().ToUpper().Contains(", FULL ADDRESS,") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS WHERE YOU WANT HIM TO SEND THE") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS WHERE YOU WANT THEM TO SEND THE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR FULL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR DELIVERY INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOU DELIVERY INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR DELIVERY DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOU DELIVERY DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR RECEIVING ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR DELIVERY ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM WITH YOU DELIVERY INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM WITH YOUR DELIVERY INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM WITH YOU DELIVERY ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM WITH YOUR DELIVERY ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("RECONFIRM TO US DHL YOU FULL INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR COMPLETE ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR FULL INFORMATION TO AVOID WRONG DELIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR OFFICE/RESIDENTIAL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER AND ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("PROVIDE THE RESIDENTIAL") ||
            preProcessedBody.Trim().ToUpper().Contains("PERMANENT ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM US YOUR FULL RESIDENTIAL") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR HOME ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAME AND ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("FULL DELIVERY ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("FULL ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND YOUR OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("DIRECT CONTACT ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("WHICH CITY ARE YOU IN") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE YOUR ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR RESIDENT ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR MAILING ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("LOCATION\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL NAME AND YOUR RESIDENT ADDRESS AND PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL NAME , RESIDENT ADDRESS AND PHONE NUMBER") ||
            preProcessedBody.Replace(" ", "").ToUpper().Contains("NAME,ADDRESS,EMAIL,TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("GIVE ME YOUR ADDRESS"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsAddress(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("COUNTRY OF ORIGIN") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY.....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY .....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY :") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY:") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY___") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY:___") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY----") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY====") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME.....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME .....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME :") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME:") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME___") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME----") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY NAME====") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY.....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY .....") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY :") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY:") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY___") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY----") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY/CITY====") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR COUNTRY.") ||
            preProcessedBody.Trim().ToUpper().Contains(", COUNTRY,") ||
            preProcessedBody.Trim().ToUpper().Contains(", COUNTRY NAME,") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY DO YOU COME FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY, OCCUPATION, AGE AND TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY AND AGE") ||
            preProcessedBody.Trim().ToUpper().Contains("TELL ME WHERE YOU FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR COUNTRY OF ORIGIN") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY ARE YOU FROM") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT COUNTRY DO YOU LIVE"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsCountry(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION.....") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION .....") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION :") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION:") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION___") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION:___") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION----") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION====") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION.....") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION .....") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION :") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION:") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION___") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION----") ||
            preProcessedBody.Trim().ToUpper().Contains("POSITION====") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR OCCUPATION.") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR JOB.") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB___") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB:___") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB----") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB====") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB.....") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB :") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB:") ||
            preProcessedBody.Trim().ToUpper().Contains(", OCCUPATION,") ||
            preProcessedBody.Trim().ToUpper().Contains(", JOB,") ||
            preProcessedBody.Trim().ToUpper().Contains(", JOB TITLE,") ||
            preProcessedBody.Trim().ToUpper().Contains("WHERE DO YOU WORK") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR AGE & OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("[OCCUPATION]") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY, OCCUPATION, AGE AND TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE NUMBER AND OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND YOUR OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PROFESSION") ||
            preProcessedBody.Trim().ToUpper().Contains("WORK\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("JOB\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("OCCUPATION\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR JOB"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsOccupation(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR GENDER") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX.....") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX .....") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX :") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX:") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX___") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX:___") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX----") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX====") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER.....") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER .....") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER :") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER:") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER___") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER----") ||
            preProcessedBody.Trim().ToUpper().Contains("GENDER====") ||
            preProcessedBody.Trim().ToUpper().Contains(", SEX,") ||
            preProcessedBody.Trim().ToUpper().Contains(", GENDER,") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE/SEX/MARITAL STATUS") ||
            preProcessedBody.Trim().ToUpper().Contains("[SEX]") ||
            preProcessedBody.Trim().ToUpper().Contains("SEX\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR SEX"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsGender(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("EMAIL.....") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL .....") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL :") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL___") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL:___") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL----") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL====") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ADDRESS.....") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ADDRESS .....") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ADDRESS :") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ADDRESS:") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ADDRESS___") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ADDRESS----") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ADDRESS====") ||
            preProcessedBody.Trim().ToUpper().Contains(", EMAIL,") ||
            preProcessedBody.Trim().ToUpper().Contains(", EMAIL ADDRESS,") ||
            preProcessedBody.Trim().ToUpper().Contains("[EMAIL ADDRESS]") ||
            preProcessedBody.Trim().ToUpper().Contains("[EMAIL]"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsEmail(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR PASSWORD") ||
                    preProcessedBody.Trim().ToUpper().Contains("PASSWORD.....") ||
                    preProcessedBody.Trim().ToUpper().Contains("PASSWORD .....") ||
                    preProcessedBody.Trim().ToUpper().Contains("PASSWORD :") ||
                    preProcessedBody.Trim().ToUpper().Contains("PASSWORD:") ||
                    preProcessedBody.Trim().ToUpper().Contains("PASSWORD") ||
                    preProcessedBody.Trim().ToUpper().Contains("PASSWORD:___") ||
                    preProcessedBody.Trim().ToUpper().Contains("PASSWORD----") ||
                    preProcessedBody.Trim().ToUpper().Contains("PASSWORD====") ||
                    preProcessedBody.Trim().ToUpper().Contains(", PASSWORD,") ||
                    preProcessedBody.Trim().ToUpper().Contains("AGE/PASSWORD/MARITAL STATUS") ||
                    preProcessedBody.Trim().ToUpper().Contains("[PASSWORD]") ||
                    preProcessedBody.Trim().ToUpper().Contains("PASSWORD\r\n"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsPassword(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("YOUR MARITAL STATUS") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS.....") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS .....") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS___") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS----") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS====") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS :") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITAL STATUS:") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS.....") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS .....") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS___") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS----") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS====") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS :") ||
            preProcessedBody.Trim().ToUpper().Contains("MARITALSTATUS:") ||
            preProcessedBody.Trim().ToUpper().Contains(", MARITAL STATUS,") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE/SEX/MARITAL STATUS") ||
            preProcessedBody.Trim().ToUpper().Contains("ARE YOU SINGLE"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsMaritalStatus(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("OLD ARE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR AGE") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH YEAR") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DAY") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH MONTH") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH TIME") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE :") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE:") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE:___") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE___") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE----") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE====") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE :") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE:") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE___") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE----") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTHDATE====") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE :") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE:") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE___") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE----") ||
            preProcessedBody.Trim().ToUpper().Contains("BIRTH DATE====") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH.....") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH .....") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH :") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH:") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH___") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH----") ||
            preProcessedBody.Trim().ToUpper().Contains("DATE OF BIRTH====") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR AGE.") ||
            preProcessedBody.Trim().ToUpper().Contains(", AGE,") ||
            preProcessedBody.Trim().ToUpper().Contains(", BIRTHDATE,") ||
            preProcessedBody.Trim().ToUpper().Contains(", BIRTH DATE,") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE/SEX/MARITAL STATUS") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR AGE & OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY AND AGE") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY, OCCUPATION, AGE AND TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("[AGE]") ||
            preProcessedBody.Trim().ToUpper().Contains("AGE\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU AGE"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsBirthdate(rand) + " ";
        }
        if (preProcessedBody.Replace(" ", "").ToUpper().Contains("NAME,ADDRESS,EMAIL,TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL NAME AND YOUR RESIDENT ADDRESS AND PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FULL NAME , RESIDENT ADDRESS AND PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains(", PHONE NUMBER,") ||
            preProcessedBody.Trim().ToUpper().Contains(", PHONE,") ||
            preProcessedBody.Trim().ToUpper().Contains(", TELEPHONE NUMBER,") ||
            preProcessedBody.Trim().ToUpper().Contains(", TELEPHONE,") ||
            preProcessedBody.Trim().ToUpper().Contains("1:YOUR TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("ADDRESS AND PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("CELL PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("COUNTRY, OCCUPATION, AGE AND TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("DIRECT PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("DROP YOUR TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("DROP YOUR TELL, WOULD LIKE TO CALL") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT NUMBER DID YOU SAY THAT I SHOULD MESSAGE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT NUMBER DID YOU SAID THAT I SHOULD MESSAGE YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HOME PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW CAN I CALL YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW CAN I REACH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("MOBILE PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED YOUR CELL PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED YOUR DIRECT PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("NEED YOUR PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER AND ADDRESS") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE NUMBER AND OCCUPATION") ||
            preProcessedBody.Trim().ToUpper().Contains("PRIVATE PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOU TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NUMBER\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NUMBERS\r\n") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE/FAX NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT IS YOUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT NUMBER TO") ||
            preProcessedBody.Trim().ToUpper().Contains("WHAT YOUR PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("WORKING TELEPHONE NUM") ||
            preProcessedBody.Trim().ToUpper().Contains("WAITING FOR TELEPHONE NUM") ||
            preProcessedBody.Trim().ToUpper().Contains("WAITING FOR PHONE NUM") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU MOBILE TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR CELL NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR CELL PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR CURRENT PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR DIRECT CELL PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR MOBILE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR MOBILE/CELL PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAME AND MOBILE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NAMES AND MOBILE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR NUMBER.") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PERMANENT TELEPHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PRIVATE PHONE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR TELEPHONE AND FAX NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR TELEPHONE NO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR TELEPHONE NUMBER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR TELEPHONE.") ||
            preProcessedBody.Trim().ToUpper().Contains("[TEL]") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL.....") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL .....") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL___") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL----") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL====") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL :") ||
            preProcessedBody.Trim().ToUpper().Contains("TEL:") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE___") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE----") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE====") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE :") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE:") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO.....") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO .....") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO___") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO----") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO====") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO :") ||
            preProcessedBody.Trim().ToUpper().Contains("TELEPHONE NO:") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE :") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE:") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE___") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE----") ||
            preProcessedBody.Trim().ToUpper().Contains("PHONE====") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER.....") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER .....") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER___") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER:___") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER----") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER====") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER :") ||
            preProcessedBody.Trim().ToUpper().Contains("NUMBER:") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO.....") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO .....") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO___") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO----") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO====") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO :") ||
            preProcessedBody.Trim().ToUpper().Contains("CALL NO:"))
        {
            askedForDetails = true;
            response += GetRandomQuestionsPhoneNumber(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("HOW MUCH LOAN DO YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MUCH DO YOU NEED AS A LOAN") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW LARGE OF A LOAN DO YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW LARGE OF A LOAN DO YOU SEEK") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW LARGE OF A LOAN DO YOU REQUEST") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF FUNDING YOU ARE LOOKING FOR") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF FUNDING YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF FUNDING YOU ARE SEEKING") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF LOAN") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED:") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED :") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED.....") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED______") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED:______") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT REQUESTED-----") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT:") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT :") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT.....") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT______") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT:______") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT-----") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("LOAN AMOUNT REQUEST") ||
            preProcessedBody.Trim().ToUpper().Contains("LET US KNOW THE AMOUNT YOU NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MUCH LOAN YOU ARE IN NEED") ||
            preProcessedBody.Trim().ToUpper().Contains("NOTE THE AMOUNT YOU WANT TO BORROW") ||
            preProcessedBody.Trim().ToUpper().Contains("HOW MUCH OF A LOAN DO YOU NEED"))
        {
            response += GetRandomQuestionsHowBigOfLoan(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("ANY IDENTITY COPY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ALONG YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ID") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("ATTACH YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("ATTACH THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ID OR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR IDENTITY OR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR IDENTIFICATION OR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("UPLOAD YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("UPLOAD THE ID") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT OR ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD OR INTERNATIONAL PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR I.D") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR VALID I.D") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR VALID ID") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR WORK ID") ||
            preProcessedBody.Trim().ToUpper().Contains("COPY OF YOUR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains(" ID COPY") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH OUT IDENTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD.....") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD .....") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD___") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD:___") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD----") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD====") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD :") ||
            preProcessedBody.Trim().ToUpper().Contains("ID CARD:") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD.....") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD .....") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD___") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD:___") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD----") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD====") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD :") ||
            preProcessedBody.Trim().ToUpper().Contains("I.D CARD:") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD.....") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD .....") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD___") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD:___") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD----") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD====") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD :") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITY CARD:") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE.....") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE .....") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE___") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE:___") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE----") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE====") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE :") ||
            preProcessedBody.Trim().ToUpper().Contains("LICENSE:") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT.....") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT .....") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT___") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT:___") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT----") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT====") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT :") ||
            preProcessedBody.Trim().ToUpper().Contains("PASSPORT:") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD.....") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD .....") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD___") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD:___") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD----") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD====") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD :") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTITYCARD:") ||
            preProcessedBody.Trim().ToUpper().Contains(", ID,") ||
            preProcessedBody.Trim().ToUpper().Contains(", ID CARD,") ||
            preProcessedBody.Trim().ToUpper().Contains(", LICENSE,") ||
            preProcessedBody.Trim().ToUpper().Contains(", LICENSE CARD,") ||
            preProcessedBody.Trim().ToUpper().Contains("ANY FORM OF IDENTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR INTL.PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR ID BY ATTACHMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR IDENTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR IDENTITY COPY") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERNATIONAL PASSPORT OR DRIVER LICENSE") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERNATIONAL PASSPORT OR DRIVERS LICENSE") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERNATIONAL APSSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("IDENTIFICATION DOCUMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("NO IDENTIFICATION SENT") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH OUT INDETIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("WITHOUT INDETIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH OUT ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("WITHOUT ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH OUT I.D CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("WITHOUT I.D CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("WORKING IDENTITY CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("PERSONAL ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("SCAN YOUR ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME ANY OF YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME ANY OF YOUR I.D") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME ANY OF YOUR IDENTITY") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME ANY OF YOUR IDENTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME ANY OF YOUR DRIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVED YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVED YOUR I.D") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVED YOUR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVED YOUR DRIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVE YOUR ID") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVE YOUR I.D") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVE YOUR PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("NOT RECEIVE YOUR DRIVER") ||
            preProcessedBody.Trim().ToUpper().Contains("DRIVING ID CARD") ||
            preProcessedBody.Trim().ToUpper().Contains("A CLEAR PICURE") ||
            preProcessedBody.Trim().ToUpper().Contains("PICURE OF YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("A CLEAR PHOTO") ||
            preProcessedBody.Trim().ToUpper().Contains("PHOTO OF YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("PHOTOGRAPH OF YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("DRIVERS LICENSES") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERNATIONAL PASSPORT") ||
            preProcessedBody.Trim().ToUpper().Contains("VALID IDENTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("EMAIL ME YOUR ID"))
        {
            if (!String.IsNullOrEmpty(settings.PathToMyFakeID))
            {
                askedForDetails = true;
                if (!currentMessage.IncludedIDinPast)
                {
                    response += GetRandomQuestionsID(rand) + " ";
                    currentMessage.IncludeID = true; //The SendMessage will pull in the image file
                }
                else
                {
                    response += GetRandomQuestionsAlreadyIncludedID(rand) + " ";
                }
            }
        }
        else
        {
            if (preProcessedBody.Trim().ToUpper().Contains("CANNOT OPEN FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT OPEN FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO OPEN FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE OPEN FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("CANNOT OPEN ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT OPEN ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO OPEN ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE OPEN ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("CANNOT ACCESS FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT ACCESS FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO ACCESS FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE ACCESS FILE") ||
                preProcessedBody.Trim().ToUpper().Contains("CANNOT ACCESS ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT ACCESS ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO ACCESS ATTACH") ||
                preProcessedBody.Trim().ToUpper().Contains("CANNOT OPEN IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("CANNOT ACCESS IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT OPEN IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("CAN NOT ACCESS IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO OPEN IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE OPEN IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE TO ACCESS IMAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("UNABLE ACCESS IMAGE"))
            {
                response += GetRandomQuestionsCannotOpenAttachment(rand) + " ";
            }
        }
        if (preProcessedBody.Trim().ToUpper().Contains("BETTER PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("BETTER PHOTO") ||
            preProcessedBody.Trim().ToUpper().Contains("BETTER IMAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("QUALITY PICUTRE") ||
            preProcessedBody.Trim().ToUpper().Contains("QUALITY PHOTO") ||
            preProcessedBody.Trim().ToUpper().Contains("QUALITY IMAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSER PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSE PICTURE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSER PHOTO") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSE PHOTO") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSER IMAGE") ||
            preProcessedBody.Trim().ToUpper().Contains("CLOSE IMAGE"))
        {
            response += GetRandomQuestionsBetterPhoto(rand) + " ";
        }
        if (preProcessedBody.Trim().ToUpper().Contains("INCLUDE THIS CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THIS CODE :") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE FOLLOWING CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THE FOLLOWING CODE :") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THIS FOLLOWING CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("INCLUDE THIS FOLLOWING CODE :") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THIS CODE :") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THIS CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE CODE :") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE FOLLOWING CODE:") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE FOLLOWING CODE :"))
        {
            string[] split = preProcessedBody.ToUpper().Split(new char[] { ' ' });
            string code = String.Empty;
            bool codeNext = false;

            foreach (string s in split)
            {
                if (s == "CODE:" || s == "CODE")
                {
                    codeNext = true;
                    continue;
                }
                if (codeNext)
                {
                    if (s.Trim() == String.Empty || s.Trim().Length < 3)
                    {
                        continue;
                    }
                    else
                    {
                        code = s;
                        break;
                    }
                }
            }

            if (String.IsNullOrEmpty(code.Trim()))
            {
                //Generate a randome code since we couldn't parse it out. They probably will just ignore the fact that it is the wrong code.
                code = Crypto.CalculateMD5Hash(DateTime.Now.ToString()).Substring(0, rand.Next(4, 10));
            }

            response += Environment.NewLine + "Here is the code: " + code + Environment.NewLine;
        }
        if (!askedForDetails &&
            (preProcessedBody.Trim().ToUpper().Contains("PROVIDE THE REQUIRED DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("PROVIDE ALL THIS DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("PROVIDE THE INFORMATION NEEDED") ||
            preProcessedBody.Trim().ToUpper().Contains("FORWARD ME YOUR FULL DATA") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH YOUR FULL INFORMATION") ||
            preProcessedBody.Trim().ToUpper().Contains("WITHOUT YOUR DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOU FULL DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND US YOUR FULL DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOUR FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND THE REQUIRED INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME THE CONTACT INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR PERSONAL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR FULL DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND ME YOUR INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("RECONFIRM YOUR DATAS") ||
            preProcessedBody.Trim().ToUpper().Contains("I NEED THOSE INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("I ASK YOU FOR YOUR INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("DETAILS NEEDED FROM YOU BEFORE WE CAN PROCEED") ||
            preProcessedBody.Trim().ToUpper().Contains("WAITING FOR YOUR INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU FULL INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR DETAILS INFO") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE TO SEND YOU DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE TO SEND YOUR DETAIL") ||
            preProcessedBody.Trim().ToUpper().Contains("FORWARD THE DETAILS") ||
            preProcessedBody.Trim().ToUpper().Contains("CONFIRM YOUR DATAS") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND FULL INFORMATION")))
        {
            response += GetRandomQuestionsProvideDetails(rand) + " ";
        }
        response = response.Trim();
        if (!String.IsNullOrEmpty(response))
        {
            response = Environment.NewLine + Environment.NewLine + SettingPostProcessing(response, rand).Trim() + Environment.NewLine + Environment.NewLine;
        }

        return response;
    }
    public string GetResponseForType(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages)
    {
        Random rand = new Random();
        EmailType type = EmailType.Unknown;
        EmailTypeParseLit = new List<EmailTypeBase>();
        string rtnResponse = String.Empty;
        string attachmentType = "File";
        string name = String.Empty;
        string greeting = String.Empty;
        string signOff = String.Empty;
        string preProcessedBody = TextProcessing.PreProcessEmailText(settings, currentMessage.SubjectLine, currentMessage.EmailBodyPlain);
        //string preProcessedBody = currentMessage.SubjectLine + " " + currentMessage.EmailBodyPlain.Replace("\r\n", " ").Replace("'","");
        //preProcessedBody = RemoveUselessText(TextProcessing.MakeEmailEasierToRead(RemoveReplyTextFromMessage(preProcessedBody)));

        responseSettings.IsAdmin = IsOverridAdmin;

        EmailTypeParseLit.Add(new CheckBlankWithAttachment(responseSettings));
        EmailTypeParseLit.Add(new CheckIlluminati(responseSettings));
        EmailTypeParseLit.Add(new CheckDeathOrDying(responseSettings));
        EmailTypeParseLit.Add(new CheckRefugee(responseSettings));
        EmailTypeParseLit.Add(new CheckAccountProblem(responseSettings));
        EmailTypeParseLit.Add(new CheckScamVictim(responseSettings));
        EmailTypeParseLit.Add(new CheckOilAndGas(responseSettings));
        EmailTypeParseLit.Add(new CheckJobOffer(responseSettings));
        EmailTypeParseLit.Add(new CheckProductSupplier(responseSettings));
        EmailTypeParseLit.Add(new CheckSellingServices(responseSettings));
        EmailTypeParseLit.Add(new CheckOnlineMarketingConsult(responseSettings));
        EmailTypeParseLit.Add(new CheckLoanOffer(responseSettings));
        EmailTypeParseLit.Add(new CheckLottery(responseSettings));
        EmailTypeParseLit.Add(new CheckPolice(responseSettings));
        EmailTypeParseLit.Add(new CheckMoneyHack(responseSettings));
        EmailTypeParseLit.Add(new CheckAtmCard(responseSettings));
        EmailTypeParseLit.Add(new CheckInheritance(responseSettings));
        EmailTypeParseLit.Add(new CheckBeneficiary(responseSettings));
        EmailTypeParseLit.Add(new CheckMoneyStorage(responseSettings));
        EmailTypeParseLit.Add(new CheckInvestor(responseSettings));
        EmailTypeParseLit.Add(new CheckConsignmentBox(responseSettings));
        EmailTypeParseLit.Add(new CheckGenericPayment(responseSettings));
        EmailTypeParseLit.Add(new CheckBuildTrust(responseSettings));
        EmailTypeParseLit.Add(new CheckSellingProducts(responseSettings));
        EmailTypeParseLit.Add(new CheckPhishing(responseSettings));
        EmailTypeParseLit.Add(new CheckFreeMoney(responseSettings));
        EmailTypeParseLit.Add(new CheckJobOffer(responseSettings));
        EmailTypeParseLit.Add(new CheckInformationGathering(responseSettings));
        EmailTypeParseLit.Add(new CheckShipping(responseSettings));
        EmailTypeParseLit.Add(new CheckGenericAdvertisement(responseSettings));
        EmailTypeParseLit.Add(new CheckForeignLanguage(responseSettings));
        EmailTypeParseLit.Add(new CheckInformationGathering(responseSettings) { PassNumber = 2 });
        EmailTypeParseLit.Add(new CheckFreeMoney(responseSettings) { PassNumber = 2 });
        EmailTypeParseLit.Add(new CheckConsignmentBox(responseSettings) { PassNumber = 2 });
        EmailTypeParseLit.Add(new CheckGenericPayment(responseSettings) { PassNumber = 2 });
        EmailTypeParseLit.Add(new CheckBlankWithAttachment(responseSettings) { PassNumber = 2 });

        if (IsOverridAdmin && preProcessedBody.Trim().ToUpper().StartsWith("_SYN API DOCUMENTATION_"))
        {
            rtnResponse = "Below is a list of keywords you can send to auto trigger the given email type response. Make sure to send an email with the Subject line of the text below. If you reply with multiple types it will only respond with a single type for the one with the highest priority." + Environment.NewLine + Environment.NewLine;
            rtnResponse += "_SYN RANDOM_ (This will return a random response)" + Environment.NewLine;

            foreach (EmailTypeBase et in EmailTypeParseLit)
            {
                rtnResponse += et.AutoResponseKeyword + Environment.NewLine;
            }

            return rtnResponse;
        }
        else if (IsOverridAdmin && preProcessedBody.Trim().ToUpper().StartsWith("_SYN RANDOM_"))
        {
            int rng = rand.Next(0, EmailTypeParseLit.Count() - 1);

            type = EmailTypeParseLit[rng].Type;

            rtnResponse = "(Random response for type: " + type.ToString() + ")" + Environment.NewLine + Environment.NewLine;
        }

        //If we got a type from the Random API call then we can skip looking for a type in the message
        if (type == EmailType.Unknown)
        {
            bool foundSame = false;
            if (preProcessedBody.Length > 200) //Only check for duplacte long emails since some of the shorter emails could be the same between different email threads. Like "What do you mean?" as a reply to many different situations
            {
                DateTime determinedDateCutoff = currentMessage.DateReceived.AddDays(-2.0);

                foreach (MailStorage ms in pastMessages)
                {
                    //string tmpPastMsg = RemoveUselessText(TextProcessing.MakeEmailEasierToRead(RemoveReplyTextFromMessage(ms.SubjectLine + " " + ms.EmailBodyPlain.Replace("\r\n", " ").Replace("'", ""))));
                    string tmpPastMsg = TextProcessing.PreProcessEmailText(settings, ms.SubjectLine, ms.EmailBodyPlain);

                    //Only check recent messages for duplicates, if they resend the same email later we can reply to it again
                    if (ms.DateReceived >= determinedDateCutoff)
                    {
                        if (tmpPastMsg != preProcessedBody)
                        {
                            if (preProcessedBody.Length > 0 && tmpPastMsg.Length > 0)
                            {
                                int sizeDifference = Math.Abs(tmpPastMsg.Length - preProcessedBody.Length);
                                double percentChange = (double)sizeDifference / preProcessedBody.Length;
                                if ((percentChange * 100) < 20) //If we have less than a 20% change in size move to the next check
                                {
                                    //I am going to take 3 word chunks and search for each set of 3 word pairs to attempt to see if the email is mostly the same
                                    string[] words = preProcessedBody.Split(new char[] { ' ' });

                                    int foundSuccessCount = 0;
                                    int foundFailCount = 0;
                                    for (int i = 0; i < words.Length - 2; i++)
                                    {
                                        string tmpTri = words[i] + ' ' + words[i + 1] + ' ' + words[i + 2];
                                        if (tmpPastMsg.Contains(tmpTri))
                                        {
                                            foundSuccessCount++;
                                        }
                                        else
                                        {
                                            foundFailCount++;
                                        }

                                        if (foundFailCount > 20 && foundSuccessCount == 0)
                                        {
                                            break; //Early break if we find nothing but fails to start the compare to save on performance
                                        }
                                    }

                                    //If the amount of 3 word pairs successfully found is less than 90% return a fail
                                    double triFoundPercent = (double)foundSuccessCount / (foundSuccessCount + foundFailCount);
                                    if ((triFoundPercent * 100) > 90)
                                    {
                                        foundSame = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foundSame = true;
                            break;
                        }
                    }
                }
            }

            if (foundSame)
            {
                currentMessage.Ignored = true;
                currentMessage.Replied = true;
                currentMessage.IsDuplicateMessage = true;

                Logger.Write(loggerInfo, "Message determined to be duplicate. Message Subject: " + currentMessage.SubjectLine + ", Message ID: " + currentMessage.MsgId);

                return String.Empty;
            }

            //Check to see if the body contains something that we should skip
            if (preProcessedBody.Trim().ToUpper().Contains("INBOX IS FULL") ||
                preProcessedBody.Trim().ToUpper().Contains("MAILBOX IS FULL") ||
                preProcessedBody.Trim().ToUpper().Contains("THE EMAIL ACCOUNT THAT YOU TRIED TO REACH DOES NOT EXIST") ||
                preProcessedBody.Trim().ToUpper().Contains("RECIPIENT ADDRESS REJECTED: MAILBOX FULL") ||
                preProcessedBody.Trim().ToUpper().Contains("SUSPECTED SPAM MESSAGE REJECTED") ||
                preProcessedBody.Trim().ToUpper().Contains("NO IMMEDIATE DELIVERY: LOAD AVERAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("THIS EMAIL MATCHES A PROFILE THE INTERNET COMMUNITY MAY CONSIDER SPAM") ||
                preProcessedBody.Trim().ToUpper().Contains("WILL NOT ACCEPT DELIVERY OF THIS MESSAGE") ||
                preProcessedBody.Trim().ToUpper().Contains("YOU ARE NOT ALLOWED TO SEND FROM THAT EMAIL ADDRESS"))
            {
                currentMessage.Ignored = true;
                currentMessage.Replied = true;

                Logger.Write(loggerInfo, "Message was bounceback. Skip the message. Message Subject: " + currentMessage.SubjectLine + ", Message ID: " + currentMessage.MsgId);

                return String.Empty;
            }

            //Types of emails
            if (Settings.ParseBooleanSetting(settings.EnableLongMessageTypeReplies) && preProcessedBody.Length > settings.LongMessageUpperLimit)
            {
                type = EmailType.MessageTooLong;
            }
            else if (Settings.ParseBooleanSetting(settings.EnableShortMessageTypeReplies) && preProcessedBody.Length - TextProcessing.RemoveUselessText(TextProcessing.MakeEmailEasierToRead(currentMessage.SubjectLine)).Length < settings.ShortMessageLowerLimit)
            {
                type = EmailType.MessageTooShort;
            }
            else if (currentMessage.SubjectLine.Contains("Test ") || currentMessage.SubjectLine.Contains(" Test"))
            {
                type = EmailType.Test;
            }
            else if (EmailTypeParseLit.Count() > 0)
            {
                for (int i = 0; i < EmailTypeParseLit.Count(); i++)
                {
                    TypeParseResponse response = EmailTypeParseLit[i].TryTypeParse(loggerInfo, ref currentMessage, pastMessages, preProcessedBody);

                    if (response.IsMatch)
                    {
                        type = EmailTypeParseLit[i].Type;
                        break;
                    }
                }

                if (type == EmailType.Unknown)
                {
                    for (int i = pastMessages.Count() - 1; i >= 0; i--)
                    {
                        if (pastMessages[i].MessageType != 0)
                        {
                            type = (EmailType)pastMessages[i].MessageType;
                            break;
                        }
                    }
                }
            }
        }

        if ((EmailType)currentMessage.MessageType == EmailType.Unknown || type != EmailType.Unknown)
            currentMessage.MessageType = (int)type;

        //Determine attachment type
        if (currentMessage.AtachmentTypes.ToUpper().Contains("PDF"))
        {
            attachmentType = "PDF";
        }
        else if (currentMessage.AtachmentTypes.ToUpper().Contains("DOC") || currentMessage.AtachmentTypes.ToUpper().Contains("RTF"))
        {
            attachmentType = "Word";
        }
        else if (currentMessage.AtachmentTypes.ToUpper().Contains("PNG") ||
            currentMessage.AtachmentTypes.ToUpper().Contains("JPG") ||
            currentMessage.AtachmentTypes.ToUpper().Contains("JPEG") ||
            currentMessage.AtachmentTypes.ToUpper().Contains("BMP") ||
            currentMessage.AtachmentTypes.ToUpper().Contains("GIF") ||
            currentMessage.AtachmentTypes.ToUpper().Contains("TIFF"))
        {
            attachmentType = "Image";
        }

        //Look to see if we have already sent our ID to them, so that we do not send it again
        for (int i = pastMessages.Count() - 1; i > 0; i--)
        {
            if (pastMessages[i].IncludeID || pastMessages[i].IncludedIDinPast)
            {
                currentMessage.IncludedIDinPast = true;
                break;
            }
        }

        //Try to find their name from the past messages
        if (String.IsNullOrEmpty(currentMessage.PersonName))
        {
            for (int i = pastMessages.Count() - 1; i > 0; i--)
            {
                if (!String.IsNullOrEmpty(pastMessages[i].PersonName))
                {
                    name = pastMessages[i].PersonName;
                    break;
                }
            }
        }
        else
        {
            name = currentMessage.PersonName;
        }
        if (String.IsNullOrEmpty(name))
        {
            //Parse the body looking for their name, if a name is not found we generate a random one.
            //TODO Add more name searching methods
            string tmpName = TextProcessing.AttemptToFindPersonName(currentMessage.EmailBodyPlain);
            if (!String.IsNullOrEmpty(tmpName))
                name = tmpName;
        }
        if (String.IsNullOrEmpty(name))
        {
            name = GetRandomName(rand);
        }

        //Some of the scammers like to use an invalid email to send the message then instruct you to email them with the email in the message (Maybe to avoid getting the mailbox shutdown) so try to find the email address they want you to send to
        string newEmailAddress = TextProcessing.AttemptToFindReplyToEmailAddress(settings, preProcessedBody).Trim();
        if (!String.IsNullOrEmpty(newEmailAddress))
        {
            //Remove trailing "." from email address if it was displayed at the end of a sentence
            while (newEmailAddress.EndsWith("."))
            {
                newEmailAddress = newEmailAddress.Substring(0, newEmailAddress.Length - 1);
            }
            //Remove trailing "_" from email address. Sometimes their messages start and end every line with "_" and not sure why
            while (newEmailAddress.EndsWith("_"))
            {
                newEmailAddress = newEmailAddress.Substring(0, newEmailAddress.Length - 1);
            }
            //Remove trailing "|" from email address. Sometimes they throw random symbols after the email address
            while (newEmailAddress.EndsWith("|"))
            {
                newEmailAddress = newEmailAddress.Substring(0, newEmailAddress.Length - 1);
            }

            if (!currentMessage.ToAddress.Trim().ToUpper().Contains(newEmailAddress.ToUpper()) && !settings.EmailAddress.ToUpper().Contains(newEmailAddress.ToUpper()))
            {
                currentMessage.ToAddress += newEmailAddress + ";";
            }
        }

        //If after all the checks we still do not know who to send it to then search every word to see if any are a valid email address
        if (String.IsNullOrEmpty(currentMessage.ToAddress))
        {
            string[] tempSplit = (currentMessage.SubjectLine + " " + currentMessage.EmailBodyHtml).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int k = 0; k < tempSplit.Count(); k++)
            {
                if (tempSplit[k].Contains("@"))
                {
                    if (TextProcessing.IsValidEmail(settings, tempSplit[k]))
                    {
                        currentMessage.ToAddress += tempSplit[k] + ";";
                    }
                }
            }
        }

        currentMessage.PersonName = name;
        greeting = GetRandomGreeting(rand);
        signOff = GetRandomSignOff(rand);

        switch ((EmailType)currentMessage.MessageType)
        {
            case EmailType.Test:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomOpeningResponseTest(rand);
                else
                    rtnResponse += GetRandomContinuedResponseTest(rand);
                break;
            case EmailType.BlankWithAttachment:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomOpeningResponseForBlankEmailWithAttachment(rand, greeting, attachmentType);
                else
                    rtnResponse += GetRandomContinuedResponseForBlankEmailWithAttachment(rand, greeting, attachmentType);
                break;
            case EmailType.Inheritance:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForInheritance(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForInheritance(rand, greeting, name, attachmentType, currentMessage, pastMessages, preProcessedBody);
                break;
            case EmailType.Beneficiary:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForBeneficiary(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForBeneficiary(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.Lottery:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForLottery(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForLottery(rand, greeting, currentMessage, preProcessedBody);
                break;
            case EmailType.OilAndGas:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForOilAndGas(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForOilAndGas(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.Illuminati:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForIlluminati(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForIlluminati(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.ConsignmentBox:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForConsignmentBox(rand, greeting, name, attachmentType, currentMessage, pastMessages, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForConsignmentBox(rand, greeting, name, attachmentType, currentMessage, preProcessedBody);
                break;
            case EmailType.DeathOrDying:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForDeathOrDying(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForDeathOrDying(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.LoanOffer:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForLoanOffer(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForLoanOffer(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.MoneyStorage:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForMoneyStorage(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForMoneyStorage(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.AtmCard:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForAtmCard(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForAtmCard(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.Police:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForPolice(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForPolice(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.GenericPayment:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForGenericPayment(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForGenericPayment(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.SellingServices:
                rtnResponse += GetRandomResponseForSellingServices(rand, greeting, name, currentMessage);
                break;
            case EmailType.OnlineMarketingConsult:
                rtnResponse += "No";
                break;
            case EmailType.BuildTrust:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForBuildTrust(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForBuildTrust(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.Investor:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForInvestor(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForInvestor(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.MoneyHack:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForMoneyHack(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForMoneyHack(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.JobOffer:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForJobOffer(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForJobOffer(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.SellingProducts:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForSellingProducts(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForSellingProducts(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.FreeMoney:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForFreeMoney(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForFreeMoney(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.InformationGathering:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForInformationGathering(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForInformationGathering(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.Phishing:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForPhishing(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForPhishing(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.ScamVictim:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForScamVictims(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForScamVictims(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.ForeignLanguage:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForForeignLanguage(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForForeignLanguage(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.GenericAdvertisement:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForGenericAdvertisement(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForGenericAdvertisement(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.Shipping:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForShipping(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForShipping(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.Refugee:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForRefugee(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForRefugee(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.AccountProblem:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForAccountProblem(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForAccountProblem(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.ProductSupplier:
                if (pastMessages.Count() > 0 && pastMessages[pastMessages.Count() - 1].MessageType == currentMessage.MessageType)
                    rtnResponse += GetRandomContinuedResponseForProductSupplier(rand, greeting, name, currentMessage, preProcessedBody);
                else
                    rtnResponse += GetRandomOpeningResponseForProductSupplier(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.MessageTooLong:
                rtnResponse += GetRandomOpeningResponseLongMessageType(rand, greeting, name, currentMessage, preProcessedBody);
                break;
            case EmailType.MessageTooShort:
                rtnResponse += GetRandomOpeningResponseShortMessageType(rand, greeting, name, currentMessage, preProcessedBody);
                break;
        }

        //If we cannot determine a type but we can answer a direct question then reply with that and see if they reveal what they are really after
        if (String.IsNullOrEmpty(rtnResponse))
        {
            rtnResponse += HandleDirectQuestions(preProcessedBody, ref currentMessage, rand).Trim('\r').Trim('\n').Trim();
        }

        if (!String.IsNullOrEmpty(rtnResponse))
            rtnResponse += Environment.NewLine + Environment.NewLine + signOff + ", " + MyName;

        //TODO Make enabling this a setting
        //Replace synonyms
        rtnResponse = TextProcessing.SynonymReplacement(rand, rtnResponse);

        return rtnResponse;
    }

    private string SettingPostProcessing(string text, Random rand)
    {
        return SettingPostProcessing(text, new List<string> { }, new List<string> { }, rand);
    }
    private string SettingPostProcessing(string text, List<string> placeholder, List<string> replacement, Random rand)
    {
        string paymentType = String.Empty;
        if (placeholder.Count() != replacement.Count())
            throw new Exception("Setting Post Process counts do not match");

        //Attempt to find the payment method they are requesting and default to the generic term "transfer" if we cannot find it
        paymentType = TextProcessing.AttemptToFindPaymentType(text);
        if (String.IsNullOrEmpty(paymentType))
            paymentType = "transfer";

        placeholder.Add("|introduction|");
        replacement.Add(GetRandomInroduction(rand));
        placeholder.Add("|GetRandomMemory|");
        replacement.Add(GetRandomMemory(rand));
        placeholder.Add("|GetRandomAcquaintance|");
        replacement.Add(GetRandomAcquaintance(rand));
        placeholder.Add("|GetRandomFollowupLine|");
        replacement.Add(GetRandomFollowupLine(rand));
        placeholder.Add("|GetRandomLocation|");
        replacement.Add(GetRandomLocation(rand));
        placeholder.Add("|GetRandomProduct|");
        replacement.Add(GetRandomProduct(rand));
        placeholder.Add("|GetRandomPaymentMethod|");
        replacement.Add(GetRandomPaymentMethod(rand));
        placeholder.Add("|GetRandomThought|");
        replacement.Add(GetRandomThought(rand));
        placeholder.Add("|GetRandomDevice|");
        replacement.Add(GetRandomDevice(rand));
        placeholder.Add("|PhoneNumber|");
        replacement.Add(settings.MyFakePhoneNumber);
        placeholder.Add("|Address|");
        replacement.Add(settings.MyFakeAddress);
        placeholder.Add("|Birthdate|");
        replacement.Add(settings.MyFakeBirthdate);
        placeholder.Add("|Occupation|");
        replacement.Add(settings.MyFakeOccupation);
        placeholder.Add("|Gender|");
        replacement.Add(settings.MyFakeGender);
        placeholder.Add("|MaritalStatus|");
        replacement.Add(settings.MyFakeMaritalStatus);
        placeholder.Add("|Country|");
        replacement.Add(settings.MyFakeCountry);
        placeholder.Add("|Name|");
        replacement.Add(settings.MyName);
        placeholder.Add("|Email|");
        replacement.Add(settings.EmailAddress);
        placeholder.Add("|GetRandomNumber10|");
        replacement.Add(GetRandomNumber(rand, 10).ToString());
        placeholder.Add("|GetRandomNumber100|");
        replacement.Add(GetRandomNumber(rand, 100).ToString());
        placeholder.Add("|GetRandomNumber1000|");
        replacement.Add(GetRandomNumber(rand, 1000).ToString());
        placeholder.Add("|GetRandomNumber10000|");
        replacement.Add(GetRandomNumber(rand, 10000).ToString());
        placeholder.Add("|GetRandomNumber100000|");
        replacement.Add(GetRandomNumber(rand, 100000).ToString());
        placeholder.Add("|GetRandomNumber1000000|");
        replacement.Add(GetRandomNumber(rand, 1000000).ToString());
        placeholder.Add("|GetRandomColor|");
        replacement.Add(GetRandomColor(rand));
        placeholder.Add("|GetRandomCharacters1|");
        replacement.Add(GetRandomCharacters(rand, 1));
        placeholder.Add("|GetRandomCharacters2|");
        replacement.Add(GetRandomCharacters(rand, 2));
        placeholder.Add("|GetRandomCharacters3|");
        replacement.Add(GetRandomCharacters(rand, 3));
        placeholder.Add("|GetRandomCharacters4|");
        replacement.Add(GetRandomCharacters(rand, 4));
        placeholder.Add("|GetRandomCharacters5|");
        replacement.Add(GetRandomCharacters(rand, 5));
        placeholder.Add("|GetRandomCharacters10|");
        replacement.Add(GetRandomCharacters(rand, 10));
        placeholder.Add("|GetRandomCharacters25|");
        replacement.Add(GetRandomCharacters(rand, 25));
        placeholder.Add("|GetRandomCharacters50|");
        replacement.Add(GetRandomCharacters(rand, 50));
        placeholder.Add("|GetRandomCharacters100|");
        replacement.Add(GetRandomCharacters(rand, 100));
        placeholder.Add("|GetRandomCharacters1000|");
        replacement.Add(GetRandomCharacters(rand, 1000));
        placeholder.Add("|GetListOfAcquaintance|");
        replacement.Add(GetListOfAcquaintance(rand, 2));
        placeholder.Add("|GetPaymentTypeFromMessage|");
        replacement.Add(paymentType);
        placeholder.Add("|Environment.NewLine|");
        replacement.Add(Environment.NewLine);

        int ctr = 0;
        while (text.Contains("|") && ctr < 25) //Sometimes replace values contain other replace values, keep looping until we have replaces them all
        {
            for (int i = 0; i < placeholder.Count(); i++)
            {
                text = text.Replace(placeholder[i], replacement[i]);
            }
            ctr++;
        }

        return text;
    }
    public string GetRandomCharacters(Random rand, int numberOfChars)
    {
        string rtn = String.Empty;
        string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] charArr = chars.ToCharArray();

        for (int i = 0; i < numberOfChars; i++)
        {
            rtn += charArr[rand.Next(0, charArr.Length - 1)];
        }

        return rtn;
    }
    public int GetRandomNumber(Random rand, int maxNumber)
    {
        return rand.Next(0, maxNumber);
    }
    public string GetRandomColor(Random rand)
    {
        List<string> colors = new List<string>() { "red", "yellow", "blue", "pink", "orange", "purple", "green", "brown", "white", "black", "grey" };

        return colors[rand.Next(0, colors.Count() - 1)];
    }

    #region SpecialTypes
    private string GetRandomOpeningResponseLongMessageType(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseLongMessageType[rand.Next(0, settings.ResponseLongMessageType.Count())], rand);
    }
    private string GetRandomOpeningResponseShortMessageType(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseShortMessageType[rand.Next(0, settings.ResponseShortMessageType.Count())], rand);
    }
    private string GetRandomResponseForSellingServices(Random rand, string greetings, string name, MailStorage currentMessage)
    {
        List<string> lst = new List<string>
        {
            "UNSUBSCRIBE",
            "Please remove me from your mailing list"
        };

        return lst[rand.Next(0, lst.Count())];
    }
    #endregion

    #region Opening Responses
    private string GetRandomOpeningResponseTest(Random rand)
    {
        List<string> lst = new List<string>
        {
            "Your Test message has been received.",
            "Thank you for the Test message.",
            "The Test email worked successfully."
        };

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomOpeningResponseForBeneficiary(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string response = String.Empty;
        string introduction = String.Empty;
        string followup = String.Empty;
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        response += greetings + " " + name + ", ";

        //Opening introduction of myself
        introduction = SettingPostProcessing(GetRandomInroduction(rand), rand);
        //Closing Followup line
        followup = GetRandomFollowupLine(rand);

        response += directResponse + introduction + Environment.NewLine + Environment.NewLine + followup;

        return response;
    }
    private string GetRandomOpeningResponseForBlankEmailWithAttachment(Random rand, string greetings, string attachmentType)
    {
        return greetings + ". " + SettingPostProcessing(settings.ResponseOpeningBlankEmailWithAttachment[rand.Next(0, settings.ResponseOpeningBlankEmailWithAttachment.Count())], new List<string> { "|attachmentType|" }, new List<string> { attachmentType }, rand);
    }
    private string GetRandomOpeningResponseForLottery(Random rand, string greetings, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningLottery[rand.Next(0, settings.ResponseOpeningLottery.Count())], rand);
    }
    private string GetRandomOpeningResponseForOilAndGas(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string rtn = String.Empty;
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        rtn = greetings + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningOilAndGas[rand.Next(0, settings.ResponseOpeningOilAndGas.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 5) }, rand);

        if (currentMessage.EmailBodyPlain.ToUpper().Contains("LOW PRICE") || currentMessage.EmailBodyPlain.ToUpper().Contains("LOW RATE") || currentMessage.EmailBodyPlain.ToUpper().Contains("CHEAP") || currentMessage.EmailBodyPlain.ToUpper().Contains("DISCOUNT") || currentMessage.EmailBodyPlain.ToUpper().Contains("A GOOD PRICE"))
        {
            rtn += Environment.NewLine + Environment.NewLine + "You mention that I would get the product at a good rate so I have a few questions:" + Environment.NewLine;
            rtn += GetRandomListOfOilAndGasQuestions(rand, 2);
            rtn += "These are the initial questions I have.";
        }

        return rtn;
    }
    private string GetRandomOpeningResponseForIlluminati(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningIlluminati[rand.Next(0, settings.ResponseOpeningIlluminati.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForConsignmentBox(Random rand, string greetings, string name, string attachmentType, MailStorage currentMessage, string preProcessedBody)
    {
        string attachmentIncludedText = String.Empty;
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        if (attachmentType == "Image")
        {
            attachmentIncludedText = " " + GetRandomConsignmentBoxImageIncluded(rand);
        }
        else
        {
            attachmentIncludedText = " " + GetRandomConsignmentBoxImageNotIncluded(rand);
        }

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningConsignmentBox[rand.Next(0, settings.ResponseOpeningConsignmentBox.Count())], new List<string> { "|attachmentIncludedText|" }, new List<string> { attachmentIncludedText }, rand);
    }
    private string GetRandomOpeningResponseForDeathOrDying(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningDeathOrDying[rand.Next(0, settings.ResponseOpeningDeathOrDying.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForLoanOffer(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningLoanOffer[rand.Next(0, settings.ResponseOpeningLoanOffer.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForMoneyStorage(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningMoneyStorage[rand.Next(0, settings.ResponseOpeningMoneyStorage.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForAtmCard(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningAtmCard[rand.Next(0, settings.ResponseOpeningAtmCard.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForPolice(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningPolice[rand.Next(0, settings.ResponseOpeningPolice.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForGenericPayment(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningGenericPayment[rand.Next(0, settings.ResponseOpeningGenericPayment.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForInvestor(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningInvestor[rand.Next(0, settings.ResponseOpeningInvestor.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForMoneyHack(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningMoneyHack[rand.Next(0, settings.ResponseOpeningMoneyHack.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForInheritance(Random rand, string greetings, string name, string attamentType, MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        string response = String.Empty;
        string introduction = String.Empty;
        string inheritorDescription = String.Empty;
        string memories = String.Empty;
        string followup = String.Empty;
        bool isMale = true;
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        response += greetings + " " + name + ", ";

        //Opening introduction of myself
        introduction = SettingPostProcessing(GetRandomInroduction(rand), rand);
        //Description of interitor
        inheritorDescription = GetRandomPersonDescription(rand, ref isMale);
        //Share memories
        memories = GetRandomMemory(rand, isMale);
        //Closing Followup line
        followup = GetRandomFollowupLine(rand);

        response += directResponse + introduction + Environment.NewLine + Environment.NewLine + inheritorDescription + Environment.NewLine + Environment.NewLine + memories + Environment.NewLine + Environment.NewLine + followup;

        return response;
    }
    private string GetRandomOpeningResponseForBuildTrust(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string introduction = SettingPostProcessing(GetRandomInroduction(rand), rand);
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        List<string> lst = new List<string>
        {
            greetings + " friend, " + directResponse + introduction,
            greetings + " my friend, " + directResponse + introduction
        };

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomOpeningResponseForJobOffer(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningJobOffer[rand.Next(0, settings.ResponseOpeningJobOffer.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForSellingProducts(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        return SettingPostProcessing(settings.ResponseOpeningSellingProducts[rand.Next(0, settings.ResponseOpeningSellingProducts.Count())], new List<string> { }, new List<string> { }, rand);
    }
    private string GetRandomOpeningResponseForFreeMoney(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningFreeMoney[rand.Next(0, settings.ResponseOpeningFreeMoney.Count())], rand);
    }
    private string GetRandomOpeningResponseForInformationGathering(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningInformationGathering[rand.Next(0, settings.ResponseOpeningInformationGathering.Count())], rand);
    }
    private string GetRandomOpeningResponseForPhishing(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningPhishing[rand.Next(0, settings.ResponseOpeningPhishing.Count())], rand);
    }
    private string GetRandomOpeningResponseForScamVictims(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningScamVictim[rand.Next(0, settings.ResponseOpeningScamVictim.Count())], rand);
    }
    private string GetRandomOpeningResponseForForeignLanguage(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningForeignLanguage[rand.Next(0, settings.ResponseOpeningForeignLanguage.Count())], rand);
    }
    private string GetRandomOpeningResponseForGenericAdvertisement(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningGenericAdvertisement[rand.Next(0, settings.ResponseOpeningGenericAdvertisement.Count())], rand);
    }
    private string GetRandomOpeningResponseForShipping(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningShipping[rand.Next(0, settings.ResponseOpeningShipping.Count())], rand);
    }
    private string GetRandomOpeningResponseForRefugee(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningRefugee[rand.Next(0, settings.ResponseOpeningRefugee.Count())], rand);
    }
    private string GetRandomOpeningResponseForAccountProblem(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningAccountProblem[rand.Next(0, settings.ResponseOpeningAccountProblem.Count())], rand);
    }
    private string GetRandomOpeningResponseForProductSupplier(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningProductSupplier[rand.Next(0, settings.ResponseOpeningProductSupplier.Count())], rand);
    }
    #endregion

    //Continued Responses
    #region Continued Responses
    private string GetRandomContinuedResponseTest(Random rand)
    {
        List<string> lst = new List<string>
        {
            "Your continued Test message has been received.",
            "Thank you for the continued Test message.",
            "The Test email continues to work successfully."
        };

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomContinuedResponseForBlankEmailWithAttachment(Random rand, string greetings, string attachmentType)
    {
        return greetings + ". " + SettingPostProcessing(settings.ResponseContinuedBlankEmailWithAttachment[rand.Next(0, settings.ResponseContinuedBlankEmailWithAttachment.Count())], new List<string> { "|attachmentType|" }, new List<string> { attachmentType }, rand);
    }
    private string GetRandomContinuedResponseForLottery(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedLottery[rand.Next(0, settings.ResponseContinuedLottery.Count())], rand);
    }
    private string GetRandomContinuedResponseForOilAndGas(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string rtn = String.Empty;
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        rtn = greetings + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedOilAndGas[rand.Next(0, settings.ResponseContinuedOilAndGas.Count())], new List<string> { "|GetListOfAcquaintance|" }, new List<string> { GetListOfAcquaintance(rand, 5) }, rand);

        if (currentMessage.EmailBodyPlain.ToUpper().Contains("LOW PRICE") || currentMessage.EmailBodyPlain.ToUpper().Contains("LOW RATE") || currentMessage.EmailBodyPlain.ToUpper().Contains("CHEAP") || currentMessage.EmailBodyPlain.ToUpper().Contains("DISCOUNT") || currentMessage.EmailBodyPlain.ToUpper().Contains("A GOOD PRICE"))
        {
            rtn += Environment.NewLine + Environment.NewLine + "I have a few more questions:" + Environment.NewLine;
            rtn += GetRandomListOfOilAndGasQuestions(rand, 2);
            rtn += "These are the additional questions I have.";
        }

        return rtn;
    }
    private string GetRandomContinuedResponseForIlluminati(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedIlluminati[rand.Next(0, settings.ResponseContinuedIlluminati.Count())], rand);
    }
    private string GetRandomContinuedResponseForConsignmentBox(Random rand, string greetings, string name, string attachmentType, MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        string attachmentIncludedText = String.Empty;
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        if (attachmentType == "Image")
        {
            attachmentIncludedText = " " + GetRandomConsignmentBoxImageIncluded(rand);
        }
        else
        {
            attachmentIncludedText = " " + GetRandomConsignmentBoxImageNotIncluded(rand);
        }

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedConsignmentBox[rand.Next(0, settings.ResponseContinuedConsignmentBox.Count())], new List<string> { "|attachmentIncludedText|" }, new List<string> { attachmentIncludedText }, rand);
    }
    private string GetRandomContinuedResponseForDeathOrDying(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedDeathOrDying[rand.Next(0, settings.ResponseContinuedDeathOrDying.Count())], rand);
    }
    private string GetRandomContinuedResponseForLoanOffer(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedLoanOffer[rand.Next(0, settings.ResponseContinuedLoanOffer.Count())], rand);
    }
    private string GetRandomContinuedResponseForMoneyStorage(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedMoneyStorage[rand.Next(0, settings.ResponseContinuedMoneyStorage.Count())], rand);
    }
    private string GetRandomContinuedResponseForAtmCard(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedAtmCard[rand.Next(0, settings.ResponseContinuedAtmCard.Count())], rand);
    }
    private string GetRandomContinuedResponseForPolice(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedPolice[rand.Next(0, settings.ResponseContinuedPolice.Count())], rand);
    }
    private string GetRandomContinuedResponseForGenericPayment(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningGenericPayment[rand.Next(0, settings.ResponseOpeningGenericPayment.Count())], rand);
    }
    private string GetRandomContinuedResponseForInvestor(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseOpeningInvestor[rand.Next(0, settings.ResponseOpeningInvestor.Count())], rand);
    }
    private string GetRandomContinuedResponseForMoneyHack(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedMoneyHack[rand.Next(0, settings.ResponseContinuedMoneyHack.Count())], rand);
    }
    private string GetRandomContinuedResponseForInheritance(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedInheritance[rand.Next(0, settings.ResponseContinuedInheritance.Count())], rand);
    }
    private string GetRandomContinuedResponseForBeneficiary(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedBeneficiary[rand.Next(0, settings.ResponseContinuedBeneficiary.Count())], rand);
    }
    private string GetRandomContinuedResponseForBuildTrust(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedBuildTrust[rand.Next(0, settings.ResponseContinuedBuildTrust.Count())], rand);
    }
    private string GetRandomContinuedResponseForJobOffer(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedJobOffer[rand.Next(0, settings.ResponseContinuedJobOffer.Count())], rand);
    }
    private string GetRandomContinuedResponseForSellingProducts(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedSellingProducts[rand.Next(0, settings.ResponseContinuedSellingProducts.Count())], rand);
    }
    private string GetRandomContinuedResponseForFreeMoney(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedFreeMoney[rand.Next(0, settings.ResponseContinuedFreeMoney.Count())], rand);
    }
    private string GetRandomContinuedResponseForInformationGathering(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedInformationGathering[rand.Next(0, settings.ResponseContinuedInformationGathering.Count())], rand);
    }
    private string GetRandomContinuedResponseForPhishing(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedPhishing[rand.Next(0, settings.ResponseContinuedPhishing.Count())], rand);
    }
    private string GetRandomContinuedResponseForScamVictims(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedScamVictim[rand.Next(0, settings.ResponseContinuedScamVictim.Count())], rand);
    }
    private string GetRandomContinuedResponseForForeignLanguage(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedForeignLanguage[rand.Next(0, settings.ResponseContinuedForeignLanguage.Count())], rand);
    }
    private string GetRandomContinuedResponseForGenericAdvertisement(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedGenericAdvertisement[rand.Next(0, settings.ResponseContinuedGenericAdvertisement.Count())], rand);
    }
    private string GetRandomContinuedResponseForShipping(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedShipping[rand.Next(0, settings.ResponseContinuedShipping.Count())], rand);
    }
    private string GetRandomContinuedResponseForRefugee(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedRefugee[rand.Next(0, settings.ResponseContinuedRefugee.Count())], rand);
    }
    private string GetRandomContinuedResponseForAccountProblem(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedAccountProblem[rand.Next(0, settings.ResponseContinuedAccountProblem.Count())], rand);
    }
    private string GetRandomContinuedResponseForProductSupplier(Random rand, string greetings, string name, MailStorage currentMessage, string preProcessedBody)
    {
        string directResponse = HandleDirectQuestions(preProcessedBody, ref currentMessage, rand);

        return greetings + " " + name + ". " + directResponse + SettingPostProcessing(settings.ResponseContinuedProductSupplier[rand.Next(0, settings.ResponseContinuedProductSupplier.Count())], rand);
    }
    #endregion

    //Supporting Random lists
    #region Random Lists
    public string GetRandomName(Random rand)
    {
        List<string> lst = settings.Names;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomGreeting(Random rand)
    {
        List<string> lst = settings.Greeting;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomSignOff(Random rand)
    {
        List<string> lst = settings.Signoff;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomInroduction(Random rand)
    {
        //Simply get these lines and let the SettingPostProcessing handle all the replacements
        string opening = GetRandomIntroductionOpeningLine(rand);
        string body = GetRandomIntroductionBodyLine(rand);
        string closing = GetRandomIntroductionClosingLine(rand);

        return opening + " " + body + " " + closing;
    }
    public string GetRandomIntroductionOpeningLine(Random rand)
    {
        List<string> lst = settings.IntroductionOpening;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomIntroductionBodyLine(Random rand)
    {
        List<string> lst = settings.Introduction;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomIntroductionClosingLine(Random rand)
    {
        List<string> lst = settings.IntroductionClosing;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomPersonDescription(Random rand, ref bool descriptionIsMale)
    {
        List<string> lstMale = settings.PersonDescriptionMale;
        List<string> lstFemale = settings.PersonDescriptionFemale;

        if (rand.Next(0, 100) > 50)
        {
            descriptionIsMale = true;
            return lstMale[rand.Next(0, lstMale.Count())];
        }
        else
        {
            descriptionIsMale = false;
            return lstFemale[rand.Next(0, lstFemale.Count())];
        }
    }
    public string GetRandomMemory(Random rand)
    {
        return GetRandomMemory(rand, null);
    }
    public string GetRandomMemory(Random rand, bool? isMale)
    {
        string rtnValue = String.Empty;
        List<string> lst = settings.Memory;

        rtnValue = lst[rand.Next(0, lst.Count())];
        if (isMale != null && isMale == false)
        {
            rtnValue = rtnValue.Replace(" him ", " her ").Replace(" he ", " she ").Replace(" his ", " her ").Replace(" Him ", " Her ").Replace(" He ", " She ").Replace(" His ", " Her ").Replace(" him.", " her.").Replace(" he.", " she.").Replace(" his.", " her.");
        }

        return rtnValue;
    }
    public string GetRandomFollowupLine(Random rand)
    {
        List<string> lst = settings.FollowupLine;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomAcquaintance(Random rand)
    {
        List<string> lst = settings.Acquaintance;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetListOfAcquaintance(Random rand, int count)
    {
        string rtn = String.Empty;

        for (int i = 0; i < count; i++)
        {
            if (i != 0)
                rtn += ", ";

            rtn += GetRandomAcquaintance(rand);
        }

        return rtn;
    }
    public string GetRandomListOfOilAndGasQuestions(Random rand, int count)
    {
        List<int> indexes = new List<int>();
        int indexCtr = 0;
        string rtn = String.Empty;

        if (settings.ResponseOpeningOilAndGasQuestionList.Count() > count)
        {
            while (indexes.Count() < count)
            {
                int tmp = rand.Next(0, settings.ResponseOpeningOilAndGasQuestionList.Count() - 1);

                bool found = false;
                foreach (int i in indexes)
                {
                    if (tmp == i)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    indexes.Add(tmp);
                    indexCtr++;
                }
            }

            for (int i = 0; i < indexes.Count(); i++)
            {
                rtn += (i + 1).ToString() + ") " + SettingPostProcessing(settings.ResponseOpeningOilAndGasQuestionList[indexes[i]], new List<string> { }, new List<string> { }, rand) + Environment.NewLine;
            }
        }
        else
        {
            throw new Exception("More Oil and Gas questions requested than setup. Please add more ResponseOpeningOilAndGasQuestionList to the settings.json file.");
        }

        return rtn;
    }
    public string GetRandomLocation(Random rand)
    {
        List<string> lst = settings.Locations;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomProduct(Random rand)
    {
        List<string> lst = settings.Products;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomPaymentMethod(Random rand)
    {
        List<string> lst = settings.PaymentMethods;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomThought(Random rand)
    {
        List<string> lst = settings.RandomThoughts;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomDevice(Random rand)
    {
        List<string> lst = settings.Devices;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomConsignmentBoxImageIncluded(Random rand)
    {
        List<string> lst = settings.ConsignmentBoxImageIncluded;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomConsignmentBoxImageNotIncluded(Random rand)
    {
        List<string> lst = settings.ConsignmentBoxImageNotIncluded;

        return lst[rand.Next(0, lst.Count())];
    }
    public string GetRandomPaymentReceiptPath(Random rand)
    {
        List<string> lst = settings.PathToTransferReceipts;

        return lst[rand.Next(0, lst.Count())];
    }
    #endregion

    //Get Random Questions lists
    #region Random Question lists
    private string GetRandomQuestionsHowAreYou(Random rand)
    {
        List<string> lst = settings.QuestionsHowAreYou;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsJokingAround(Random rand)
    {
        List<string> lst = settings.QuestionsJokingAround;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsNotAnswering(Random rand)
    {
        List<string> lst = settings.QuestionsNotAnswering;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsNotListening(Random rand)
    {
        List<string> lst = settings.QuestionsNotListening;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsNotUnderstanding(Random rand)
    {
        List<string> lst = settings.QuestionsNotUnderstanding;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsPermission(Random rand)
    {
        List<string> lst = settings.QuestionsPermission;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsSpokenLanguage(Random rand)
    {
        List<string> lst = settings.QuestionsSpokenLanguage;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsTrust(Random rand)
    {
        List<string> lst = settings.QuestionsTrust;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAddress(Random rand)
    {
        List<string> lst = settings.QuestionsAddress;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsID(Random rand)
    {
        List<string> lst = settings.QuestionsID;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsPhoneNumber(Random rand)
    {
        List<string> lst = settings.QuestionsPhoneNumber;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsWhyNoAnswer(Random rand)
    {
        List<string> lst = settings.QuestionsWhyNoAnswer;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsCannotOpenAttachment(Random rand)
    {
        List<string> lst = settings.QuestionsCannotOpenAttachment;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAlreadyIncludedID(Random rand)
    {
        List<string> lst = settings.QuestionsAlreadyIncludedID;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsBirthdate(Random rand)
    {
        List<string> lst = settings.QuestionsBirthdate;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsBetterPhoto(Random rand)
    {
        List<string> lst = settings.QuestionsBetterPhoto;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsOccupation(Random rand)
    {
        List<string> lst = settings.QuestionsOccupation;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsGender(Random rand)
    {
        List<string> lst = settings.QuestionsGender;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsMaritalStatus(Random rand)
    {
        List<string> lst = settings.QuestionsMaritalStatus;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsCountry(Random rand)
    {
        List<string> lst = settings.QuestionsCountry;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsName(Random rand)
    {
        List<string> lst = settings.QuestionsName;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsEmail(Random rand)
    {
        List<string> lst = settings.QuestionsEmail;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsPassword(Random rand)
    {
        List<string> lst = settings.QuestionsPassword;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsWeAreCaught(Random rand)
    {
        List<string> lst = settings.QuestionsWeAreCaught;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsProvideDetails(Random rand)
    {
        List<string> lst = settings.QuestionsProvideDetails;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAreYouReady(Random rand)
    {
        List<string> lst = settings.QuestionsAreYouReady;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsChangeContactMethod(Random rand)
    {
        List<string> lst = settings.QuestionsChangeContactMethod;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsContactMeLater(Random rand)
    {
        List<string> lst = settings.QuestionsContactMeLater;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAreYouOnboard(Random rand)
    {
        List<string> lst = settings.QuestionsAreYouOnboard;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsPayTheFee(Random rand)
    {
        List<string> lst = settings.QuestionsPayTheFee;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsTheyConfused(Random rand)
    {
        List<string> lst = settings.QuestionsTheyConfused;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsHowBigOfLoan(Random rand)
    {
        List<string> lst = settings.QuestionsHowBigOfLoan;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsMustPayBefore(Random rand)
    {
        List<string> lst = settings.QuestionsMustPayBefore;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsMeetUs(Random rand)
    {
        List<string> lst = settings.QuestionsMeetUs;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsFillOutForm(Random rand)
    {
        List<string> lst = settings.QuestionsFillOutForm;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsGetBackToUs(Random rand)
    {
        List<string> lst = settings.QuestionsGetBackToUs;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsNeedBankDetails(Random rand)
    {
        List<string> lst = settings.QuestionsNeedBankDetails;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsWhatTypeOfProof(Random rand)
    {
        List<string> lst = settings.QuestionsWhatTypeOfProof;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsHowDoYouWantFundsReleased(Random rand)
    {
        List<string> lst = settings.QuestionsHowDoYouWantFundsReleased;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsWeCantDoThat(Random rand)
    {
        List<string> lst = settings.QuestionsWeCantDoThat;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsContactTheBank(Random rand)
    {
        List<string> lst = settings.QuestionsContactTheBank;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsContactThePerson(Random rand)
    {
        List<string> lst = settings.QuestionsContactThePerson;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAreYouMember(Random rand)
    {
        List<string> lst = settings.QuestionsAreYouMember;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsDidYouSeeOurMessage(Random rand)
    {
        List<string> lst = settings.QuestionsDidYouSeeOurMessage;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsInvalidAddress(Random rand)
    {
        List<string> lst = settings.QuestionsInvalidAddress;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsTellUsWhatTheyAskedYouToDo(Random rand)
    {
        List<string> lst = settings.QuestionsTellUsWhatTheyAskedYouToDo;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsTellUsAboutYourself(Random rand)
    {
        List<string> lst = settings.QuestionsTellUsAboutYourself;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAutomatedProgram(Random rand)
    {
        List<string> lst = settings.QuestionsAutomatedProgram;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsUseWalmartToPay(Random rand)
    {
        List<string> lst = settings.QuestionsUseWalmartToPay;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsHowMuchMoneyDoIHave(Random rand)
    {
        List<string> lst = settings.QuestionsHowMuchMoneyDoIHave;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsSendTransferReceipt(Random rand)
    {
        List<string> lst = settings.QuestionsSendTransferReceipt;

        return lst[rand.Next(0, lst.Count())];
    }
    private string GetRandomQuestionsAlreadyToldYou(Random rand)
    {
        List<string> lst = settings.QuestionsAlreadyToldYou;

        return lst[rand.Next(0, lst.Count())];
    }
    #endregion
}