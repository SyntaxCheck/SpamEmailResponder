using System;
using System.Collections.Generic;
using static ResponseProcessing;

public class CheckFreeMoney : EmailTypeBase
{
    private ResponseSettings Settings { get; set; }

    public CheckFreeMoney(ResponseSettings settings) : base()
    {
        Settings = settings;
        Type = EmailType.FreeMoney;
    }

    public override TypeParseResponse TryTypeParse(LoggerInfo loggerInfo, ref MailStorage currentMessage, List<MailStorage> pastMessages, string preProcessedBody)
    {
        if (PassNumber <= 1)
        {
            if ((Settings.IsAdmin && preProcessedBody.Trim().ToUpper().StartsWith(AutoResponseKeyword)) ||
            preProcessedBody.Trim().ToUpper().Contains("ABANDONED FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("ABANDON FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("AMOUNT OF GRANT") ||
            preProcessedBody.Trim().ToUpper().Contains("ASK HIM TO SEND YOU THE TOTAL") ||
            preProcessedBody.Trim().ToUpper().Contains("ASSIST IN RECEIVING") ||
            preProcessedBody.Trim().ToUpper().Contains("ASSISTANCE TO SET UP MY CHARITY FOUNDATION") ||
            preProcessedBody.Trim().ToUpper().Contains("AWARDED THE SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("BANK CHECK DRAFT") ||
            preProcessedBody.Trim().ToUpper().Contains("CASH GRANT DONATION") ||
            (preProcessedBody.Trim().ToUpper().Contains("CHOOSE AMOUNT") && preProcessedBody.Trim().ToUpper().Contains("$")) ||
            preProcessedBody.Trim().ToUpper().Contains("CHOOSEN TO RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("CHOOSING TO RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("CHOSEN TO RECEIVE $") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM HIS DEPOSITED FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM YOUR BANK DRAFT") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM YOUR CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("CLAIM YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("COLLECT YOU FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("COLLECT YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION AWARD") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION FOR YOUR EFFORTS") ||
            preProcessedBody.Trim().ToUpper().Contains("COMPENSATION OVERDUE PAYMENT") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTACT THE PAYING BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("CONTAINS THE SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("CREDIT YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("CRYPTOCURRENCY FREE") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVER YOUR OWN COMPENSATION") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF THE CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF THE WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("DELIVERY OF YOUR WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("DON ATION") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATE A HUGE AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATE WHAT I HAVE TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATE THE SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATE TO CHARITY THROUGH YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATED") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATING") ||
            preProcessedBody.Trim().ToUpper().Contains("DONATION") ||
            preProcessedBody.Trim().ToUpper().Contains("EFFECT THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("EXPECTING TO RECEIVE IS A CASH") ||
            preProcessedBody.Trim().ToUpper().Contains("FREE CASH GRANT") ||
            preProcessedBody.Trim().ToUpper().Contains("FREE CRYPTOCURRENCY") ||
            preProcessedBody.Trim().ToUpper().Contains("FREE GRANT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND BELONGING TO MY DECEASED CLIENT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND IN A CASHIER CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND IN A CASHIER CHEQUE") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND IS BEING RELEASED TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND THE SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("FUND TRANSFER") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS HAS BEEN ORDERED") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS IN A CASHIER CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS IN A CASHIER CHEQUE") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS TO YOU CONTACT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS TO YOU, CONTACT") ||
            preProcessedBody.Trim().ToUpper().Contains("FUNDS TO YOUR CONTACT") ||
            preProcessedBody.Trim().ToUpper().Contains("GRANT YOU LIKE TO APPLY") ||
            preProcessedBody.Trim().ToUpper().Contains("GREATLY IN NEED OF AN INDIVIDUAL WHO CAN HANDLE") ||
            preProcessedBody.Trim().ToUpper().Contains("GREATLY IN NEED OF INDIVIDUAL WHO CAN HANDLE") ||
            preProcessedBody.Trim().ToUpper().Contains("HAVE A PACKAGE OF $") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE WILLED £") ||
            preProcessedBody.Trim().ToUpper().Contains("I HAVE WILLED $") ||
            preProcessedBody.Trim().ToUpper().Contains("I WANT TO DISTRIBUTE MY $") ||
            preProcessedBody.Trim().ToUpper().Contains("I WISH TO BEQUEATH YOU IN SPECIES THIS SU M") || //How can we possibly predict emails with wording/grammer like this?
            preProcessedBody.Trim().ToUpper().Contains("I WISH TO WILL $") ||
            preProcessedBody.Trim().ToUpper().Contains("I WISH TO WILL US$") ||
            preProcessedBody.Trim().ToUpper().Contains("I WISH TO WILL USD$") ||
            preProcessedBody.Trim().ToUpper().Contains("IMMEDIATE WITHDRAWAL OF YOUR CASH FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("IN REGARDS TO YOUR DRAFT OF $") ||
            preProcessedBody.Trim().ToUpper().Contains("IN REGARDS TO YOUR DRAFT OF US") ||
            preProcessedBody.Trim().ToUpper().Contains("INCOMING TRANSFER NOTIFICATION") ||
            preProcessedBody.Trim().ToUpper().Contains("INSTANT RICH SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERESTED IN GRANT FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERIGENS FUND WHORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("INTERNATIONAL CASHIER'S BANK DRAFT, TO THE TUNE") ||
            preProcessedBody.Trim().ToUpper().Contains("INVEST THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("INVOLVING THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("KEPT THE CHECK WITH THEM") ||
            preProcessedBody.Trim().ToUpper().Contains("KEPT THE CHEQUE WITH THEM") ||
            preProcessedBody.Trim().ToUpper().Contains("MAPPED OUT A COMPENSATION") ||
            preProcessedBody.Trim().ToUpper().Contains("MONETARY FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("MONEY WILL BE TRANSFER") ||
            preProcessedBody.Trim().ToUpper().Contains("OFFERED YOU $") ||
            preProcessedBody.Trim().ToUpper().Contains("OFFERED YOU WITH $") ||
            preProcessedBody.Trim().ToUpper().Contains("ON YOUR FAVOR A DRAFT WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("PARTNERSHIP FOR THE TRANSFER") ||
            preProcessedBody.Trim().ToUpper().Contains("PARTNERSHIP TO TRANSFER") ||
            preProcessedBody.Trim().ToUpper().Contains("PROCESSING OF FUND TRANSFER") ||
            preProcessedBody.Trim().ToUpper().Contains("PROMISE TO PAY THE SOM") ||
            preProcessedBody.Trim().ToUpper().Contains("PROMISE TO PAY THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("REASSIGN IN YOUR FAVOR") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THIS FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THIS MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THIS SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE THIS WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVE YOUR PROFIT") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVED THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVED THIS FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVED THIS MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVED THIS SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVED THIS WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVED YOUR PROFIT") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVING THEIR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVING YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVING YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVING YOUR SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("RECEIVING YOUR WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("RECOVERY SUM AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("RECLAIM THE MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("REFLECT IN YOUR BANK") ||
            preProcessedBody.Trim().ToUpper().Contains("REGARDS TO YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE FUND TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE OF THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE OF THIS FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE SOME FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE YOUR DRAFT") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE YOUR CASH") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE YOUR CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASE YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("RELEASED TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("SECURED SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("SEND YOU THE REST OF MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("SO HE CAN RELEASE YOUR DRAFT TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("SOLUTION TO A MONEY TRANSFER") ||
            preProcessedBody.Trim().ToUpper().Contains("STILL WANT YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("THAT WAS AWARDED TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("THE MONEY IS HUGE") ||
            preProcessedBody.Trim().ToUpper().Contains("THE TRANSMISSION OF THE FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("THOSE FUNDS TRANSFERRED") ||
            preProcessedBody.Trim().ToUpper().Contains("TO BE COMPENSATED") ||
            preProcessedBody.Trim().ToUpper().Contains("TO YOUR BANK ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("TOTAL SUM OF") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSACTION WE WERE PURSING TOGETHER") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF YOUR SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OF YOUR WEALTH") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER OUR CASH") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER SUM OF $") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFER TO YOUR ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRED THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRED TO YOUR OWN PERSONAL ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRING OF THIS FUND SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRING YOUR FUNDS") ||
            preProcessedBody.Trim().ToUpper().Contains("TRANSFERRING YOUR MONEY") ||
            preProcessedBody.Trim().ToUpper().Contains("TRUTH ABOUT YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("UNCLAIMED ACCOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("UNCLAIMED FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("UNPAID FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("USD WAS DANATED TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("USD WAS DONATED TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WE HAVE DECIDED TO DONATE THE SUM") ||
            preProcessedBody.Trim().ToUpper().Contains("WILL THIS FORTUNE TO YOU") ||
            preProcessedBody.Trim().ToUpper().Contains("WITH THE SUM AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU ARE ELIGIBLE TO RECEIVE YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU CAN RECEIVE YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU HAVE AN UNCLAIMED FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU WILL BE RECEIVING THE FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOU WILL RECEIVE YOUR FUND") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR CASHIER'S CHECK") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR DELIVERY WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUND AMOUNT") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUND RELEASE") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUND TRANSFER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUND WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUNDS TO BE PAID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUNDS TRANSFER") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUNDS VALUED") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR FUNDS WILL BE PAID") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR PACKAGE WORTH") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR SHARE/COMPENSATION") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR SHARECOMPENSATION") ||
            preProcessedBody.Trim().ToUpper().Contains("YOUR WIRE TRANSFER") ||
            (preProcessedBody.Trim().ToUpper().Contains("FUND") && preProcessedBody.Trim().ToUpper().Contains("URGENT DELIVERY")) ||
            (preProcessedBody.Trim().ToUpper().Contains("ASSIGNED TO BE DELIVERED") && preProcessedBody.Trim().ToUpper().Contains("$")) ||
            (preProcessedBody.Trim().ToUpper().Contains("FUND") && preProcessedBody.Trim().ToUpper().Contains("UNCLAIMED") && preProcessedBody.Trim().ToUpper().Contains("DEPOSITED")) ||
            (preProcessedBody.Trim().ToUpper().Contains("OF THIS MONEY") && preProcessedBody.Trim().ToUpper().Contains("OFFER YOU")) ||
            (preProcessedBody.Trim().ToUpper().Contains("DONATE $") && preProcessedBody.Trim().ToUpper().Contains("TO YOU")))
            {
                base.ParseResponse.IsMatch = true;
                base.ParseResponse.TotalHits++;
            }
        }
        else if (PassNumber == 2)
        {
            List<string> moneyWords = new List<string>() { "FUND", "CASH", "CHECK", "SUM", "WEALTH" };
            List<string> actionWords = new List<string>() { "RELEASE", "RECEIVE", "GIVEN", "PAID", "TRANSFER", "OFFER", "CLAIM", "COMPENSATE", "INVEST", "GRANT" };

            foreach (string s in moneyWords)
            {
                foreach (string s2 in actionWords)
                {
                    if (preProcessedBody.Trim().ToUpper().Contains(s) && preProcessedBody.Trim().ToUpper().Contains(s2))
                    {
                        base.ParseResponse.IsMatch = true;
                        base.ParseResponse.TotalHits++;

                        break;
                    }
                }
            }
        }

        return base.ParseResponse;
    }
}