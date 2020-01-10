using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckForeignLanguage : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckForeignLanguage(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.ForeignLanguage;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("À ÉTÉ") ||
            preProcessedBody.Trim().ToUpper().Contains("ÊTES") ||
            preProcessedBody.Trim().Contains("ß") ||
            preProcessedBody.Trim().ToUpper().Contains("ı") ||
            preProcessedBody.Trim().ToUpper().Contains("Ő") ||
            preProcessedBody.Trim().Contains("ő") ||
            preProcessedBody.Trim().ToUpper().Contains("Ű") ||
            preProcessedBody.Trim().Contains("ű") ||
            preProcessedBody.Trim().Contains("Ӳ") ||
            preProcessedBody.Trim().Contains("ӳ") ||
            preProcessedBody.Trim().ToUpper().Contains("AFIN QUE VOS") ||
            preProcessedBody.Trim().ToUpper().Contains("AHN-YOUNG-HA-SE-YO") ||
            preProcessedBody.Trim().ToUpper().Contains("ANNAK BARMELY") ||
            preProcessedBody.Trim().ToUpper().Contains("BEHOEFTE") ||
            preProcessedBody.Trim().ToUpper().Contains("BIEN POUR") ||
            preProcessedBody.Trim().ToUpper().Contains("BONJOUR") ||
            preProcessedBody.Trim().ToUpper().Contains("BUENOS DÍAS") ||
            preProcessedBody.Trim().ToUpper().Contains("CAS OU VOTRE") ||
            preProcessedBody.Trim().ToUpper().Contains("CHARGÉ") ||
            preProcessedBody.Trim().ToUpper().Contains("CIAO") ||
            preProcessedBody.Trim().ToUpper().Contains("CRÉDIT") ||
            preProcessedBody.Trim().ToUpper().Contains("DASS SIE") ||
            preProcessedBody.Trim().ToUpper().Contains("DE LUTTER") ||
            preProcessedBody.Trim().ToUpper().Contains("DE VOS RÊVES") ||
            preProcessedBody.Trim().ToUpper().Contains("DES PAYS DE") ||
            preProcessedBody.Trim().ToUpper().Contains("DICH ALS") ||
            preProcessedBody.Trim().ToUpper().Contains("DSCH") ||
            preProcessedBody.Trim().ToUpper().Contains("EEUW") ||
            preProcessedBody.Trim().ToUpper().Contains("EL BANCO") ||
            preProcessedBody.Trim().ToUpper().Contains("ESTE LE") ||
            preProcessedBody.Trim().ToUpper().Contains("ET GROS") ||
            preProcessedBody.Trim().ToUpper().Contains("ETTÄ") ||
            preProcessedBody.Trim().ToUpper().Contains("EZ AZ ") ||
            preProcessedBody.Trim().ToUpper().Contains("FÜR") ||
            preProcessedBody.Trim().ToUpper().Contains("GUTEN TAG") ||
            preProcessedBody.Trim().ToUpper().Contains("HABARI") ||
            preProcessedBody.Trim().ToUpper().Contains("HABEN") ||
            preProcessedBody.Trim().ToUpper().Contains("HALLO") ||
            preProcessedBody.Trim().ToUpper().Contains("HOLA") ||
            preProcessedBody.Trim().ToUpper().Contains("ICH BIN") ||
            preProcessedBody.Trim().ToUpper().Contains("IEUW") ||
            preProcessedBody.Trim().ToUpper().Contains("JAMBO") ||
            preProcessedBody.Trim().ToUpper().Contains("JE ME") ||
            preProcessedBody.Trim().ToUpper().Contains("KONBAN WA") ||
            preProcessedBody.Trim().ToUpper().Contains("KONNICHIWA") ||
            preProcessedBody.Trim().ToUpper().Contains("LA TUA") ||
            preProcessedBody.Trim().ToUpper().Contains("MARHABA") ||
            preProcessedBody.Trim().ToUpper().Contains("MERHABA") ||
            preProcessedBody.Trim().ToUpper().Contains("NAMASTE") ||
            preProcessedBody.Trim().ToUpper().Contains("NAY HOH") ||
            preProcessedBody.Trim().ToUpper().Contains("NEM AZ ") ||
            preProcessedBody.Trim().ToUpper().Contains("NI HAU") ||
            preProcessedBody.Trim().ToUpper().Contains("NO ME GUSTA") ||
            preProcessedBody.Trim().ToUpper().Contains("OHAYO") ||
            preProcessedBody.Trim().ToUpper().Contains("OLÀ") ||
            preProcessedBody.Trim().ToUpper().Contains("ONS DAN EEN") ||
            preProcessedBody.Trim().ToUpper().Contains("OU ZE5 OU") ||
            preProcessedBody.Trim().ToUpper().Contains("PARA TODAS") ||
            preProcessedBody.Trim().ToUpper().Contains("POR ESTE") ||
            preProcessedBody.Trim().ToUpper().Contains("POR FAVOR") ||
            preProcessedBody.Trim().ToUpper().Contains("PREGA DI") ||
            preProcessedBody.Trim().ToUpper().Contains("PROSENTIN") ||
            preProcessedBody.Trim().ToUpper().Contains("QUE JE") ||
            preProcessedBody.Trim().ToUpper().Contains("QUI NOUS") ||
            preProcessedBody.Trim().ToUpper().Contains("SAIN BAINUU") ||
            preProcessedBody.Trim().ToUpper().Contains("SALAAM") ||
            preProcessedBody.Trim().ToUpper().Contains("SALAMA ALEIKUM") ||
            preProcessedBody.Trim().ToUpper().Contains("SALEMETSIZ BE") ||
            preProcessedBody.Trim().ToUpper().Contains("SALUT MON") ||
            preProcessedBody.Trim().ToUpper().Contains("SANNU") ||
            preProcessedBody.Trim().ToUpper().Contains("SE JOUE") ||
            preProcessedBody.Trim().ToUpper().Contains("SIE IHR") ||
            preProcessedBody.Trim().ToUpper().Contains("SIE MIT") ||
            preProcessedBody.Trim().ToUpper().Contains("SOCIÉTÉS") ||
            preProcessedBody.Trim().ToUpper().Contains("STUUR ONS") ||
            preProcessedBody.Trim().ToUpper().Contains("SZIA") ||
            preProcessedBody.Trim().ToUpper().Contains("TERVEISIÄ") ||
            preProcessedBody.Trim().ToUpper().Contains("TSCH") ||
            preProcessedBody.Trim().ToUpper().Contains("UNO DE ") ||
            preProcessedBody.Trim().ToUpper().Contains("UN HOMME") ||
            preProcessedBody.Trim().ToUpper().Contains("WENN JA") ||
            preProcessedBody.Trim().ToUpper().Contains("WERDEN") ||
            preProcessedBody.Trim().ToUpper().Contains("WIE IST DEINE") ||
            preProcessedBody.Trim().ToUpper().Contains("VOTRE") ||
            preProcessedBody.Trim().ToUpper().Contains("ZDRAS-TVUY-TE") ||
            !TextProcessing.IsEnglish(preProcessedBody.Trim()))
        {
            base.ParseResponse.IsMatch = true;
            base.ParseResponse.TotalHits++;
        }

        return base.ParseResponse;
    }
}